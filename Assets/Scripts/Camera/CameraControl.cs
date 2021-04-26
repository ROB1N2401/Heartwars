using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [Header("Speed control")]
    [SerializeField] [Min(0)] private float movementSpeed = 5f;
    [SerializeField] [Min(0)] private float scrollSpeed = 500f;
    [SerializeField] [Min(0)] private float rotationSpeed = 30f;

    [Header("Camera bounds")]
    public Vector3 lowestPositionForCamera;
    public Vector3 highestPositionForCamera;
    
    private void Update()
    {
        MoveCamera();
        RotateCamera();
    }
    
    private void MoveCamera()
    {
        var deltaZoom = Vector3.up * (-Input.GetAxis("Mouse ScrollWheel") * scrollSpeed * Time.deltaTime);
        var deltaForward = transform.forward * Input.GetAxis("Vertical");
        var deltaLeft = transform.right * Input.GetAxis("Horizontal");
        var deltaMovement = (deltaForward + deltaLeft) * (Time.deltaTime * movementSpeed);
        deltaMovement.y = 0;

        transform.position += deltaMovement + deltaZoom;
        
        var boundControlPosition = new Vector3(
            Mathf.Clamp(transform.position.x, lowestPositionForCamera.x, highestPositionForCamera.x),
            Mathf.Clamp(transform.position.y, lowestPositionForCamera.y, highestPositionForCamera.y),
            Mathf.Clamp(transform.position.z, lowestPositionForCamera.z, highestPositionForCamera.z));
        
        transform.position = boundControlPosition;
    }

    private void RotateCamera()
    {
        float rotationDirection = 0;
        float rotationAngle = 0;
        if (Input.GetKey(KeyCode.Q))
            rotationDirection -= 1;
        if (Input.GetKey(KeyCode.E))
            rotationDirection += 1;
        
        rotationAngle = (rotationSpeed * rotationDirection * Time.deltaTime);
        
        transform.Rotate(0, rotationAngle, 0, Space.World);
    }
}
