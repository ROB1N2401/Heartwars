using System;
using UnityEngine;

[RequireComponent(typeof(PlayerInventory))]
public class Player : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private ESide side = ESide.Blue;
    [SerializeField] private Tile spawnPoint;
    
    public int PointsForTurn => playerData.InitialNumberOfPoints + _playerInventory.TotalBonusPoints;
    public int PointsLeftForTheTurn => _pointsLeftForTheTurn;
    public PlayerData PlayerData => playerData;

    public Tile SpawnPoint => spawnPoint;
    public ESide Side => side;

    private int _pointsLeftForTheTurn = 0;
    private PlayerInventory _playerInventory;
    private Tile _attachedTile;

    private void Start()
    {
        _playerInventory = GetComponent<PlayerInventory>();
        _attachedTile = spawnPoint;
        _attachedTile.PlacePlayer(this);
    }

    private void SubtractActivePoints(int pointsToSubtract)
    {
        pointsToSubtract = Mathf.Clamp(pointsToSubtract, 0, int.MaxValue);
        if(_pointsLeftForTheTurn < pointsToSubtract)
            return;

        _pointsLeftForTheTurn -= pointsToSubtract;
    }

    public void MoveTo(Tile tile)
    {
        if(tile.IsPlayerAbleToMove(this))
            return;

        tile.PlacePlayer(this);
        _attachedTile.RemovePlayer();
        _attachedTile = tile;
        
        gameObject.transform.position = tile.transform.position + tile.PlayerPositionOffset;
        
        SubtractActivePoints(playerData.PointsForMovementTaken);
    }

    public void DestroyAndAddTileToInventory(Tile tile)
    {
        var topTile = tile.GetHighestTileFromAbove();
        if(!topTile.IsPlayerAbleToDestroy(this))
            return;

        _playerInventory.AddTile(topTile);
        topTile.DestroyTile();
    }

    public void EndTurn() => _pointsLeftForTheTurn = PointsForTurn;
    public void KillPlayer() => throw new NotImplementedException();
    public void PushOtherPlayer(Player player) => throw  new NotImplementedException();
}