using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class PlayerInventory : MonoBehaviour
{
    private readonly List<Item> items = new List<Item>();

    public int TotalBonusPoints => items.Sum(item => item.BonusPoints);
}
