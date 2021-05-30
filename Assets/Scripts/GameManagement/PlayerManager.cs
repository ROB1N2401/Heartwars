using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    //Endgame screen variables
    [SerializeField] private float delayTime;


    //Other
    [SerializeField] private List<Player> players;
    private CameraControl _cameraControlRef;
    private Player _currentPlayer = null;
    private int _currentIndex;

    public Player CurrentPlayer => _currentPlayer;

    
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        _cameraControlRef = Camera.main.GetComponent<CameraControl>();
        _currentPlayer = players[0];
        _currentIndex = 0;
    }

    public void EliminatePlayer(Player player_in)
    {
        UpdatePlayerRoster();

        if (players.Count > 1)
            AnimationControl.Instance.RollAnnouncementOut(player_in);
        else
            StartCoroutine(FinishGame()); 
    }


    //todo implement functionality to account for eliminated players
    public void StartNewTurn(float timeoutMillisecond = 0)
    {
        players[_currentIndex].EndTurn();

        if (_currentIndex >= players.Count - 1)
            _currentIndex = 0;
        else _currentIndex++;
       
        _currentPlayer = players[_currentIndex];

        BonusTab.Instance.UpdateBonusTab();

        _cameraControlRef.FocusCameraAboveObject(_currentPlayer.gameObject, timeoutMillisecond);

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

    private IEnumerator FinishGame()
    {
        var canvas = FindObjectOfType<Canvas>();
        for (int i = 0; i < canvas.transform.childCount - 1; i++) //Disabling all of the child components for canvas, except for the endgame screen
        {
            canvas.transform.GetChild(i).gameObject.SetActive(false);
        }

        _cameraControlRef.FocusCameraAboveObject(players[0].gameObject);

        yield return new WaitForSeconds(delayTime);

        int j = canvas.transform.childCount - 1;
        var endgameScreen = canvas.transform.GetChild(j).gameObject;
        var tmproRef = endgameScreen.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        tmproRef.text = $"{players[0].Side} player has won!";
        endgameScreen.SetActive(true);

        yield return new WaitForSeconds(delayTime);

        var continueButton = endgameScreen.transform.GetChild(1).gameObject;
        continueButton.SetActive(true);
    }
}
