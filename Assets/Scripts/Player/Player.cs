using System;
using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof(PlayerInventory))]
public class Player : MonoBehaviour
{
    [SerializeField] [NotNull] private PlayerData playerData;
    [SerializeField] private ESide side = ESide.Blue;
    [SerializeField] [NotNull] private Tile spawnPoint;
    
    public int PointsForTurn => playerData.InitialNumberOfPoints + _playerInventory.TotalBonusPoints;
    public int PointsLeftForTheTurn => _pointsLeftForTheTurn;
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

    public void MoveTo(Tile tile)
    {
        if(!tile.IsFreeToPlacePlayer || _pointsLeftForTheTurn < playerData.PointsForMovementTaken)
            return;

        tile.PlacePlayer(this);
        _attachedTile.RemovePlayer();
        _attachedTile = tile;
        
        gameObject.transform.position = tile.transform.position + tile.PlayerPositionOffset;
        
        SubtractActivePoints(playerData.PointsForMovementTaken);
    }

    public void DestroyAndAddTileToInventory(Tile tile)
    {
        if(!tile.IsPlayerAbleToDestroy(this))
            return;
        
        _playerInventory.AddTile(tile);
        tile.DestroyTile();
    }

    private void SubtractActivePoints(int pointsToSubtract)
    {
        pointsToSubtract = Mathf.Clamp(pointsToSubtract, 0, int.MaxValue);
        if(_pointsLeftForTheTurn < pointsToSubtract)
            return;

        _pointsLeftForTheTurn -= pointsToSubtract;
    }

    public void EndTurn() => _pointsLeftForTheTurn = PointsForTurn;
    public void KillPlayer() => throw new NotImplementedException();
    public void PushOtherPlayer(Player player) => throw  new NotImplementedException();
}