using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class TabButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TabManager tabManagerRef;
    [Header("UnityEvents")]
    [SerializeField] private UnityEvent selectionEvent;
    [SerializeField] private UnityEvent deselectionEvent;
    [Header("Images")]
    [SerializeField] private Sprite spriteIdle;
    [SerializeField] private Sprite spriteHovered;
    [SerializeField] private Sprite spriteSelected;
    private Image _imageRef;

    public UnityEvent SelectionEvent => selectionEvent;
    public UnityEvent DeselectionEvent => deselectionEvent;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    private void Start()
    {
        _imageRef = GetComponent<Image>();
        ResetSprite();
    }

    public void ResetSprite()
    {
        _imageRef.sprite = spriteIdle;
    }

    public void SetActiveSprite()
    {
        _imageRef.sprite = spriteSelected;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(this != tabManagerRef.SelectedTab)
        {
            tabManagerRef.Select(this);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(this != tabManagerRef.SelectedTab)
            _imageRef.sprite = spriteHovered;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (this != tabManagerRef.SelectedTab)
            _imageRef.sprite = spriteIdle;
    }
}
