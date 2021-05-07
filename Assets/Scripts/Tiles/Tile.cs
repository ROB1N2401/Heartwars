﻿using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] protected TileData tileData;
    [SerializeField] protected Vector3 playerPositionOffset;
    [SerializeField] protected Vector3 tileAbovePositionOffset;

    public TileData TileData => tileData;
    public Vector3 PlayerPositionOffset => playerPositionOffset;
    public Vector3 TileAbovePositionOffset => tileAbovePositionOffset;

    protected bool IsPlayerOnTile => _player != null;
    protected bool CanBeDestroyed => !IsPlayerOnTile && tileData.IsDestroyable && neighbourTiles.aboveTile == null;
    protected Player _player;
    protected (Tile aboveTile, Tile underTile) neighbourTiles;


    protected void Start()
    {
        var rayToTheUp = new Ray(transform.position, -transform.forward);
        var rayToTheBottom = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        
        if (Physics.Raycast(rayToTheUp, out hit))
        {
            var aboveTile = hit.transform.GetComponent<Tile>();
            if(aboveTile != null)
                neighbourTiles.aboveTile = aboveTile;
        }
    
        if (Physics.Raycast(rayToTheBottom, out hit))
        {
            var underTile = hit.transform.GetComponent<Tile>();
            if(underTile != null)
                neighbourTiles.underTile = underTile;
        }
    }

    protected void OnDrawGizmos()
    {
        var rayToTheUp = new Ray(transform.position, -transform.forward);
        var rayToTheBottom = new Ray(transform.position, transform.forward);
        Gizmos.color = Color.magenta;
        
        Gizmos.DrawRay(rayToTheUp);
        Gizmos.DrawRay(rayToTheBottom);
    }

    /// <summary>Return the very top tile above this one. In case there is no tiles above, this tile will be returned</summary>
    public Tile HighestTileFromAbove
    {
        get
        {
            if (neighbourTiles.aboveTile == null)
                return this;
            
            var highest = this;
            while (highest.neighbourTiles.aboveTile != null)
                highest = highest.neighbourTiles.aboveTile; 

            return highest;
        }
    }
    
    /// <summary>Return the lowest tile under this one. In case there is no tiles underneath, this tile will be returned</summary>
    public Tile LowestTileFromUnderneath
    {
        get
        {
            if (neighbourTiles.underTile == null)
                return this;
            
            var lowest = this;
            while (lowest.neighbourTiles.underTile != null)
                lowest = lowest.neighbourTiles.underTile; 

            return lowest;
        }
    }

    /// <summary>Hides tile (if possible)</summary>
    public virtual void DestroyTile()
    {
        if(neighbourTiles.aboveTile != null)
            return;
        
        var underTile = neighbourTiles.underTile;
        
        if(underTile != null)
            underTile.neighbourTiles.aboveTile = null;

        neighbourTiles.aboveTile = null;
        neighbourTiles.underTile = null;
        
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
    public bool IsPlayerAbleToDestroy(Player player) =>
        CanBeDestroyed &&
        player.PointsLeftForTheTurn >= tileData.PointsToDestroy;

    /// <summary>Places tile above this tile</summary>
    /// <param name="tileToAdd">Tile that will be placed above this tile</param>
    public void PlaceTileAbove(Tile tileToAdd)
    {
        if(tileToAdd == null || neighbourTiles.aboveTile != null)
            return;
        
        neighbourTiles.aboveTile = tileToAdd;
        tileToAdd.neighbourTiles.underTile = this;

        tileToAdd.transform.position = transform.position + tileAbovePositionOffset;
        tileToAdd.gameObject.SetActive(true);
    }
    
    public virtual void PlacePlayer(Player player)
    {
        if(player == null)
            return;

        if(player.attachedTile != null)
            player.attachedTile.RemovePlayer();
        
        if (TileData.Side != ESide.Neutral && tileData.Side != player.Side || tileData.TileType == ETileType.Void)
        {
            RemovePlayer();
            player.Die();
            return;
        }
        
        _player = player;
        player.attachedTile = this;
        player.transform.position = transform.position + playerPositionOffset;
    }

    public void RemovePlayer()
    {
        if(_player == null)
            return;
        
        _player.attachedTile = null;
        _player = null;
    }
}