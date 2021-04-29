using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private readonly Dictionary<ETileType, List<Tile>> _items = new Dictionary<ETileType, List<Tile>>();
    public int TotalBonusPoints => _items.Sum(pair => pair.Value.Sum(tile => tile.TileData.BonusPoints));

    public int GetNumberOfGivenTilesInInventory(ETileType tileType)
    {
        if (!_items.ContainsKey(tileType))
            return 0;
        
        return _items.Where(pair => pair.Key == tileType).Sum(pair => pair.Value.Count);
    }

    public void AddTile(Tile tileToAdd)
    {
        var itemType = tileToAdd.TileData.TileType;
        
        if(_items.ContainsKey(itemType))
            _items[itemType].Add(tileToAdd);
        else
            _items.Add(itemType, new List<Tile>(){tileToAdd});
    }

    public Tile DeleteAndGetTile(ETileType itemType)
    {
        if(GetNumberOfGivenTilesInInventory(itemType) == 0)
            return null;

        var itemToReturn = _items[itemType][0];
        _items[itemType].RemoveAt(0);
        return itemToReturn;
    }
}