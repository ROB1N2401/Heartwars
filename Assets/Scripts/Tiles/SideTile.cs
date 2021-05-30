using System;
using System.Linq;
using UnityEngine;

public class SideTile : Tile
{
    private Renderer[] _renderers;
    private new void Awake()
    {
        base.Awake();
        _renderers = GetComponentsInChildren<Renderer>().Where(val => val.tag.Equals("Colorable")).ToArray();
        
        if(tileSide != ESide.Neutral)
            AssignSide(tileSide);
    }

    public override bool IsPlayerAbleToDestroy(Player player)
    {
        bool isPlayerAbleToDestroyDueToItsSide = tileData.IsDestroyable;
        bool isPlayerAbleToDestroyDueToTileType = !IsPlayerOnTile || tileData.TileType == ETileType.Spawn;

        if (destructionIgnoreSides.Length > 0)
        {
            if (tileData.IsDestroyable)
                isPlayerAbleToDestroyDueToItsSide = !destructionIgnoreSides.Contains(player.Side);
            if (!tileData.IsDestroyable)
                isPlayerAbleToDestroyDueToItsSide = destructionIgnoreSides.Contains(player.Side);
        }

        //todo reduce hardcode
        if (tileData.TileType != ETileType.Spawn && tileSide == player.Side)
            isPlayerAbleToDestroyDueToItsSide = true;
        
        return isPlayerAbleToDestroyDueToTileType &&
               isPlayerAbleToDestroyDueToItsSide &&
               _neighbourTiles.aboveTile == null &&
               player.PointsLeftForTheTurn >= tileData.PointsToDestroy;
    }

    public override void DestroyTile(Player player)
    {
        if(_neighbourTiles.aboveTile != null)
            return;

        var underTile = _neighbourTiles.underTile;
        
        if(underTile != null) 
            underTile._neighbourTiles.aboveTile = null;

        _neighbourTiles.aboveTile = null;
        _neighbourTiles.underTile = null;
        
        gameObject.SetActive(false);

        if (player != null && tileData.TileType != ETileType.Spawn) 
            AssignSide(player.Side);

        AudioManager.InvokeDestructionSound(TileData.TileType);
        underTile.PlacePlayer(_attachedPlayer);
    }

    public void AssignSide(ESide side)
    {
        if(side == ESide.Neutral)
            throw new ArgumentException("Side tile can not be neutral");

        tileSide = side;
        destructionIgnoreSides = new []{side};
        movementIgnoreSides = ((ESide[]) Enum.GetValues(typeof(ESide)))
            .Where(val => val != tileSide && val != ESide.Neutral)
            .ToArray();

        switch (side)
        {
            case ESide.Blue:
                foreach (var renderer in _renderers) 
                    renderer.material.color = new Color(0.1f, 0.1f, 0.75f);
                break;
            case ESide.Green:
                foreach (var renderer in _renderers) 
                    renderer.material.color = new Color(0.1f, 0.75f, 0.1f);
                break;
            case ESide.Orange:
                foreach (var renderer in _renderers) 
                    renderer.material.color = new Color(255, 140, 0);
                break;
            case ESide.Purple:
                foreach (var renderer in _renderers) 
                    renderer.material.color = new Color(144, 0, 255);
                break;
            case ESide.Red:
                foreach (var renderer in _renderers) 
                    renderer.material.color = new Color(0.75f, 0.1f, 0.1f);
                break;
            case ESide.Yellow:
                foreach (var renderer in _renderers) 
                    renderer.material.color = Color.yellow;
                break;
        }
    }
}