using System;
using UnityEngine;

[RequireComponent(typeof(PlayerInventory))]
public class Player : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private ESide side = ESide.Blue;
    [SerializeField] private Tile spawnPoint;

    public int PointsAtTheBeginningOfTheTurn => playerData.InitialNumberOfPoints + _playerInventory.TotalBonusPoints;
    public int PointsLeftForTheTurn => _pointsLeftForTheTurn;
    public bool IsTurnTime => _isTurnTime;
    public PlayerData PlayerData => playerData;

    [HideInInspector] public Tile attachedTile;
    public Tile SpawnPoint => spawnPoint;
    public ESide Side => side;

    private bool _isTurnTime = false;
    private int _pointsLeftForTheTurn = 0;
    private PlayerInventory _playerInventory;

    private void Start()
    {
        _playerInventory = GetComponent<PlayerInventory>();
        if(spawnPoint.TileData.TileType != ETileType.Spawn || spawnPoint.TileSide != Side || spawnPoint == null )
            throw new ArgumentException("SpawnPoint has to be Spawn type, have the same side as player and be non null");
        attachedTile = spawnPoint;
        attachedTile.PlacePlayer(this);
        _pointsLeftForTheTurn = PointsAtTheBeginningOfTheTurn;
    }

    /// <summary>Moves player to the given tile</summary>
    /// <param name="tile">Target tile where player will be moved</param>
    public void MoveTo(Tile tile)
    {
        var topTile = tile.HighestTileFromAbove;
        if(!topTile.IsPlayerAbleToMove(this))
            return;

        // attachedTile.RemovePlayer();
        topTile.PlacePlayer(this);

        SubtractActivePoints(playerData.PointsForMovementTaken);
    }

    /// <summary>Checks if there is item available in players inventory</summary>
    /// <param name="tileType">Tile type gor which condition will be checked</param>
    /// <returns>Returns tru if such item available in the inventory. Otherwise returns false</returns>
    public bool HasSuchItemInInventory(ETileType tileType)
    {
        if (_playerInventory.GetNumberOfGivenTilesInInventory(tileType) <= 0)
            return false;

        return true;
    }
    
    public void PlaceTile(ETileType tileType, Tile baseTile)
    {
        var tileToPlace = _playerInventory.TakeTileFromInventory(tileType);
        if(tileToPlace == null || baseTile == null)
            return;

        var veryTopTile = baseTile.HighestTileFromAbove;
        if (!veryTopTile.IsPlayerAbleToPlaceTileAbove(tileToPlace, this))
        {
            _playerInventory.AddTile(tileToPlace);
            return;
        }

        veryTopTile.PlaceTileAbove(tileToPlace);
        
        SubtractActivePoints(tileToPlace.TileData.PointsToPlace);
    }

    /// <summary>Destroys given tile tile</summary>
    /// <param name="tile">Tile that will be destroyed</param>
    public void DestroyTopTile(Tile tile)
    {
        var topTile = tile.HighestTileFromAbove;
        if(!topTile.IsPlayerAbleToDestroy(this))
            return;

        _playerInventory.AddTile(topTile);
        topTile.DestroyTile();

        SubtractActivePoints(topTile.TileData.PointsToDestroy);
    }

    public Tile GetTileUnderneathWithRaycast()
    {
        var rayDownwards = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(rayDownwards, out hit))
            return hit.transform.GetComponent<Tile>();

        return null;
    }

    /// <summary>Method that sets the conditions for player when its turn begins</summary>
    public void StartTurn()
    {
        _pointsLeftForTheTurn = PointsAtTheBeginningOfTheTurn;
        _isTurnTime = true;
    }

    /// <summary>Method that sets the conditions for player when its turn begins</summary>
    public void EndTurn()
    {
        _isTurnTime = false;
        _pointsLeftForTheTurn = PointsAtTheBeginningOfTheTurn;
    }

    public void PushOtherPlayer(Player player) => throw new NotImplementedException();

    public void Die()
    {
        if (_playerInventory.GetNumberOfGivenTilesInInventory(ETileType.Bonus) > 0)
            while (_playerInventory.GetNumberOfGivenTilesInInventory(ETileType.Bonus) > 0)
                attachedTile.HighestTileFromAbove.PlaceTileAbove(_playerInventory.TakeTileFromInventory(ETileType.Bonus));

        _pointsLeftForTheTurn = playerData.PointsForMovementTaken;

        if(attachedTile != null)
            attachedTile.RemovePlayer();
        if (spawnPoint == null)
        {
            gameObject.SetActive(false);
            return;
        }
        
        MoveTo(spawnPoint);
        EndTurn();
    }

    private void SubtractActivePoints(int pointsToSubtract)
    {
        pointsToSubtract = Mathf.Abs(pointsToSubtract);
        if(_pointsLeftForTheTurn < pointsToSubtract)
            return;

        _pointsLeftForTheTurn -= pointsToSubtract;
    }
}