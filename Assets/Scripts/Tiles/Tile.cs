using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private TileData tileData;
    [SerializeField] protected Vector3 playerPositionOffset;

    public TileData TileData => tileData;
    public Vector3 PlayerPositionOffset => playerPositionOffset;

    protected bool IsPlayerOnTile => _player != null;
    protected bool CanBeDestroyed => !IsPlayerOnTile && tileData.IsDestroyable;
    protected Player _player;
    protected (Tile aboveTile, Tile underTile) neighbourTiles;

    #region BuildManagement
    public void PlaceTileAboveOtherTile(Tile baseTile) => baseTile.AddTileAbove(this);

    /// <summary>Destroys(if possible) the tile</summary>
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

    protected void AddTileAbove(Tile tileToAdd)
    {
        neighbourTiles.aboveTile = tileToAdd;
        tileToAdd.neighbourTiles.underTile = this;
    }
    
    public Tile GetHighestTileFromAbove()
    {
        if (neighbourTiles.aboveTile == null)
            return this;

        var highest = this;
        while (highest.neighbourTiles.aboveTile != null) 
            highest = highest.neighbourTiles.aboveTile;

        return highest;
    }

    public bool IsPlayerAbleToPlaceTileAbove(Tile tileToPlace, Player player) =>
        tileToPlace.TileData.IsPlaceable && tileData.IsAllowedToPlaceTilesAbove &&
        player.PointsLeftForTheTurn >= tileToPlace.TileData.PointsToPlace;
    
    /// <summary>Checks if player is able to destroy given tile</summary>
    /// <param name="player">Player for which condition will be checked</param>
    /// <returns>Returns true if player can destroy the tile</returns>
    public bool IsPlayerAbleToDestroy(Player player) =>
        player != null && CanBeDestroyed && player.PointsLeftForTheTurn < tileData.PointsToDestroy;
    #endregion
    
    #region PlayerManagement
    /// <summary>Method that binds player to the tile and moves player on it(tile)</summary>
    /// <param name="player">Player that will be attached to this tile</param>
    public virtual void PlacePlayer(Player player) => _player = player;

    /// <summary>Removes and unbinds player from the tile</summary>
    public virtual void RemovePlayer() => _player = null;
    
    /// <summary>Checks if player is able to move on this tile</summary>
    /// <param name="player">Player for which condition will be checked</param>
    /// <returns>Returns true if player can be moved to the tile</returns>
    public bool IsPlayerAbleToMove(Player player) =>
        tileData.IsWalkable &&
        IsPlayerOnTile &&
        player.PointsLeftForTheTurn >= player.PlayerData.PointsForMovementTaken;
    #endregion
}