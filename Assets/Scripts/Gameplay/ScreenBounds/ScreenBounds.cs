using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ScreenBounds: MonoBehaviour
{
    public float zScale = 10;

    private static Camera cam;
    private static BoxCollider boxColl;


    void Awake()
    {
        cam = Camera.main;
        // Need to make sure that the camera is Orthographic for this to work
        if (!cam.orthographic)
        {
            Debug.LogError("Camera.main needs to be orthographic ");
        }

        boxColl = GetComponent<BoxCollider>();
        boxColl.size = Vector3.one;

        transform.position = Vector3.zero;
        transform.localScale = CalculateScale();
    }

    private Vector3 CalculateScale()
    {
        Vector3 scaleDesired;

        scaleDesired.z = zScale;
        scaleDesired.y = cam.orthographicSize * 2;
        scaleDesired.x = scaleDesired.y * cam.aspect;

        return scaleDesired.ComponentDivide(cam.transform.localScale);
    }

    public static Vector3 RANDOM_ON_SCREEN_LOC
    {
        get
        {
            Vector3 min = boxColl.bounds.min;
            Vector3 max = boxColl.bounds.max;
            Vector3 loc = new Vector3(Random.Range(min.x, max.x), Random.Range(min.y, max.y), 0);
            return loc;
        }
    }

    public static Bounds BOUNDS => boxColl.bounds;
}


