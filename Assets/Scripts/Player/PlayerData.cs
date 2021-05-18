using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PlayerData", fileName = "NewPlayerData")]
public class PlayerData : ScriptableObject
{
    [SerializeField] [Range(0, 100)] private int initialNumberOfPoints = 10;
    [SerializeField] [Range(0, 10)] private int pointsForMovementTaken = 1;
    [SerializeField] [Range(0, 10)] private int pointsForPushTaken = 1;

    public int InitialNumberOfPoints => initialNumberOfPoints;
    public int PointsForMovementTaken => pointsForMovementTaken;
    public int PointsForPushTaken => pointsForPushTaken;
}
