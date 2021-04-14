using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerShip : Singleton<PlayerShip>
{
    [Header("Set in Inspector")]
    public float shipSpeed = 10f;
    public GameObject bulletPrefab;

    Rigidbody _rigid;

    protected override void Awake()
    {
        base.Awake();

        _rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float aX = Input.GetAxis("Horizontal");
        float aY = Input.GetAxis("Vertical");

        Vector3 vel = new Vector3(aX, aY);
        if (vel.magnitude > 1)
        {
            // Avoid speed multiplying by sqrt(2) when moving at a diagonal
            vel.Normalize();
        }

        _rigid.velocity = vel * shipSpeed;

        if (Input.GetButtonDown("Fire1"))
        {
            //TO DO: Fire();
        }
    }


    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "asteroid")
        {
            //TO DO: extraxt life
        }
    }

    void Fire()
    {
        Vector3 mPos = Input.mousePosition;
        mPos.z = -Camera.main.transform.position.z;
        Vector3 mPos3D = Camera.main.ScreenToWorldPoint(mPos);

        GameObject go = Instantiate<GameObject>(bulletPrefab);
        go.transform.position = transform.position;
        go.transform.LookAt(mPos3D);
    }
}
