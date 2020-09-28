
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera cam;
    private void Awake()
    {
        cam = GetComponent<Camera>();
        cam.orthographicSize = 2.9f / cam.aspect;
    }

}
