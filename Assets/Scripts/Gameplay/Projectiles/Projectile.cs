using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    public float bulletSpeed = 20;
    public float lifeTime = 2;

    private Rigidbody _rigidbody;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable() {
        // Set Bullet to self-destruct in lifeTime seconds
        Invoke(nameof(DestroyMe), lifeTime);

        // Set the velocity of the Bullet
        _rigidbody.velocity = transform.forward * bulletSpeed;
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "asteroid") {
            Destroy(other.gameObject);
            DestroyMe();
        }
    }

    void DestroyMe() {
        gameObject.SetActive(false);
        CancelInvoke(nameof(DestroyMe));
    }
}
