using UnityEngine;

public class ProjectilePool: ObjectPool<Projectile>
{
    public void InitProjectile(Projectile projectile, bool createdByPlayer, Vector3 position, Vector3 target)
    {
        projectile.CreatedByPlayer = createdByPlayer;
        projectile.transform.position = position;
        projectile.transform.LookAt(target);
        projectile.transform.position += projectile.transform.forward;
        projectile.gameObject.SetActive(true);

        if (createdByPlayer)
            GameManager.S.audioManager.PlayShootSound();
    }
}
