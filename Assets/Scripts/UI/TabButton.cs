using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public abstract class TabButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private int _id;

    protected TabManager tabManagerRef;

    [Header("UnityEvents")]
    [SerializeField] protected UnityEvent selectionEvent;
    [SerializeField] protected UnityEvent deselectionEvent;

    protected Image imageRef;

    public int Id { get => _id; set => _id = value; }
    public UnityEvent SelectionEvent => selectionEvent;
    public UnityEvent DeselectionEvent => deselectionEvent;



    private void Awake()
    {
        tabManagerRef = transform.GetComponentInParent<TabManager>();
        imageRef = GetComponent<Image>();
    }

    protected abstract void OnSelect(TabButton tabButton_in);
    protected abstract void OnDeSelect(TabButton tabButton_in);
    public abstract void OnPointerClick(PointerEventData eventData);
    public abstract void OnPointerEnter(PointerEventData eventData);
    public abstract void OnPointerExit(PointerEventData eventData);
}
