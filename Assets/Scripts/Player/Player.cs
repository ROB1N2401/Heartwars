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
        if(spawnPoint.TileData.TileType != ETileType.Spawn || spawnPoint == null)
            throw new ArgumentException("SpawnPoint has to be Spawn type, be non null and should have the same side as player");
        spawnPoint.tileSide = side;
        attachedTile = spawnPoint;
        attachedTile.PlacePlayer(this);
        _pointsLeftForTheTurn = PointsAtTheBeginningOfTheTurn;
    }

    /// <summary>Checks if there is item available in players inventory</summary>
    /// <param name="tileType">Tile type for which condition will be checked</param>
    /// <returns>Returns true if such item available in the inventory. Otherwise returns false</returns>
    public bool HasSuchItemInInventory(ETileType tileType)
    {
        if (_playerInventory.GetNumberOfGivenTilesInInventory(tileType) <= 0)
            return false;

        return true;
    }

    /// <summary>Checks if there is item available in players inventory</summary>
    /// <param name="tileType">Tile type for which condition will be checked</param>
    /// <returns>Returns true if such item available in the inventory. Otherwise returns false</returns>
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
    
    /// <summary>Pushes other player to the opposite direction</summary>
    /// <param name="playerToPush">Player that will be pushed</param>
    public void PushOtherPlayer(Player playerToPush)
    {
        if(playerToPush == null)
            return;

        var destinationTile = playerToPush.attachedTile.LowestTileFromUnderneath
            .GetTileFromOppositeDirection(attachedTile.LowestTileFromUnderneath);
        
        if(destinationTile == null)
            playerToPush.Die();
        
        destinationTile.HighestTileFromAbove.PlacePlayer(playerToPush);
    }

    /// <summary>Method that sets the conditions for player when its turn begins</summary>
    public void StartTurn()
    {
        _pointsLeftForTheTurn = PointsAtTheBeginningOfTheTurn;
        _isTurnTime = true;
    }

    /// <summary>Method that sets the conditions for player when its turn ends</summary>
    public void EndTurn()
    {
        _isTurnTime = false;
        _pointsLeftForTheTurn = PointsAtTheBeginningOfTheTurn;
    }

    /// <summary>Method that defines logic behind player death</summary>
    public void Die()
    {
        if (_playerInventory.GetNumberOfGivenTilesInInventory(ETileType.Bonus) > 0)
            while (_playerInventory.GetNumberOfGivenTilesInInventory(ETileType.Bonus) > 0)
                attachedTile.HighestTileFromAbove.PlaceTileAbove(_playerInventory.TakeTileFromInventory(ETileType.Bonus));

        _pointsLeftForTheTurn = playerData.PointsForMovementTaken;

        if(attachedTile != null)
            attachedTile.RemovePlayer();
        if (spawnPoint == null || spawnPoint.isActiveAndEnabled == false)
        {
            gameObject.SetActive(false);
            return;
        }
        
        MoveTo(spawnPoint);
        EndTurn();
    }
    
    /// <summary>Subtracts points left for turn for current player</summary>
    /// <param name="pointsToSubtract">Amount of point to subtract</param>
    private void SubtractActivePoints(int pointsToSubtract)
    {
        pointsToSubtract = Mathf.Abs(pointsToSubtract);
        if(_pointsLeftForTheTurn < pointsToSubtract)
            return;

        _pointsLeftForTheTurn -= pointsToSubtract;
    }
}