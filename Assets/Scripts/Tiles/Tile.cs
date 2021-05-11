﻿using System.Linq;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] protected TileData tileData;
    [SerializeField] protected ESide tileSide;
    [SerializeField] protected ESide[] destructionIgnoreSides;
    [Header("Offset options")]
    [SerializeField] protected Vector3 playerPositionOffset;
    [SerializeField] protected Vector3 tileAbovePositionOffset;
    
    public TileData TileData => tileData;
    public ESide TileSide => tileSide;
    public Vector3 PlayerPositionOffset => playerPositionOffset;
    public Vector3 TileAbovePositionOffset => tileAbovePositionOffset;

    protected bool IsPlayerOnTile => _player != null;
    protected Player _player;
    protected (Tile aboveTile, Tile underTile) _neighbourTiles;


    protected virtual void Start()
    {
        var rayToTheUp = new Ray(transform.position, -transform.forward);
        var rayToTheBottom = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        
        if (Physics.Raycast(rayToTheUp, out hit))
        {
            var aboveTile = hit.transform.GetComponent<Tile>();
            if(aboveTile != null)
                _neighbourTiles.aboveTile = aboveTile;
        }
    
        if (Physics.Raycast(rayToTheBottom, out hit))
        {
            var underTile = hit.transform.GetComponent<Tile>();
            if(underTile != null)
                _neighbourTiles.underTile = underTile;
        }
    }
    
    //todo debug
    private Vector3 _direction;
    private Vector3 _start;
    protected void OnDrawGizmos()
    {
        var rayToTheUp = new Ray(transform.position, -transform.forward);
        var rayToTheBottom = new Ray(transform.position, transform.forward);
        Gizmos.color = Color.magenta;
        
        Gizmos.DrawRay(rayToTheUp);
        Gizmos.DrawRay(rayToTheBottom);
        
        // Gizmos.color = Color.black;
        // Gizmos.DrawRay(_start, _direction);
    }

    /// <summary>Return the very top tile above this one. In case there is no tiles above, this tile will be returned</summary>
    public Tile HighestTileFromAbove
    {
        get
        {
            if (_neighbourTiles.aboveTile == null)
                return this;
            
            var highest = this;
            while (highest._neighbourTiles.aboveTile != null)
                highest = highest._neighbourTiles.aboveTile; 

            return highest;
        }
    }
    
    /// <summary>Return the lowest tile under this one. In case there is no tiles underneath, this tile will be returned</summary>
    public Tile LowestTileFromUnderneath
    {
        get
        {
            if (_neighbourTiles.underTile == null)
                return this;
            
            var lowest = this;
            while (lowest._neighbourTiles.underTile != null)
                lowest = lowest._neighbourTiles.underTile; 

            return lowest;
        }
    }

    /// <summary>Hides tile (if possible)</summary>
    public virtual void DestroyTile()
    {
        if(_neighbourTiles.aboveTile != null)
            return;
        
        var underTile = _neighbourTiles.underTile;
        
        if(underTile != null)
            underTile._neighbourTiles.aboveTile = null;

        _neighbourTiles.aboveTile = null;
        _neighbourTiles.underTile = null;
        
        gameObject.SetActive(false);
    }
    
    /// <summary>Checks if given player can step on this tile</summary>
    /// <param name="player">Player for which condition will be checked</param>
    /// <returns>Returns true if player can be moved to this tile. Otherwise returns false</returns>
    public bool IsPlayerAbleToMove(Player player) =>
        tileData.IsWalkable && !IsPlayerOnTile &&
        player.PointsLeftForTheTurn >= player.PlayerData.PointsForMovementTaken;
    /// <summary>Checks if given player able to place given tile above this one</summary>
    /// <param name="tileToPlace">Tile player tries to place</param>
    /// <param name="player">Player for which condition will be checked</param>
    /// <returns>Returns true if player can place tile. Otherwise returns false</returns>
    public bool IsPlayerAbleToPlaceTileAbove(Tile tileToPlace, Player player) =>
        tileToPlace.TileData.IsPlaceable &&
        tileData.IsAllowedToBuildTilesAbove &&
        !IsPlayerOnTile &&
        player.PointsLeftForTheTurn >= tileToPlace.TileData.PointsToPlace;

    /// <summary>Checks if given player able to destroy this tile</summary>
    /// <param name="player">Player for which condition will be checked</param>
    /// <returns>Returns true if player can destroy this tile. Otherwise returns false</returns>
    public bool IsPlayerAbleToDestroy(Player player)
    {
        bool isPlayerAbleToDestroyDueToItsSide = tileData.IsDestroyable;

        if (destructionIgnoreSides.Length > 0)
        {
            if (tileData.IsDestroyable)
                isPlayerAbleToDestroyDueToItsSide = !destructionIgnoreSides.Contains(player.Side);
            if (!tileData.IsDestroyable)
                isPlayerAbleToDestroyDueToItsSide = destructionIgnoreSides.Contains(player.Side);
        }

        return !IsPlayerOnTile &&
               isPlayerAbleToDestroyDueToItsSide &&
               _neighbourTiles.aboveTile == null &&
               player.PointsLeftForTheTurn >= tileData.PointsToDestroy;
    }

    /// <summary>Places tile above this tile</summary>
    /// <param name="tileToAdd">Tile that will be placed above this tile</param>
    public void PlaceTileAbove(Tile tileToAdd)
    {
        if(tileToAdd == null || _neighbourTiles.aboveTile != null)
            return;
        
        _neighbourTiles.aboveTile = tileToAdd;
        tileToAdd._neighbourTiles.underTile = this;

        tileToAdd.transform.position = transform.position + tileAbovePositionOffset;
        tileToAdd.gameObject.SetActive(true);
    }
    
    public virtual void PlacePlayer(Player player)
    {
        if(player == null)
            return;

        if(player.attachedTile != null)
            player.attachedTile.RemovePlayer();
        
        _player = player;
        player.attachedTile = this;
        player.transform.position = transform.position + playerPositionOffset;
        
        if (tileSide != ESide.Neutral && tileSide != player.Side || tileData.TileType == ETileType.Void)
        {
            player.Die();
            RemovePlayer();
        }
    }

    public void RemovePlayer()
    {
        if(_player == null)
            return;
        
        _player.attachedTile = null;
        _player = null;
    }
    
    public Tile GetTileFromOppositeDirection(Tile neighbourOppositeTile)
    {
        if (neighbourOppositeTile == null)
            return null;
        neighbourOppositeTile = neighbourOppositeTile.LowestTileFromUnderneath;

        var positionOfTheBaseTile = LowestTileFromUnderneath.transform.position;
        var directionOfTheOppositeTile = neighbourOppositeTile.transform.position - positionOfTheBaseTile;
        directionOfTheOppositeTile = -directionOfTheOppositeTile.normalized;

        //todo debug
        _direction = directionOfTheOppositeTile;
        _start = positionOfTheBaseTile;
        
        var rayToTheOppositeTile = new Ray(positionOfTheBaseTile, directionOfTheOppositeTile);
        RaycastHit hit;

        if (Physics.Raycast(rayToTheOppositeTile, out hit)) 
            return hit.transform.GetComponent<Tile>();

        return null;
    }
}