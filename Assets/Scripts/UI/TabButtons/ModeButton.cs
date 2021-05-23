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

    // Start is called before the first frame update
    private void Start()
    {
        ResetSprite();
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
        BrightenImage();
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
            DarkenImage();
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if (this != tabManagerRef.SelectedTab)
            BrightenImage();
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
