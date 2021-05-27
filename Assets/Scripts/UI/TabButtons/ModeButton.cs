using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ModeButton : TabButton
{
    [Header("Images")]
    [SerializeField] private Sprite spriteSelected;

    private RectTransform _iconTransform; //icon holder object should always be the first child in button's ierarchy in order to work properly
    private Vector2 _idlePos;
    private Vector2 _pressedPos;
    private Vector2 _spriteOffset = new Vector2(3.0f, -3.0f);

    // Start is called before the first frame update
    private void Start()
    {
        _iconTransform = transform.GetChild(0).GetComponent<RectTransform>();
        _idlePos = _iconTransform.anchoredPosition;
        _pressedPos = _iconTransform.anchoredPosition + _spriteOffset;
        ResetIcon();
    }

    protected override void OnSelect(TabButton tabButton_in)
    {
        if(this.Id == tabButton_in.Id)
        {
            if (tabManagerRef.SelectedTab != tabButton_in)
            {
                if (tabManagerRef.SelectedTab != null)
                    tabManagerRef.Deselect(tabManagerRef.SelectedTab);

                tabManagerRef.SelectedTab = tabButton_in;
                UpdateIcon();
                tabButton_in.SelectionEvent.Invoke();
            }
        }
    }

    protected override void OnDeSelect(TabButton tabButton_in)
    {
        ResetIcon();
        BrightenImage();
        tabButton_in.DeselectionEvent.Invoke();
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (this != tabManagerRef.SelectedTab)
        {
     
            tabManagerRef.Select(this);
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (this != tabManagerRef.SelectedTab)
            DarkenImage();
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if (this != tabManagerRef.SelectedTab)
            BrightenImage();
    }

    public void ResetIcon()
    {
        imageRef.sprite = spriteIdle;
        _iconTransform.anchoredPosition = _idlePos;
    }

    public void UpdateIcon()
    {
        imageRef.sprite = spriteSelected;
        _iconTransform.anchoredPosition = _pressedPos;
    }
}
