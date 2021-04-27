using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInventory))]
public class Player : MonoBehaviour
{
    private PlayerInventory _playerInventory;
    private Tile _attachedTile;
    
    private void Start()
    {
        _playerInventory = GetComponent<PlayerInventory>();
    }

}
