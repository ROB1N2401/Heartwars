using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraControl : MonoBehaviour
{
    [Header("Mouse control")] 
    [SerializeField] [Min(.1f)] private float mouseBorderTriggerOffset = 40f;

    [Header("Speed")]
    [SerializeField] [Min(0)] private float movementSpeed = 5f;
    [SerializeField] [Min(0)] private float scrollSpeed = 500f;
    [SerializeField] [Min(0)] private float rotationSpeed = 30f;

    [Header("Camera bounds")]
    public Vector3 lowestPositionForCamera;
    public Vector3 highestPositionForCamera;

    private Camera _camera;

    private void Start() => _camera = GetComponent<Camera>();

    private void Update()
    {
        MoveCamera();
        RotateCamera();
    }
    
    private void MoveCamera()
    {
        float directionForward = 0, directionLeft = 0;
        directionForward += Input.GetAxis("Vertical");
        directionLeft += Input.GetAxis("Horizontal");
        if (Input.mousePosition.x >= _camera.pixelWidth - mouseBorderTriggerOffset)
            directionLeft += 1;
        if (Input.mousePosition.x <= 0 + mouseBorderTriggerOffset)
            directionLeft -= 1;
        if (Input.mousePosition.y >= _camera.pixelHeight - mouseBorderTriggerOffset)
            directionForward += 1;
        if (Input.mousePosition.y <= 0 + mouseBorderTriggerOffset)
            directionForward -= 1;

        var deltaZoom = Vector3.up * (-Input.GetAxis("Mouse ScrollWheel") * scrollSpeed * Time.deltaTime);
        var deltaForward = transform.forward * directionForward;
        var deltaLeft = transform.right * directionLeft;
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