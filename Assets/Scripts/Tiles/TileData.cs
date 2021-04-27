using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObjects/TileData", fileName = "NewTileData")]
public class TileData : ScriptableObject
{
    public bool IsWalkable = false;
    public bool IsDestroyable = false;
    public ESide tileESide = ESide.Neutral;
}
