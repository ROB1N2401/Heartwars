using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private readonly List<Tile> _items = new List<Tile>();
    public int TotalBonusPoints => _items.Sum(tile => tile.TileData.BonusPoints);
    
    public void AddTile(Tile item) => _items.Add(item);
    public void DeleteTile(Tile item) => throw new NullReferenceException();
}