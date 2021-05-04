using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabManager : MonoBehaviour
{
    private List<TabButton> _tabButtons = null;
    private TabButton _selectedTab = null;

    public TabButton SelectedTab { get => _selectedTab; set => _selectedTab = value; }

    public void IncludeButton(TabButton button_in)
    {
        if (_tabButtons == null)
        {
            _tabButtons = new List<TabButton>();
        }

        _tabButtons.Add(button_in);
    }
}
