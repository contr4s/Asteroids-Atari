using System.Collections;
using System.Collections.Generic;
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

        var explosion = GameManager.S.explosionsPool.GetAvailableObject();
        explosion.transform.position = transform.position;
        explosion.gameObject.SetActive(true);
        explosion.Play();

        GameManager.S.audioManager.PlayExplosionSound();
    }

    private IEnumerator ShootingAtPlayerShip(float reload)
    {
        while(true)
        {
            var projectile = _projectilePool.GetAvailableObject();

            projectile.CreatedByPlayer = false;
            projectile.transform.position = transform.position;
            projectile.transform.LookAt(PlayerShip.position);
            projectile.transform.position += projectile.transform.forward;
            projectile.gameObject.SetActive(true);

            yield return new WaitForSeconds(reload);
        }
    }
}
