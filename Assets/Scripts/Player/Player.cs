using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof(PlayerInventory))]
public class Player : MonoBehaviour
{
    [SerializeField] [NotNull] private PlayerData playerData;
    [SerializeField] [NotNull] private Tile spawnPoint;
    
    public int PointsForTurn => playerData.initialNumberOfPoints + _playerInventory.TotalBonusPoints;
    
    private PlayerInventory _playerInventory;
    private Tile _attachedTile;
    private int _pointsLeftForTheTurn = 0;

    private void Start()
    {
        _playerInventory = GetComponent<PlayerInventory>();
    }
}
