using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EndTurnControl : MonoBehaviour, IPointerClickHandler
{
    CameraControl cameraControlRef;

    void Awake()
    {
        cameraControlRef = Camera.main.GetComponent<CameraControl>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (cameraControlRef.IsControllable)
        {
            PlayerManager.Instance.StartNewTurn();
        }
    }
}
