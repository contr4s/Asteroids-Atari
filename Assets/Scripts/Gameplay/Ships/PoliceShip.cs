using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ProjectilePool))]
public class PoliceShip: MonoBehaviour, IDestroyable
{
    [SerializeField] private float _speed;
    [SerializeField] private float _reloadTime;
    [SerializeField] private int _rewardForDestroy;

    private ProjectilePool _projectilePool;

    public bool CreatedByPlayer => false;

    private void Awake()
    {
        _projectilePool = GetComponent<ProjectilePool>();
    }

    private void OnEnable()
    {
        StartCoroutine(ShootingAtPlayerShip(_reloadTime));
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, PlayerShip.position, _speed * Time.deltaTime);
    }

    public void DestroyMe(eDestroyedBy destroyedBy = eDestroyedBy.none)
    {
        gameObject.SetActive(false);

        if (destroyedBy == eDestroyedBy.Projectile)
        {
            GameManager.S.Score += _rewardForDestroy;
        }

        ParticleSystem explosion = GameManager.S.explosionsPool.GetAvailableObject();
        GameManager.S.explosionsPool.InitExplosion(explosion, transform.position);
    }

    private IEnumerator ShootingAtPlayerShip(float reload)
    {
        yield return new WaitForSeconds(reload);

        while (true)
        {
            Projectile projectile = _projectilePool.GetAvailableObject();
            _projectilePool.InitProjectile(projectile, false, transform.position, PlayerShip.position);

            yield return new WaitForSeconds(reload);
        }
    }
}
