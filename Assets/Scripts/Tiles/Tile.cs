using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Tile : MonoBehaviour
{
    [SerializeField] protected TileData tileData;
    public ESide tileSide;
    [SerializeField] protected ESide[] destructionIgnoreSides;
    [SerializeField] protected ESide[] movementIgnoreSides;
    [Header("Offset options")]
    [SerializeField] protected Vector3 playerPositionOffset;
    [SerializeField] protected Vector3 tileAbovePositionOffset;
    
    public TileData TileData => tileData;
    public Player AttachedPlayer => _attachedPlayer;
    public Vector3 PositionForPlayer => playerPositionOffset + transform.position;
    public Vector3 TileAbovePositionOffset => tileAbovePositionOffset;
    public bool IsPlayerOnTile => _attachedPlayer != null;

    protected Player _attachedPlayer;
    protected internal (Tile aboveTile, Tile underTile) _neighbourTiles;
    protected PlayerTransition Animation; 
    
    private static readonly int[] _rotationAnglesPool = {60, 120, 180, 240, 300};

    protected virtual void Awake()
    {
        if(!isActiveAndEnabled)
            return;
            
        var rayToTheUp = new Ray(transform.position, Vector3.up);
        var rayToTheBottom = new Ray(transform.position, Vector3.down);
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

        if (_neighbourTiles.underTile == this)
            _neighbourTiles.underTile = null;
        if (_neighbourTiles.aboveTile == this)
            _neighbourTiles.aboveTile = null;
        
        if (_neighbourTiles.underTile == this || _neighbourTiles.aboveTile == this)
            throw new ApplicationException($"Reference of a neighbour is set to itself inside {gameObject.name}");
    }

    /// <summary>Checks if there is any neighbour tiles above and under current tile with raycast</summary>
    /// <exception cref="ApplicationException">Throws an exception if neighbour tiles referencing current tile</exception>
    protected virtual void Start()
    {
        //Rotates children from current tile in a random direction
        var randomRotationAngle = _rotationAnglesPool[Random.Range(0, _rotationAnglesPool.Length)];
        foreach (Transform transformInChild in transform)
            transformInChild.Rotate(0, randomRotationAngle, 0, Space.World);


        Animation = GetComponent<PlayerTransition>();
    }
    
    //todo debug
    protected void OnDrawGizmos()
    {
        var rayToTheUp = new Ray(transform.position, Vector3.up);
        var rayToTheBottom = new Ray(transform.position, Vector3.down);
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
    public bool IsPlayerAbleToMove(Player player)
    {
        bool isPlayerAbleToMoveDueToSide = tileData.IsWalkable;

        if (destructionIgnoreSides.Length > 0)
        {
            if (tileData.IsWalkable)
                isPlayerAbleToMoveDueToSide = !movementIgnoreSides.Contains(player.Side);
            else
                isPlayerAbleToMoveDueToSide = movementIgnoreSides.Contains(player.Side);
        }
        
        return isPlayerAbleToMoveDueToSide &&
               !IsPlayerOnTile &&
               player.PointsLeftForTheTurn >= player.PlayerData.PointsForMovementTaken;
    }

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
    public virtual bool IsPlayerAbleToDestroy(Player player)
    {
        bool isPlayerAbleToDestroyDueToItsSide = tileData.IsDestroyable;

        if (destructionIgnoreSides.Length > 0)
        {
            if (tileData.IsDestroyable)
                isPlayerAbleToDestroyDueToItsSide = !destructionIgnoreSides.Contains(player.Side);
            else
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
        
        var destinationPosition = transform.position + tileAbovePositionOffset;
        tileToAdd.transform.position = destinationPosition + Vector3.up * 20f;

        TileTransition.Instance.Fly(tileToAdd, destinationPosition, .2f,
            () =>
            {
                tileToAdd.gameObject.SetActive(true);
                AudioManager.InvokePlacementSound(tileToAdd.TileData.TileType);
            });

        // tileToAdd.transform.position = destinationPosition;
        // tileToAdd.gameObject.SetActive(true);
        // AudioManager.InvokePlacementSound(tileToAdd.TileData.TileType);
    }

    /// <summary>Hides tile (if possible)</summary>
    public virtual void DestroyTile(Player player)
    {
        if (_neighbourTiles.aboveTile != null)
            return;

        var underTile = _neighbourTiles.underTile;

        if (underTile != null)
            underTile._neighbourTiles.aboveTile = null;

        _neighbourTiles.aboveTile = null;
        _neighbourTiles.underTile = null;

        var destinationPosition = transform.position + Vector3.up * 20f;

        TileTransition.Instance.Fly(this, destinationPosition, .5f,
            () => AudioManager.InvokeDestructionSound(TileData.TileType), 
            () => gameObject.SetActive(false));
        
        // gameObject.SetActive(false);
        // AudioManager.InvokeDestructionSound(TileData.TileType);
    }

    /// <summary>Places given player above current tile</summary>
    /// <param name="player">Player that will be placed above</param>
    public virtual void PlacePlayer(Player player, ETransitionType transitionType = ETransitionType.Walk)
    {
        if(player == null)
            return;

        if(player.attachedTile != null)
            player.attachedTile.RemovePlayer();
        
        _attachedPlayer = player;
        player.attachedTile = this;
        var animator = player.GetComponent<PlayerTransition>();

        if (animator != null)
        {
            switch (transitionType)
            {
                case ETransitionType.Walk:
                    animator.DirectTransition(PositionForPlayer);
                    break;
                case ETransitionType.Spawn:
                    animator.Respawn(PositionForPlayer, 10f);
                    break;
            }
        }

        if (tileSide != ESide.Neutral && tileSide != player.Side || tileData.TileType == ETileType.Void)
            player.Die();
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

        if (Physics.Raycast(rayToTheOppositeTile, out hit, 1f)) 
            return hit.transform.GetComponent<Tile>();

        return null;
    }
}