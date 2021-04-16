using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Asteroid: MonoBehaviour, IDestroyable
{
    public float size;
    public int generation;

    private Rigidbody _rigidbody;

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
            policeShip.DestroyMe();
        }
    }

    public void DestroyMe()
    {
        gameObject.SetActive(false);
        GameManager.S.SpawnChildAsteroid(generation + 1, transform.position);
    }

    private void InitVelocity()
    {
        Vector3 vel = Random.insideUnitCircle;
        vel.Normalize();
        vel *= Random.Range(AsteroidsScriptableObject.S.minVel, AsteroidsScriptableObject.S.maxVel) / size;


        _rigidbody.velocity = vel;
        _rigidbody.angularVelocity = Random.insideUnitSphere * AsteroidsScriptableObject.S.maxAngularVel;
    }
}
