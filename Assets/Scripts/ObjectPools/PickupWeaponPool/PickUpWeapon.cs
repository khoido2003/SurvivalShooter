using UnityEngine;

public class PickUpWeapon : Interactable, IObjectItemPoolable
{
    [SerializeField]
    private WeaponData weaponData;

    [SerializeField]
    private BackUpWeaponModel[] backUpWeaponModels;

    [SerializeField]
    private Weapon weapon;

    private PlayerWeaponController playerWeaponController;

    private bool isOldWeapon;

    private void Start() { }

    // Call everytime this object is set active again
    private void OnEnable()
    {
        if (!isOldWeapon)
        {
            weapon = new Weapon(weaponData);
            UpdateGameObject();
        }
    }

    public void UpdateGameObject()
    {
        gameObject.name = "PickUpWeapon_" + weaponData.weaponType.ToString();

        UpdateItemModel();
    }

    public void SetupPickupWeapon(Weapon weapon, Transform transform)
    {
        // If this is an old weapon dropped by player => keep the old weapon data
        // If not then will create a new weapon at the start since this will be called before Start method
        isOldWeapon = true;

        this.weapon = weapon;
        weaponData = weapon.weaponData;
        this.transform.position = transform.position;

        // Force update after new data come in
        UpdateGameObject();
    }

    public void UpdateItemModel()
    {
        foreach (BackUpWeaponModel backUpWeaponModel in backUpWeaponModels)
        {
            backUpWeaponModel.gameObject.SetActive(false);

            if (backUpWeaponModel.weaponType == weapon.weaponType)
            {
                backUpWeaponModel.gameObject.SetActive(true);

                UpdateMeshAndMaterial(backUpWeaponModel.GetComponent<MeshRenderer>());
            }
        }
    }

    public override void Interaction()
    {
        playerWeaponController.PickUpWeapon(weapon);

        // After interaction, remove the weapon from the scene and send back to the pool
        PoolManager.Instance.Return<PickUpWeapon>(this);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (playerWeaponController == null)
        {
            playerWeaponController = other.GetComponent<PlayerWeaponController>();
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
    }

    public void OnSpawn()
    {
        UpdateGameObject();
    }

    public void OnDespawn()
    {
        UpdateGameObject();
    }
}
