using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HueChange : MonoBehaviour
{
    [SerializeField] private bool randomize;
    [SerializeField] private bool invert;
    [SerializeField] private float speed;
    private float hue;
    private float sat;
    private float bri;
    private Image rend;

    void Start()
    {
        rend = GetComponent<Image>();
        if (randomize)
        {
            hue = Random.Range(0f, 1f);
        }
        sat = 1;
        bri = 1;
        speed = 2;
        rend.color = Color.HSVToRGB(hue, sat, bri);
    }

    void Update()
    {
        Color.RGBToHSV(rend.color, out hue, out sat, out bri);

        hue += speed / 10000;
        if (hue >=1)
        {
            hue = 0;
        }
        
        rend.color = Color.HSVToRGB(hue, sat, bri);
    }
}