using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Asteroid: MonoBehaviour, IDestroyable
{
    public float size;
    public int generation;

    private Rigidbody _rigidbody;


    public bool CreatedByPlayer => false;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        transform.localScale = Vector3.one * size;
        InitVelocity();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out PoliceShip policeShip))
        {
            policeShip.DestroyMe(eDestroyedBy.Asteroid);
        }
    }

    public void DestroyMe(eDestroyedBy destroyedBy = eDestroyedBy.none)
    {
        if (destroyedBy == eDestroyedBy.Projectile)
        {
            GameManager.S.Score += GameManager.S.asteroidsSO.pointsForAsteroidGeneration[generation];
        }

        var explosion = GameManager.S.explosionsPool.GetAvailableObject();
        explosion.transform.position = transform.position;
        explosion.transform.localScale = Vector3.one * size;
        explosion.gameObject.SetActive(true);
        explosion.Play();

        GameManager.S.audioManager.PlayExplosionSound();

        gameObject.SetActive(false);
        GameManager.S.SpawnChildAsteroid(generation + 1, transform.position);
    }

    private void InitVelocity()
    {
        Vector3 vel = Random.insideUnitCircle;
        vel.Normalize();
        vel *= Random.Range(GameManager.S.asteroidsSO.minVel, GameManager.S.asteroidsSO.maxVel) / size;


        _rigidbody.velocity = vel;
        _rigidbody.angularVelocity = Random.insideUnitSphere * GameManager.S.asteroidsSO.maxAngularVel;
    }
}
