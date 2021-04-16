using UnityEngine;

public class OffScreenWrapper: MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (!enabled)
        {
            return;
        }

        if (other.TryGetComponent<ScreenBounds>(out ScreenBounds bounds))
        {
            ScreenWrap(bounds);
        }
        else
        {
            var extraBounds = other.GetComponentInParent<ScreenBounds>();

            if (extraBounds)
            {
                Vector3 pos = transform.position.ComponentDivide(other.transform.localScale);
                pos.z = 0;
                transform.position = pos;

                ScreenWrap(extraBounds);
            }
        }
    }

    private void ScreenWrap(ScreenBounds bounds)
    {
        // Wrap whichever direction is necessary
        Vector3 relativeLoc = bounds.transform.InverseTransformPoint(transform.position);
        // Because this is now a child of OnScreenBounds, 0.5f is the edge of the screen.
        if (Mathf.Abs(relativeLoc.x) > 0.5f)
        {
            relativeLoc.x *= -1;
        }
        if (Mathf.Abs(relativeLoc.y) > 0.5f)
        {
            relativeLoc.y *= -1;
        }
        transform.position = bounds.transform.TransformPoint(relativeLoc);
    }

}