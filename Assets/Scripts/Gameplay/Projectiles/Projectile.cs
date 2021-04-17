using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile: MonoBehaviour, IDestroyable
{
    [SerializeField] private float bulletSpeed = 20;
    [SerializeField] private float lifeTime = 2;

    private Rigidbody _rigidbody;
    private Coroutine _lifeCycle;

    public bool createdByPlayer = true;
    public bool CreatedByPlayer { 
        get => createdByPlayer;
        set => createdByPlayer = value; 
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _lifeCycle = StartCoroutine(LifeCycle());

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
        StopCoroutine(_lifeCycle);
    }

    private IEnumerator LifeCycle()
    {
        yield return new WaitForSeconds(lifeTime);
        DestroyMe();
    }
}
