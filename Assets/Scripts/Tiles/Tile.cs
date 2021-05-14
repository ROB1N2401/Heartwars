using System;
using System.Linq;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] protected TileData tileData;
    public ESide tileSide;
    [SerializeField] protected ESide[] destructionIgnoreSides;
    [Header("Offset options")]
    [SerializeField] protected Vector3 playerPositionOffset;
    [SerializeField] protected Vector3 tileAbovePositionOffset;
    
    public TileData TileData => tileData;
    public Player AttachedPlayer => _attachedPlayer;
    public Vector3 PlayerPositionOffset => playerPositionOffset;
    public Vector3 TileAbovePositionOffset => tileAbovePositionOffset;

    protected bool IsPlayerOnTile => _attachedPlayer != null;
    protected Player _attachedPlayer;
    protected (Tile aboveTile, Tile underTile) _neighbourTiles;
    
    /// <summary>Checks if there is any neighbour tiles above and under current tile with raycast</summary>
    /// <exception cref="ApplicationException">Throws an exception if neighbour tiles referencing current tile</exception>
    protected virtual void Start()
    {
        var rayToTheUp = new Ray(transform.position, -transform.forward);
        var rayToTheBottom = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        
        if (Physics.Raycast(rayToTheUp, out hit, 5f))
        {
            var aboveTile = hit.transform.GetComponent<Tile>();
            _neighbourTiles.aboveTile = aboveTile;
        }
    
        if (Physics.Raycast(rayToTheBottom, out hit, 5f))
        {
            var underTile = hit.transform.GetComponent<Tile>();
            _neighbourTiles.underTile = underTile;
        }

        if (_neighbourTiles.underTile == this || _neighbourTiles.aboveTile == this)
            throw new ApplicationException($"Reference of a neighbour is set to itself inside {gameObject.name}");
    }
    
    //todo debug
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
            if (_neighbourTiles.aboveTile == null)
                return this;
            
            if(_neighbourTiles.aboveTile == this)
                throw new ApplicationException("Reference of a neighbour is set to itself");
            
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
            
            if(_neighbourTiles.underTile == this)
                throw new ApplicationException("Reference of a neighbour is set to itself");
            
            var lowest = this;
            while (lowest._neighbourTiles.underTile != null)
                lowest = lowest._neighbourTiles.underTile; 

            return lowest;
        }
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
    
    /// <summary>Places given player above current tile</summary>
    /// <param name="player">Player that will be placed above</param>
    public virtual void PlacePlayer(Player player)
    {
        if(player == null)
            return;

        if(player.attachedTile != null)
            player.attachedTile.RemovePlayer();
        
        _attachedPlayer = player;
        player.attachedTile = this;
        player.transform.position = transform.position + playerPositionOffset;
        
        if (tileSide != ESide.Neutral && tileSide != player.Side || tileData.TileType == ETileType.Void)
        {
            player.Die();
            RemovePlayer();
        }
    }

    /// <summary>Removes all attachments from this tile relative to player</summary>
    public void RemovePlayer()
    {
        if(_attachedPlayer == null)
            return;
        
        _attachedPlayer.attachedTile = null;
        _attachedPlayer = null;
    }
    
    /// <summary>Gets tile from the opposite direction of a given tile</summary>
    /// <param name="neighbourOppositeTile">Tile for which opposite direction will be taken</param>
    /// <returns>Returns closest tile from opposite direction</returns>
    public Tile GetTileFromOppositeDirection(Tile neighbourOppositeTile)
    {
        if (neighbourOppositeTile == null)
            return null;
        neighbourOppositeTile = neighbourOppositeTile.LowestTileFromUnderneath;

        var positionOfTheBaseTile = LowestTileFromUnderneath.transform.position;
        var directionOfTheOppositeTile = neighbourOppositeTile.transform.position - positionOfTheBaseTile;
        directionOfTheOppositeTile = -directionOfTheOppositeTile.normalized;

        var rayToTheOppositeTile = new Ray(positionOfTheBaseTile, directionOfTheOppositeTile);
        RaycastHit hit;

        if (Physics.Raycast(rayToTheOppositeTile, out hit)) 
            return hit.transform.GetComponent<Tile>();

        return null;
    }
}