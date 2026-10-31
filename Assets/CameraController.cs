using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    

    private Camera cam;
    private float pitch = 0;
    private float yaw = 0;
    public float pitchMax = 60;
    public float pitchMin = -60;
    public float turnspeed = 2;
    private void Awake()
    {
        cam = GetComponentInChildren<Camera>();
        
    }
    public void MoveCam(InputAction.CallbackContext context)
    {

        Vector2 mouseMovement = context.ReadValue<Vector2>();
        
        pitch += mouseMovement.y * turnspeed;
        if (pitch < pitchMin)
        {
            pitch = pitchMin;
        }
        if (pitch > pitchMax)
        {
            pitch = pitchMax;
        }

        yaw += mouseMovement.x * turnspeed;
        
        cam.transform.rotation = Quaternion.Euler(-pitch, yaw, 0);
        transform.rotation = Quaternion.Euler(0, yaw, 0);
    }
}
