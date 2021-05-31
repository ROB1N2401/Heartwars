using UnityEngine;

public class SkyboxControl : MonoBehaviour
{
    [SerializeField] private float RotateSpeed = 1.2f;

    private Material _skyboxCopy;
    
    private void Start() => _skyboxCopy = Instantiate(RenderSettings.skybox);

    private void Update()
    {
        _skyboxCopy.SetFloat("_Rotation", Time.time * RotateSpeed);
        RenderSettings.skybox = _skyboxCopy;
    }
}