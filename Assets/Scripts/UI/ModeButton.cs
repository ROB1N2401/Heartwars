using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ModeButton : TabButton
{
    [Header("Images")]
    [SerializeField] private Sprite spriteIdle;
    [SerializeField] private Sprite spriteHovered;
    [SerializeField] private Sprite spriteSelected;

    private void Start()
    {
        ResetSprite();
    }

    private void OnEnable()
    {
        TabManager.SelectButton += OnSelect;
        TabManager.DeselectButton += OnDeSelect;
    }

    private void OnDisable()
    {
        TabManager.SelectButton -= OnSelect;
        TabManager.DeselectButton -= OnDeSelect;
    }

    private void OnDestroy()
    {
        TabManager.SelectButton -= OnSelect;
        TabManager.DeselectButton -= OnDeSelect;
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
                SetActiveSprite();
                tabButton_in.SelectionEvent.Invoke();
            }
        }
    }

    protected override void OnDeSelect(TabButton tabButton_in)
    {
        ResetSprite();
        tabButton_in.DeselectionEvent.Invoke();
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (this != tabManagerRef.SelectedTab)
        {
            SetActiveSprite();
            tabManagerRef.Select(this);
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (this != tabManagerRef.SelectedTab)
            imageRef.sprite = spriteHovered;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if (this != tabManagerRef.SelectedTab)
            ResetSprite();
    }

    public void ResetSprite()
    {
        imageRef.sprite = spriteIdle;
    }

    public void SetActiveSprite()
    {
        imageRef.sprite = spriteSelected;
    }
}
