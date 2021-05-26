using UnityEngine;

public class InputManager : MonoBehaviour
{
    private InputMaster _inputMaster;
    [Header("Tab Managers")]
    [SerializeField] private TabManager _modeTabManagerRef;
    [SerializeField] private TabManager _inventoryTabManagerRef;
    //[SerializeField] private TabManager _tabManagerRef;
    //private KeyCode _testKey;

    void Awake()
    { 
        _inputMaster = new InputMaster();
        _inputMaster.Main.MovementMode.performed += ctx => _modeTabManagerRef.Select(_modeTabManagerRef.TabButtonEntry[0]);
        _inputMaster.Main.DestructionMode.performed += ctx => _modeTabManagerRef.Select(_modeTabManagerRef.TabButtonEntry[1]);
        _inputMaster.Main.PlacementMode.performed += ctx => _modeTabManagerRef.Select(_modeTabManagerRef.TabButtonEntry[2]);
        _inputMaster.Main.PushingMode.performed += ctx => _modeTabManagerRef.Select(_modeTabManagerRef.TabButtonEntry[3]);

        _inputMaster.Main.SelectFloor.performed += ctx => SelectNewBlock(_inventoryTabManagerRef.TabButtonEntry[0]);
        _inputMaster.Main.SelectWall.performed += ctx => SelectNewBlock(_inventoryTabManagerRef.TabButtonEntry[1]);
        _inputMaster.Main.SelectIce.performed += ctx => SelectNewBlock(_inventoryTabManagerRef.TabButtonEntry[2]);
        _inputMaster.Main.SelectCamp.performed += ctx => SelectNewBlock(_inventoryTabManagerRef.TabButtonEntry[3]);
        _inputMaster.Main.SelectTrampoline.performed += ctx => SelectNewBlock(_inventoryTabManagerRef.TabButtonEntry[4]);
    }

    void SelectNewBlock(TabButton tabButton_in)
    {
        _modeTabManagerRef.Select(_modeTabManagerRef.TabButtonEntry[2]);
        _inventoryTabManagerRef.Select(tabButton_in);
    }

    void OnEnable()
    {
        _inputMaster.Enable();
    }

    void OnDisable()
    {
        _inputMaster.Disable();
    }
}