using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HueChange : MonoBehaviour
{
    public bool randomize;
    public bool invert;
    public float speed;
    public float hue;
    public float sat;
    public float bri;
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
        rend.material.color = Color.HSVToRGB(hue, sat, bri);
    }

    void Update()
    {
        Color.RGBToHSV(rend.material.color, out hue, out sat, out bri);
        if (invert)
        {
            hue -= speed / 10000;
            if (hue <=0)
            {
                hue = 0.99f;
            }
        }
        else
        {
            hue += speed / 10000;
            if (hue >=1)
            {
                hue = 0;
            }
        }
        rend.material.color = Color.HSVToRGB(hue, sat, bri);
    }
}