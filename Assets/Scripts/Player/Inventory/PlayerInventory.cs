using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    
    private readonly List<Item> _items = new List<Item>();
    public int TotalBonusPoints => _items.Sum(item => item.BonusPoints);
    
    public void AddItem(Item item) => _items.Add(item);
    //todo add bonus item logic
}