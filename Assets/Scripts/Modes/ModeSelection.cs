using UnityEngine;

public class ModeSelection : MonoBehaviour
{
    private TabManager _tabManagerRef;
    private InputMaster _inputMaster;
    private KeyCode _testKey;

    void Awake()
    { 
        _tabManagerRef = FindObjectOfType<TabManager>();
        _inputMaster = new InputMaster();
        _inputMaster.Main.MovementMode.performed += ctx => _tabManagerRef.Select(_tabManagerRef.TabButtonEntry[0]);
        _inputMaster.Main.DestructionMode.performed += ctx => _tabManagerRef.Select(_tabManagerRef.TabButtonEntry[1]);
        _inputMaster.Main.PFloorMode.performed += ctx => _tabManagerRef.Select(_tabManagerRef.TabButtonEntry[2]);
        _inputMaster.Main.PushingMode.performed += ctx => _tabManagerRef.Select(_tabManagerRef.TabButtonEntry[3]);
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