using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryButton : TabButton
{
    [Header("Images")]
    [SerializeField] private Sprite spriteMask;

    private GameObject _highlightMask; //highlight mask object should always be the first child in button's ierarchy in order to work properly
    private Image _defaultImage; //default image object should always be the second child in button's ierarchy in order to work properly

    void Start()
    {
        _defaultImage = transform.GetChild(1).GetComponent<Image>();
        _defaultImage.sprite = spriteIdle;
        _highlightMask = transform.GetChild(0).gameObject;
        _highlightMask.GetComponent<Image>().sprite = spriteMask;
        _highlightMask.SetActive(false);
    }

    protected override void OnSelect(TabButton tabButton_in)
    {
        if (this.Id == tabButton_in.Id)
        {
            if (tabManagerRef.SelectedTab != tabButton_in)
            {
                if (tabManagerRef.SelectedTab != null)
                    tabManagerRef.Deselect(tabManagerRef.SelectedTab);

                tabManagerRef.SelectedTab = tabButton_in;
                BrightenImage();
                _highlightMask.SetActive(true);
                tabButton_in.SelectionEvent.Invoke();
            }
        }
    }

    protected override void OnDeSelect(TabButton tabButton_in)
    {
        _highlightMask.SetActive(false);
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
}
