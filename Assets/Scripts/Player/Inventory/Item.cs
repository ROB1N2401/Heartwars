using System;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] [Min(0)] private int bonusPoints;
    
    public int BonusPoints => bonusPoints;
}