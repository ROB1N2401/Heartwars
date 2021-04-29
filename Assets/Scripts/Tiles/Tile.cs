using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] protected TileData tileData;
    [SerializeField] protected Vector3 playerPositionOffset;
    [SerializeField] protected Vector3 tileAbovePositionOffset;

    public TileData TileData => tileData;
    public Vector3 PlayerPositionOffset => playerPositionOffset;

    protected bool IsPlayerOnTile => _player != null;
    protected bool CanBeDestroyed => !IsPlayerOnTile && tileData.IsDestroyable;
    protected Player _player;
    protected (Tile aboveTile, Tile underTile) neighbourTiles;

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

    public virtual void DestroyTile()
    {
        var tileAbove = neighbourTiles.aboveTile;
        var tileUnder = neighbourTiles.underTile;

        tileAbove.neighbourTiles.underTile = tileUnder;
        tileAbove.neighbourTiles.aboveTile = tileAbove;

        neighbourTiles.underTile = null;
        neighbourTiles.aboveTile = null;

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
        tileData.IsAllowedToPlaceTilesAbove &&
        !IsPlayerOnTile &&
        player.PointsLeftForTheTurn >= tileToPlace.TileData.PointsToPlace;
    /// <summary>Checks if given player able to destroy this tile</summary>
    /// <param name="player">Player for which condition will be checked</param>
    /// <returns>Returns true if player can destroy this tile. Otherwise returns false</returns>
    public bool IsPlayerAbleToDestroy(Player player) => 
        !IsPlayerOnTile && CanBeDestroyed && player.PointsLeftForTheTurn >= tileData.PointsToDestroy;

    /// <summary>Places tile above this tile</summary>
    /// <param name="tileToAdd">Tile that will be placed above this tile</param>
    public void PlaceTileAbove(Tile tileToAdd)
    {
        neighbourTiles.aboveTile = tileToAdd;
        tileToAdd.neighbourTiles.underTile = this;

        tileToAdd.transform.position = transform.position + tileAbovePositionOffset;
    }
    
    public virtual void PlacePlayer(Player player)
    {
        if (TileData.Side != ESide.Neutral && tileData.Side != player.Side)
        {
            RemovePlayer();
            player.Die();
            return;
        }
        
        _player = player;
        player.attachedTile = this;
    }

    public void RemovePlayer()
    {
        _player.attachedTile = null;
        _player = null;
    }
}