using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Asteroid: MonoBehaviour
{
    [SerializeField] private float _size;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        transform.localScale = Vector3.one * _size;
        InitVelocity();
    }

    private void InitVelocity()
    {
        Vector3 vel = Random.insideUnitCircle;
        vel.Normalize();
        vel *= Random.Range(AsteroidsScriptableObject.S.minVel, AsteroidsScriptableObject.S.maxVel) / _size;


        _rigidbody.velocity = vel;
        _rigidbody.angularVelocity = Random.insideUnitSphere * AsteroidsScriptableObject.S.maxAngularVel;
    }
}
