using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPointAtMouse : MonoBehaviour {
    private Vector3 _mousePoint3D;
    private Camera _mainCam;

    private void Awake() {
        _mainCam = Camera.main;
    }

    private void Update()
    {
        PointAtMouse();
    }

    private void PointAtMouse()
    {
        _mousePoint3D = _mainCam.ScreenToWorldPoint(Input.mousePosition + Vector3.back * Camera.main.transform.position.z);
        transform.LookAt(_mousePoint3D, Vector3.back);
    }
}
