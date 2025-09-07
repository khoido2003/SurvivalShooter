using UnityEngine;

public class EnemyShield : MonoBehaviour
{
    [SerializeField]
    private int durability;

    private EnemyMelee enemy;

    private void Awake()
    {
        enemy = GetComponentInParent<EnemyMelee>();
    }

    public void ReduceDurability()
    {
        durability--;

        Debug.Log("Hit shield");
        if (durability <= 0)
        {
            // Chase animation without shield
            enemy.animator.SetFloat("chaseIndex", 0);
            Destroy(gameObject);
        }
    }
}
