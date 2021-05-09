using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/TileData", fileName = "NewTileData")]
public class TileData : ScriptableObject
{
    [Header("Movement options")]
    [SerializeField] private bool isWalkable = false;

    [Header("Destruction options")]
    [SerializeField] private bool isDestroyable = false;
    [SerializeField] [Range(0, 10)] private int pointsToDestroy = 0;
    [SerializeField] private ESide[] ignoreSides;

    [Header("Building options")]
    [SerializeField] private ETileType tileType = ETileType.Floor;
    [SerializeField] private bool isPlaceable = false;
    [SerializeField] private bool isAllowedToBuildTilesAbove = false;
    [SerializeField] [Range(0, 10)] private int pointsToPlace = 0;

    [Header("Inventory options")]
    [SerializeField] [Range(0, 10)] private int bonusPoints = 0;
    
    public bool IsWalkable => isWalkable;
    public bool IsDestroyable => isDestroyable;
    public int PointsToDestroy => pointsToDestroy;
    public ESide[] IgnoreSides => ignoreSides;
    public ETileType TileType => tileType;
    public bool IsPlaceable => isPlaceable;
    public bool IsAllowedToBuildTilesAbove => isAllowedToBuildTilesAbove;
    public int PointsToPlace => pointsToPlace;
    public int BonusPoints => bonusPoints;
}
