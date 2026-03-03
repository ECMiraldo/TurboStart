using UnityEngine;

[ExecuteInEditMode]
public class CameraScreenResolution : MonoBehaviour
{
    public bool maintainWidth = true;
    [Range(-1, 1)]
    public int adaptPosition;
    float defaultWeidth, defaultHeight;

    Vector3 CameraPos;
      void Update()
    {
        if (maintainWidth)
            Camera.main.orthographicSize = defaultWeidth / Camera.main.aspect;
        else Camera.main.orthographicSize = defaultHeight / Camera.main.aspect;
    }
}
