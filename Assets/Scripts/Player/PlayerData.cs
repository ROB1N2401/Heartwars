using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PlayerData", fileName = "NewPlayerData")]
public class PlayerData : ScriptableObject
{
    [SerializeField] [Range(0, 20)] private int initialNumberOfPoints = 10;
    [SerializeField] [Range(-10, 0)] private int pointsForMovementTaken = -1;

    public int InitialNumberOfPoints => initialNumberOfPoints;
    public int PointsForMovementTaken => pointsForMovementTaken;
}
