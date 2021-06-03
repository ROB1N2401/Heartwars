using System;
using UnityEngine;

[RequireComponent(typeof(PlayerInventory))]
[RequireComponent(typeof(PlayerTransition))]
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
    private PlayerTransition _animation;
    private void Start()
    {
        _playerInventory = GetComponent<PlayerInventory>();
        _animation = GetComponent<PlayerTransition>();
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
        topTile.DestroyTile(this);

        //todo: find a way to decouple this method from player's logic
        if (topTile.TileData.TileType == ETileType.Bonus || topTile.TileData.TileType == ETileType.Spawn)
            BonusTab.Instance.UpdateBonusTab();

        SubtractActivePoints(topTile.TileData.PointsToDestroy);
    }
    
    /// <summary>Moves player to the given tile</summary>
    /// <param name="tile">Target tile where player will be moved</param>
    public void MoveTo(Tile tile)
    {
        var topTile = tile.HighestTileFromAbove;
        if(!topTile.IsPlayerAbleToMove(this))
            return;
        
        topTile.PlacePlayer(this);
        
        SubtractActivePoints(playerData.PointsForMovementTaken);
        AudioManager.InvokeWalkingSound();
    }
    
    /// <summary>Pushes other player to the opposite direction</summary>
    /// <param name="playerToPush">Player that will be pushed</param>
    public void PushOtherPlayer(Player playerToPush)
    {
        if(playerToPush == null || PointsLeftForTheTurn < playerData.PointsForPushTaken)
            return;

        var destinationTile = playerToPush.attachedTile.LowestTileFromUnderneath
            .GetTileFromOppositeDirection(attachedTile.LowestTileFromUnderneath);

        if (destinationTile == null)
        {
            SubtractActivePoints(playerData.PointsForPushTaken);
            playerToPush.Die();
            return;
        }

        destinationTile = destinationTile.HighestTileFromAbove;

        if (destinationTile.TileData.IsWalkable || destinationTile.TileData.TileType == ETileType.Void)
        {
            SubtractActivePoints(playerData.PointsForPushTaken);
            destinationTile.PlacePlayer(playerToPush);
        }
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

        if (spawnPoint == null || spawnPoint.isActiveAndEnabled == false)
        {
            void PreAction() => AudioManager.InvokeDeathSound();
            void AfterAction() => gameObject.SetActive(false);
            
            if (attachedTile != null && attachedTile.TileData.TileType == ETileType.Void)
            {
                _animation
                    .Fly(25f, Vector3.down, PreAction, AfterAction);
                attachedTile.RemovePlayer();
            }
            else
            {
                //todo replace with flying up animation
                _animation
                    .Fly(25f, Vector3.up, PreAction, AfterAction);
                attachedTile.RemovePlayer();
            }

            IsAlive = false;
            PlayerManager.Instance.EliminatePlayer(this);
            return;
        }
        
        if (attachedTile != null && attachedTile.TileData.TileType == ETileType.Void)
        {
            _animation
                .Fly(25f, Vector3.down, preAction: AudioManager.InvokeDeathSound);
            attachedTile.RemovePlayer();
        }
        else
        {
            //todo replace with flying up animation
            _animation
                .Fly(25f, Vector3.up, preAction: AudioManager.InvokeDeathSound);
            attachedTile.RemovePlayer();
        }

        spawnPoint.PlacePlayer(this, ETransitionType.Spawn);
        EndTurn();
        if (this == PlayerManager.Instance.CurrentPlayer)
            PlayerManager.Instance.StartNewTurn(1500);
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