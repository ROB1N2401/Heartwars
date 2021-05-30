using UnityEngine;
using UnityEngine.UI;


public class HueChange : MonoBehaviour
{
    [SerializeField] private bool randomize;
    [SerializeField] private float speed;
    private float hue;
    private float sat;
    private float bri;
    private Image _image;
    private Renderer _renderer;

    void Start()
    {
        _image = GetComponent<Image>();
        _renderer = GetComponent<Renderer>();

        if (randomize)
        {
            hue = Random.Range(0f, 1f);
        }
        sat = 1;
        bri = 1;

        if(_image != null)
            _image.color = Color.HSVToRGB(hue, sat, bri);
        if(_renderer != null)
            _renderer.material.color = Color.HSVToRGB(hue, sat, bri);
    }

    void Update()
    {
        if(_image != null)
            Color.RGBToHSV(_image.color, out hue, out sat, out bri);
        
        if(_renderer != null)
            Color.RGBToHSV(_renderer.material.color, out hue, out sat, out bri);
        
        hue += speed / 10000;
        if (hue >=1)
        {
            hue = 0;
        }

        if (_image != null) 
            _image.color = Color.HSVToRGB(hue, sat, bri);

        if(_renderer != null)
            _renderer.material.color = Color.HSVToRGB(hue, sat, bri);
    }
}