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

    public Tile attachedTile;
    public Tile SpawnPoint => spawnPoint;
    public ESide Side => side;

    private bool _isTurnTime = false;
    private int _pointsLeftForTheTurn = 0;
    private PlayerInventory _playerInventory;

    private void Start()
    {
        _playerInventory = GetComponent<PlayerInventory>();
        attachedTile = spawnPoint;
        attachedTile.PlacePlayer(this);
        _pointsLeftForTheTurn = PointsAtTheBeginningOfTheTurn;
    }

    /// <summary>Moves player to the given tile</summary>
    /// <param name="tile">Target tile where player will be moved</param>
    public void MoveTo(Tile tile)
    {
        if(!tile.IsPlayerAbleToMove(this))
            return;

        attachedTile.RemovePlayer();
        tile.PlacePlayer(this);

        gameObject.transform.position = tile.transform.position + tile.PlayerPositionOffset;
        
        SubtractActivePoints(playerData.PointsForMovementTaken);
    }

    /// <summary>Method that places tile by player</summary>
    /// <param name="tileType"></param>
    /// <param name="tileToPlaceOn"></param>
    public void PlaceTile(ETileType tileType) => throw new NotImplementedException();

    /// <summary>Destroys given tile tile</summary>
    /// <param name="tile">Tile that will be destroyed</param>
    public void DestroyTile(Tile tile)
    {
        var topTile = tile.HighestTileFromAbove;
        if(!topTile.IsPlayerAbleToDestroy(this))
            return;

        _playerInventory.AddTile(topTile);
        topTile.DestroyTile();
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
        _pointsLeftForTheTurn = playerData.PointsForMovementTaken;
        
        attachedTile.RemovePlayer();
        MoveTo(spawnPoint);
        EndTurn();
    }

    private void SubtractActivePoints(int pointsToSubtract)
    {
        pointsToSubtract = Mathf.Max(pointsToSubtract, 0);
        if(_pointsLeftForTheTurn < pointsToSubtract)
            return;

        _pointsLeftForTheTurn -= pointsToSubtract;
    }
}