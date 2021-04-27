using JetBrains.Annotations;
using UnityEngine;

public abstract class Tile : MonoBehaviour
{
    [SerializeField] private Vector3 playerPositionOffset;
    [SerializeField] [NotNull] private TileData tileData;
    
    public bool IsWalkable => tileData.IsWalkable;
    public bool IsDestroyable => tileData.IsDestroyable;
    public ESide TileESide => tileData.tileESide;
    public Player Player => _player;

    private Player _player;

    /// <summary>Method that binds player to the tile and moves player on it(tile)</summary>
    /// <param name="player">Player that will be attached to this tile</param>
    /// <returns>Returns true if player was successfully attached on the tile. Otherwise returns false</returns>
    public virtual bool SetPlayer(Player player)
    {
        if (_player != null || !tileData.IsWalkable)
            return false;

        _player = player;
        _player.gameObject.transform.position = transform.position + playerPositionOffset;
        return true;
    }

    /// <summary>Removes and unbinds player from the tile</summary>
    /// <returns>Return Player object if player was successfully removed. Otherwise returns null</returns>
    public virtual Player RemovePlayer()
    {
        if (_player == null)
            return null;
        
        var tmp = _player;
        _player = null;
        return tmp;
    }

    /// <summary>Destroys title</summary>
    /// <returns>Return Item that is dropped after tile destruction. Returns null if there is no items to be dropped</returns>
    public abstract Item DestroyAndGetItem();
    
    /// <summary>Places tile</summary>
    /// <returns>Returns true if tile was successfully placed</returns>
    public abstract bool PlaceTile();
}