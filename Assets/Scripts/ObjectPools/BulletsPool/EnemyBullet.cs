using UnityEngine;

public class EnemyBullet : Bullet
{
    protected override void OnCollisionEnter(Collision collision)
    {
        CreateImpactBulletFx(collision);

        Enemy enemy = collision.gameObject.GetComponent<Enemy>();

        if (enemy != null)
        {
            return;
        }

        Player player = collision.gameObject.GetComponent<Player>();

        if (player != null)
        {
            Debug.Log("Shot the player");
        }

        PoolManager.Instance.Return<Bullet>(this);
    }
}
