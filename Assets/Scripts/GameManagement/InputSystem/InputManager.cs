using UnityEngine;

public class InputManager : MonoBehaviour
{
    private InputMaster _inputMaster;
    [Header("Tab Managers")]
    [SerializeField] private TabManager _modeTabManagerRef;
    //[SerializeField] private TabManager _inventoryTabManagerRef;
    //[SerializeField] private TabManager _tabManagerRef;
    //private KeyCode _testKey;

    void Awake()
    { 
        _inputMaster = new InputMaster();
        _inputMaster.Main.MovementMode.performed += ctx => _modeTabManagerRef.Select(_modeTabManagerRef.TabButtonEntry[0]);
        _inputMaster.Main.DestructionMode.performed += ctx => _modeTabManagerRef.Select(_modeTabManagerRef.TabButtonEntry[1]);
        _inputMaster.Main.PlacementMode.performed += ctx => _modeTabManagerRef.Select(_modeTabManagerRef.TabButtonEntry[2]);
        _inputMaster.Main.PushingMode.performed += ctx => _modeTabManagerRef.Select(_modeTabManagerRef.TabButtonEntry[3]);


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