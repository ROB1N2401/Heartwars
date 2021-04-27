using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ItemData", fileName = "NewItemData")]
public class ItemData : ScriptableObject
{
    [Header("UI")]
    [NotNull] public Sprite displaySprite;

    [Header("Bonus options")] 
    [Min(0)]public int bonusPoints = 0;

    [Header("Placing options")]
    [SerializeField] public bool isPlaceable = false;
    [Range(-10, 0)] public int placingPoints = 0;
    public Tile tile = null;
}