using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile: MonoBehaviour
{
    public float bulletSpeed = 20;
    public float lifeTime = 2;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        Invoke(nameof(DestroyMe), lifeTime);

        _rigidbody.velocity = transform.forward * bulletSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Asteroid>(out Asteroid asteroid))
        {
            asteroid.DestroyMe();
            DestroyMe();
        }
    }

    void DestroyMe()
    {
        gameObject.SetActive(false);
        CancelInvoke(nameof(DestroyMe));
    }
}
