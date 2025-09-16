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

        if (player != null) { }

        PoolManager.Instance.Return<Bullet>(this);
    }
}
