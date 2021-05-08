using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ActionPointsInfo : MonoBehaviour
{
    private Player _playerRef = null;
    private TextMeshProUGUI _TMProRef = null;

    void Awake()
    {
        _playerRef = FindObjectOfType<Player>();
        _TMProRef = GetComponent<TextMeshProUGUI>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log($"ref: {_playerRef.PointsLeftForTheTurn} ");
        _TMProRef.text = _playerRef.PointsLeftForTheTurn.ToString();
    }
}
