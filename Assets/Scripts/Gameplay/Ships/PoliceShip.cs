using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceShip: MonoBehaviour, IDestroyable
{
    [SerializeField] private float _speed;
    [SerializeField] private int _rewardForDestroy;

    public bool CreatedByPlayer => false;

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
    }
}
