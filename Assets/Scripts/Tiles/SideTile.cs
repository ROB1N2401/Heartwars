using System;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class SideTile : Tile
{
    private Renderer _renderer;
    protected virtual void Awake()
    {
        _renderer = GetComponent<Renderer>();
        
        if(tileSide != ESide.Neutral)
            AssignSide(tileSide);
    }

    public void AssignSide(ESide side)
    {
        if(side == ESide.Neutral)
            throw new ArgumentException("Side tile can not be neutral");

        tileSide = side;

        switch (side)
        {
            case ESide.Blue:
                _renderer.material.color = Color.blue;
                break;
            case ESide.Green:
                _renderer.material.color = Color.green;
                break;
            case ESide.Orange:
                _renderer.material.color = new Color(255, 140, 0);
                break;
            case ESide.Purple:
                _renderer.material.color = new Color(144, 0, 255);
                break;
            case ESide.Red:
                _renderer.material.color = Color.red;
                break;
            case ESide.Yellow:
                _renderer.material.color = Color.yellow;
                break;
        }
    }
}