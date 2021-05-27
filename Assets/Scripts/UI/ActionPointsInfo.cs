using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ActionPointsInfo : MonoBehaviour
{
    private TextMeshProUGUI _TMProRef = null;

    void Awake()
    {
        _TMProRef = GetComponent<TextMeshProUGUI>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _TMProRef.text = PlayerManager.Instance.CurrentPlayer.PointsLeftForTheTurn.ToString();
    }
}
