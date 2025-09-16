using System.Collections.Generic;
using UnityEngine;

public enum CoverPerk
{
    Unavailable,
    CanTakeCover,
    CanTakeAndChangeCover,
}

public class EnemyRange : Enemy
{
    [Header("Enemy Perk")]
    public CoverPerk coverPerk;

    [Header("Advance Perk")]
    public float advanceSpeed;
    public float advanceStoppingDistance;
    public float advanceTime = 2.5f;

    [Header("Cover System")]
    public CoverPoint lastCover { get; private set; }
    public CoverPoint currentCover { get; private set; }
    public bool canUseCover = true;
    public float safeDistance;
    public float minCoverTime;

    [Header("Weapon Details")]
    public EnemyRangeWeaponModelType weaponModelType;
    public EnemyRangeWeaponData enemyRangeWeaponData;

    [Header("Aim Details")]
    public float slowAim = 4;
    public float fastAim = 20;
    public Transform aim;
    public Transform playerBody;
    public LayerMask whatToIgnore;

    public EnemyRangeIdleState idleState { get; private set; }
    public EnemyRangeMoveState moveState { get; private set; }
    public EnemyRangeBattleState battleState { get; private set; }
    public EnemyRangeCoverState coverState { get; private set; }
    public EnemyRangeAdvanceState advanceState { get; private set; }

    public Transform weaponHolder;
    public Transform gunPoint;

    [SerializeField]
    List<EnemyRangeWeaponData> availableWeaponData;

    protected override void Awake()
    {
        base.Awake();

        idleState = new EnemyRangeIdleState(this, stateMachine, "Idle");
        moveState = new EnemyRangeMoveState(this, stateMachine, "Move");
        battleState = new EnemyRangeBattleState(this, stateMachine, "Battle");
        coverState = new EnemyRangeCoverState(this, stateMachine, "RunToCover");
        advanceState = new EnemyRangeAdvanceState(this, stateMachine, "Advance");
    }

    protected override void Start()
    {
        base.Start();

        playerBody = player.GetComponent<Player>().playerBody;
        aim.parent = null;
        enemyVisual.SetupLook();

        SetupWeaponData();
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();

        UpdateAimPosition();
    }

    public override void EnterBattleMode()
    {
        if (inBattleMode)
        {
            return;
        }

        base.EnterBattleMode();

        if (CanGetCover())
        {
            stateMachine.ChangeState(coverState);
        }
        else
        {
            stateMachine.ChangeState(battleState);
        }
    }

    public void FireSingleBullet()
    {
        animator.SetTrigger("Shoot");

        Vector3 bulletDirection = (aim.position - gunPoint.position).normalized;

        EnemyBullet newBullet = PoolManager.Instance.Get<EnemyBullet>();

        newBullet.transform.position = gunPoint.position;
        newBullet.transform.rotation = Quaternion.LookRotation(gunPoint.forward);

        newBullet.BulletSetup();

        Rigidbody rbNewBullet = newBullet.GetRigidbody();

        Vector3 bulletWithSpreadDirection = enemyRangeWeaponData.ApplySpread(bulletDirection);

        rbNewBullet.mass = 20 / enemyRangeWeaponData.bulletSpeed;
        rbNewBullet.linearVelocity = bulletWithSpreadDirection * enemyRangeWeaponData.bulletSpeed;
    }

    private void SetupWeaponData()
    {
        List<EnemyRangeWeaponData> filteredData = new();

        foreach (var weaponData in availableWeaponData)
        {
            if (weaponData.weaponModelType == weaponModelType)
            {
                filteredData.Add(weaponData);
            }
        }

        if (filteredData.Count <= 0)
        {
            Debug.LogError("No Enemy Range Weapon Data available!");
            return;
        }

        int random = Random.Range(0, filteredData.Count);
        enemyRangeWeaponData = filteredData[random];

        gunPoint = enemyVisual.currentWeaponModel.GetComponent<EnemyRangeWeaponModel>().gunPoint;
    }

    #region Enemt Aim Region

    public bool AimOnPlayer()
    {
        float distanceAimToPlayer = Vector3.Distance(aim.position, player.transform.position);

        return distanceAimToPlayer < 2;
    }

    public void UpdateAimPosition()
    {
        float aimSpeed = AimOnPlayer() ? fastAim : slowAim;

        // Smoothly interpolate toward player
        aim.position = Vector3.Lerp(aim.position, playerBody.position, aimSpeed * Time.deltaTime);
    }

    public bool IsSeeingPlayer()
    {
        Vector3 myPosition = transform.position + Vector3.up;

        Vector3 directionToPlayer = playerBody.position - myPosition;

        if (
            Physics.Raycast(
                myPosition,
                directionToPlayer,
                out RaycastHit hit,
                Mathf.Infinity,
                whatToIgnore
            )
        )
        {
            if (hit.transform == player)
            {
                UpdateAimPosition();
                return true;
            }
        }
        return false;
    }

    #endregion


    #region Cover System

    public bool CanGetCover()
    {
        if (coverPerk == CoverPerk.Unavailable)
        {
            return false;
        }

        currentCover = AttemptToFindCover()?.GetComponent<CoverPoint>();

        if (lastCover != currentCover && currentCover != null)
        {
            return true;
        }

        Debug.Log("No Cover Found!");
        return false;
    }

    public Transform AttemptToFindCover()
    {
        List<CoverPoint> collectedCoverPoints = new();

        foreach (Cover cover in CollectNearByCover())
        {
            collectedCoverPoints.AddRange(cover.GetValidCoverPoints(transform));
        }

        CoverPoint closestCoverpoint = null;

        float shortestDistance = float.MaxValue;

        foreach (CoverPoint coverPoint in collectedCoverPoints)
        {
            float currentDistance = Vector3.Distance(
                transform.position,
                coverPoint.transform.position
            );

            if (currentDistance < shortestDistance)
            {
                closestCoverpoint = coverPoint;

                shortestDistance = currentDistance;
            }
        }

        if (closestCoverpoint != null)
        {
            lastCover?.SetOccupied(false);
            lastCover = currentCover;

            currentCover = closestCoverpoint;

            currentCover.SetOccupied(true);

            return currentCover.transform;
        }

        return null;
    }

    private List<Cover> CollectNearByCover()
    {
        float coverRadiusCheck = 30;

        Collider[] hitCollider = Physics.OverlapSphere(transform.position, coverRadiusCheck);

        List<Cover> collectedCovers = new();

        foreach (Collider collider in hitCollider)
        {
            Cover cover = collider.GetComponent<Cover>();

            if (cover != null && !collectedCovers.Contains(cover))
            {
                collectedCovers.Add(cover);
            }
        }

        return collectedCovers;
    }

    #endregion

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.blueViolet;
        Gizmos.DrawLine(transform.position, player.transform.position);
    }
}
