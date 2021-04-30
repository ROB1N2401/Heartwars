﻿using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/TileData", fileName = "NewTileData")]
public class TileData : ScriptableObject
{
    [Header("Movement options")]
    [SerializeField] private bool isWalkable = false;
    [SerializeField] private ESide side = ESide.Neutral;

    [Header("Destruction options")]
    [SerializeField] private bool isDestroyable = false;
    [SerializeField] [Range(0, 10)] private int pointsToDestroy = 0;

    [Header("Building options")]
    [SerializeField] private ETileType tileType = ETileType.Floor;
    [SerializeField] private bool isPlaceable = false;
    [SerializeField] private bool isAllowedToPlaceTilesAbove = false;
    [SerializeField] [Range(0, 10)] private int pointsToPlace = 0;

    [Header("Inventory options")]
    [SerializeField] [Range(0, 10)] private int bonusPoints = 0;
    
    public bool IsWalkable => isWalkable;
    public ESide Side => side;
    public bool IsDestroyable => isDestroyable;
    public int PointsToDestroy => pointsToDestroy;
    public ETileType TileType => tileType;
    public bool IsPlaceable => isPlaceable;
    public bool IsAllowedToPlaceTilesAbove => isAllowedToPlaceTilesAbove;
    public int PointsToPlace => pointsToPlace;
    public int BonusPoints => bonusPoints;
}
