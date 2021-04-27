using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/TileData", fileName = "NewTileData")]
public class TileData : ScriptableObject
{
    [Header("Movement options")]
    [SerializeField] private bool isWalkable = false;
    [SerializeField] private ESide tileSide = ESide.Neutral;

    [Header("Destruction options")]
    [SerializeField] private bool isDestroyable = false;
    [SerializeField] [Range(0, 10)] private int pointsToDestroy = 0;

    [Header("Placing options")] 
    [SerializeField] private bool isPlaceable = false;
    [SerializeField] [Range(-10, 0)] private int pointsToPlace = 0;

    [Header("Inventory options")]
    [SerializeField] [Range(0, 10)] private int bonusPoints = 0;
    
    public bool IsWalkable => isWalkable;
    public ESide TileSide => tileSide;
    public bool IsDestroyable => isDestroyable;
    public int PointsToDestroy => pointsToDestroy;
    public bool IsPlaceable => isPlaceable;
    public int PointsToPlace => pointsToPlace;
    public int BonusPoints => bonusPoints;
}
