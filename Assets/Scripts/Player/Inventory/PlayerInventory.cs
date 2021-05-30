using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private PlayerInventoryData inventoryData;
    
    private readonly Dictionary<ETileType, Stack<Tile>> _items = new Dictionary<ETileType, Stack<Tile>>();
    public int NumberOfItems => _items.Sum(pair => pair.Value.Count);
    public int TotalBonusPoints => _items.Sum(pair => pair.Value.Sum(tile => tile.TileData.BonusPoints));

    private Player _player;
    private void Start()
    {
        _player = GetComponent<Player>();
        if(_player == null)
            throw new NullReferenceException("Player component is not attached to the game object");

        foreach (var tileEntry in inventoryData.InitialTiles)
        {
            for (var i = 0; i < tileEntry.initialNumberOfTilesInInventory; i++)
            {
                var tile = Instantiate(tileEntry.tilePrefab).GetComponent<Tile>();
                
                if (tile == null)
                    continue;
                
                if (tile.TileData.TileType == ETileType.SideBlock)
                {
                    var sideBlock = tile as SideTile;
                    if(sideBlock != null)
                        sideBlock.AssignSide(_player.Side);
                }

                AddTile(tile);
                tile.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>Gets number of given TileType in the inventory</summary>
    /// <param name="tileType">TileType for which selection will be made</param>
    /// <returns>Returns number of given TileType in the inventor</returns>
    public int GetNumberOfGivenTilesInInventory(ETileType tileType)
    {
        if (!_items.ContainsKey(tileType))
            return 0;

        return _items[tileType].Count;
    }

    
    /// <summary>Adds tile to players inventory</summary>
    /// <param name="tileToAdd">Tile that will be added to the inventory</param>
    public void AddTile(Tile tileToAdd)
    {
        var itemType = tileToAdd.TileData.TileType;
        
        if(_items.ContainsKey(itemType))
            _items[itemType].Push(tileToAdd);
        else
        {
            _items.Add(itemType, new Stack<Tile>());
            _items[itemType].Push(tileToAdd);
        }
    }

    /// <summary>Finds tile of a given type, removes it from inventory and returns it</summary>
    /// <param name="itemType">TileType that will be found in inventory</param>
    /// <returns>Returns Tile if it is present in inventory. Otherwise returns null</returns>
    public Tile TakeTileFromInventory(ETileType itemType)
    {
        if(GetNumberOfGivenTilesInInventory(itemType) == 0)
            return null;
        
        return _items[itemType].Pop();
    }

    public List<Tile> GetAllTilesOfAGivenType(ETileType type)
    {
        if (!_items.ContainsKey(type))
            return null;

        return _items[type].ToList();
    }
}