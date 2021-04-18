using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Asteroid: MonoBehaviour, IDestroyable
{
    [HideInInspector] public float size = 1;
    [HideInInspector] public int generation = 0;

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
        GameManager.S.explosionsPool.InitExplosion(explosion, transform.position, size);

        gameObject.SetActive(false);
        GameManager.S.asteroidsManager.SpawnChildAsteroid(generation + 1, transform.position);
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
