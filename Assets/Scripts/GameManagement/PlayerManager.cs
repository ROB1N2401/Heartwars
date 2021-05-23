using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    [SerializeField] private List<Player> players;

    private Player _currentPlayer = null;
    private int _currentIndex;

    public Player CurrentPlayer => _currentPlayer;

    
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        _currentPlayer = players[0];
        _currentIndex = 0;
    }

    //todo implement functionality to account for eliminated players
    public void StartNewTurn()
    {
        players[_currentIndex].EndTurn();

        UpdatePlayerRoster();

        if (_currentIndex >= players.Count - 1)
            _currentIndex = 0;
        else _currentIndex++;
       
        _currentPlayer = players[_currentIndex];

        var cameraControl = Camera.main.GetComponent<CameraControl>();
        if (cameraControl != null)
            cameraControl.FocusCameraAboveObject(_currentPlayer.gameObject);

        players[_currentIndex].StartTurn();
    }

    private void UpdatePlayerRoster()
    {
        for(int i = 0; i < players.Count; i++)
        {
            if (!players[i].IsAlive)
                players.RemoveAt(i);
        }
    }
}
