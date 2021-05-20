using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TabManager : MonoBehaviour
{ 
    private TabButton _selectedTab = null;
    private List<TabButton> _tabButtonEntry = null;
    private int randomizeSounds;

    public TabButton SelectedTab { get => _selectedTab; set => _selectedTab = value; }
    public List<TabButton> TabButtonEntry => _tabButtonEntry;

    void Awake()
    {
        _tabButtonEntry = new List<TabButton>();

        for (int i = 0; i < transform.childCount; i++)
        {
            _tabButtonEntry.Add(transform.GetChild(i).GetComponent<TabButton>());
        }
    }

    public void Select(TabButton button_in)
    {
        if(_selectedTab != button_in)
        {
            if(_selectedTab != null)
                DeselectActiveButton();

            _selectedTab = button_in;
            button_in.SetActiveSprite();
            button_in.SelectionEvent.Invoke();
            AudioManager.instance.shouldRandomizePitch = true;

            randomizeSounds = Random.Range(0, 2);
            if (randomizeSounds == 0)
            {
                AudioManager.instance.PlaySound("UiChangeMode1");
            }
            else if (randomizeSounds == 1)
            {
                AudioManager.instance.PlaySound("UiChangeMode2");
            }
            else
            {
                return;
            }
        }
    }

    private void DeselectActiveButton()
    {
        _selectedTab.ResetSprite();
        _selectedTab.DeselectionEvent.Invoke();
        _selectedTab = null;
    }
}
