using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile: MonoBehaviour, IDestroyable
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
        if (other.TryGetComponent(out IDestroyable destroyableObject))
        {
            destroyableObject.DestroyMe();
            DestroyMe();
        }
    }

    public void DestroyMe()
    {
        gameObject.SetActive(false);
        CancelInvoke(nameof(DestroyMe));
    }
}
