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

    public int GetNumberOfGivenTilesInInventory(ETileType tileType)
    {
        if (!_items.ContainsKey(tileType))
            return 0;

        return _items[tileType].Count;
    }

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

    public Tile TakeTileFromInventory(ETileType itemType)
    {
        if(GetNumberOfGivenTilesInInventory(itemType) == 0)
            return null;
        
        return _items[itemType].Pop();
    }
}