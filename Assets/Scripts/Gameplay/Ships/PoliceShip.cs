using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceShip: MonoBehaviour, IDestroyable
{
    [SerializeField] private float _speed;

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, PlayerShip.position, _speed * Time.deltaTime);
    }

    public void DestroyMe()
    {
        gameObject.SetActive(false);
    }
}
