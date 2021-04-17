using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile: MonoBehaviour, IDestroyable
{
    [SerializeField] private float bulletSpeed = 20;

    private Rigidbody _rigidbody;

    public bool CreatedByPlayer { get; set; }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _rigidbody.velocity = transform.forward * bulletSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDestroyable destroyableObject))
        {
            destroyableObject.DestroyMe(eDestroyedBy.Projectile);
            DestroyMe();
        }
    }

    public void DestroyMe(eDestroyedBy destroyedBy = eDestroyedBy.none)
    {
        gameObject.SetActive(false);
    }
}
