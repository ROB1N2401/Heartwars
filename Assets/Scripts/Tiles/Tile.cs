using JetBrains.Annotations;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] [NotNull] private TileData tileData;
    [SerializeField] protected Vector3 playerPositionOffset;

    public bool CanBeDestroyed => !IsPlayerOnTile && tileData.IsDestroyable;
    public bool IsFreeToPlacePlayer => !IsPlayerOnTile && tileData.IsWalkable;
    public bool IsPlayerOnTile => _player != null;
    public TileData TileData => tileData;
    public Vector3 PlayerPositionOffset => playerPositionOffset;

    protected Player _player;

    /// <summary>Method that binds player to the tile and moves player on it(tile)</summary>
    /// <param name="player">Player that will be attached to this tile</param>
    public virtual void PlacePlayer(Player player) => _player = player;

    /// <summary>Removes and unbinds player from the tile</summary>
    public virtual void RemovePlayer() => _player = null;

    /// <summary>Destroys(if possible) the tile</summary>
    public virtual void DestroyTile() => gameObject.SetActive(false);

    public bool IsPlayerAbleToDestroy(Player player) =>
        player != null && CanBeDestroyed && player.PointsLeftForTheTurn < tileData.PointsToDestroy;
}