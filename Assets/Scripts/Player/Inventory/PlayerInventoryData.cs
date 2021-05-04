using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PlayerInventoryData", fileName = "NewPlayerInventoryData")]
public class PlayerInventoryData : ScriptableObject
{
    [SerializeField] private InitialTileEntry[] initialTiles;

    public InitialTileEntry[] InitialTiles => initialTiles;
}

[Serializable]
public class InitialTileEntry
{
    public GameObject tilePrefab;
    [Min(0)] public int numberOfTilesInInventory = 0;
}