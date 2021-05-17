using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(PlayerInventory))]
public class Player : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private ESide side = ESide.Blue;
    [SerializeField] private Tile spawnPoint;

    public int PointsAtTheBeginningOfTheTurn => playerData.InitialNumberOfPoints + _playerInventory.TotalBonusPoints;
    public int PointsLeftForTheTurn { get; private set; } = 0;
    public bool IsTurnTime { get; private set; } = false;
    public bool IsAlive { get; private set; } = true;
    public PlayerData PlayerData => playerData;

    [HideInInspector] public Tile attachedTile;
    public Tile SpawnPoint => spawnPoint;
    public ESide Side => side;
    
    private PlayerInventory _playerInventory;
    private int randomizeSounds;

    private void Start()
    {
        _playerInventory = GetComponent<PlayerInventory>();
        if(spawnPoint.TileData.TileType != ETileType.Spawn || spawnPoint == null)
            throw new ArgumentException("SpawnPoint has to be Spawn type, be non null and should have the same side as player");
        spawnPoint.tileSide = side;
        attachedTile = spawnPoint;
        attachedTile.PlacePlayer(this);
        PointsLeftForTheTurn = PointsAtTheBeginningOfTheTurn;
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

        AudioManager.instance.shouldRandomizePitch = true;
        randomizeSounds = Random.Range(0, 5);
        if (randomizeSounds == 0)
        {
            AudioManager.instance.PlaySound("MovePiece1");
        }
        else if (randomizeSounds == 1)
        {
            AudioManager.instance.PlaySound("MovePiece2");
        }
        else if (randomizeSounds == 2)
        {
            AudioManager.instance.PlaySound("MovePiece3");
        }
        else if (randomizeSounds == 3)
        {
            AudioManager.instance.PlaySound("MovePiece4");
        }
        else if (randomizeSounds == 4)
        {
            AudioManager.instance.PlaySound("MovePiece5");
        }
        else
        {
            return;
        }
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
        PointsLeftForTheTurn = PointsAtTheBeginningOfTheTurn;
        IsTurnTime = true;
    }

    /// <summary>Method that sets the conditions for player when its turn ends</summary>
    public void EndTurn()
    {
        IsTurnTime = false;
        PointsLeftForTheTurn = PointsAtTheBeginningOfTheTurn;
    }

    /// <summary>Method that defines logic behind player death</summary>
    public void Die()
    {
        if (_playerInventory.GetNumberOfGivenTilesInInventory(ETileType.Bonus) > 0)
            while (_playerInventory.GetNumberOfGivenTilesInInventory(ETileType.Bonus) > 0)
                attachedTile.HighestTileFromAbove.PlaceTileAbove(_playerInventory.TakeTileFromInventory(ETileType.Bonus));

        PointsLeftForTheTurn = playerData.PointsForMovementTaken;

        AudioManager.instance.shouldRandomizePitch = true;
        randomizeSounds = Random.Range(0, 3);
        if (randomizeSounds == 0)
        {
            AudioManager.instance.PlaySound("Falling1");
        }
        else if (randomizeSounds == 1)
        {
            AudioManager.instance.PlaySound("Falling2");
        }
        else if (randomizeSounds == 2)
        {
            AudioManager.instance.PlaySound("Falling3");
        }
        else
        {
            return;
        }

        if (attachedTile != null)
            attachedTile.RemovePlayer();
        if (spawnPoint == null || spawnPoint.isActiveAndEnabled == false)
        {
            gameObject.SetActive(false);
            IsAlive = false;
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
        if(PointsLeftForTheTurn < pointsToSubtract)
            return;

        PointsLeftForTheTurn -= pointsToSubtract;
    }
}