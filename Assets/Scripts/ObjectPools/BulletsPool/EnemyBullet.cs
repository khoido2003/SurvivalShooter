using UnityEngine;

public class EnemyBullet : Bullet
{
    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        Player player = collision.gameObject.GetComponent<Player>();

        if (player != null)
        {
            Debug.Log("Shot the player");
        }
    }
}
