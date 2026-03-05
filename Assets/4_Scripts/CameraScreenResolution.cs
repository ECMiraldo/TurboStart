using UnityEngine;

[ExecuteInEditMode]
public class CameraScreenResolution : MonoBehaviour
{
    void Start()
    {
        SetAspect();
    }

    void Update()
    {
#if UNITY_EDITOR
        SetAspect(); // update in editor
#endif
    }

    void SetAspect()
    {
        float targetAspect = 1f; // 1:1
        float windowAspect = (float)Screen.width / Screen.height;
        float scaleHeight = windowAspect / targetAspect;

        Camera cam = GetComponent<Camera>();

        if (scaleHeight < 1.0f)
        {
            // Letterbox (top/bottom bars)
            Rect rect = cam.rect;
            rect.width = 1f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1f - scaleHeight) / 2f;
            cam.rect = rect;
        }
        else
        {
            // Pillarbox (left/right bars)
            float scaleWidth = 1f / scaleHeight;

            Rect rect = cam.rect;
            rect.width = scaleWidth;
            rect.height = 1f;
            rect.x = (1f - scaleWidth) / 2f;
            rect.y = 0;
            cam.rect = rect;
        }
    }
}