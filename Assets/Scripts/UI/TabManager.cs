using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TabManager : MonoBehaviour
{ 
    private TabButton _selectedTab = null;
    private List<TabButton> _tabButtonEntry = null;
    private int randomizeSounds;

    public Action<TabButton> SelectButton;
    public Action<TabButton> DeselectButton;

    public TabButton SelectedTab { get => _selectedTab; set => _selectedTab = value; }
    public List<TabButton> TabButtonEntry => _tabButtonEntry;

    void Awake()
    {
        _tabButtonEntry = new List<TabButton>();

        for (int i = 0; i < transform.childCount; i++)
        {
            var temp = transform.GetChild(i).GetComponent<TabButton>();
            if(temp != null)
                _tabButtonEntry.Add(temp);
        }
        for (int i = 0; i < _tabButtonEntry.Count; i++)
        {
            _tabButtonEntry[i].GetComponent<TabButton>().Id = i;
        }
    }

    public void Select(TabButton button_in)
    {
        SelectButton?.Invoke(button_in);
    }

    public void Deselect(TabButton button_in)
    {
        DeselectButton?.Invoke(button_in);
    }

    public void Blank()
    {

    }
}
