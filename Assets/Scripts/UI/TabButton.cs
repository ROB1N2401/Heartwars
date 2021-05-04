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
    [SerializeField] private UnityEvent selectionEvent;
    [SerializeField] private UnityEvent deselectionEvent;
    [SerializeField] private Sprite spriteIdle;
    [SerializeField] private Sprite spriteHovered;
    [SerializeField] private Sprite spriteSelected;
    private Image _imageRef;



    // Start is called before the first frame update
    private void Start()
    {
        _imageRef = GetComponent<Image>();
        ResetSprite();
        tabManagerRef.IncludeButton(this);
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    public void ResetSprite()
    {
        _imageRef.sprite = spriteIdle;
    }

    private void Select()
    {
        tabManagerRef.SelectedTab = this;
        _imageRef.sprite = spriteSelected;
        selectionEvent.Invoke();
    }

    public void Deselect()
    {
        ResetSprite();
        deselectionEvent.Invoke();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(this != tabManagerRef.SelectedTab)
        {
            if (tabManagerRef.SelectedTab != null)
            {
                tabManagerRef.SelectedTab.Deselect();
            }
            Select();
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
