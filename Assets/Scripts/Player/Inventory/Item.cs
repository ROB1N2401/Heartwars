using System;
using JetBrains.Annotations;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] [NotNull] private ItemData itemData;
    public int BonusPoints => itemData.bonusPoints;
    public bool IsPlaceable => itemData.isPlaceable;
    public int PlacingPoints => itemData.placingPoints;
    public Sprite DisplaySprite => itemData.displaySprite;
}