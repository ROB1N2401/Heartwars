// GENERATED AUTOMATICALLY FROM 'Assets/Input Master.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InputMaster : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputMaster()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Input Master"",
    ""maps"": [
        {
            ""name"": ""Main"",
            ""id"": ""4ab4e6ba-3842-4fa7-94a4-44e9af81651f"",
            ""actions"": [
                {
                    ""name"": ""Movement Mode"",
                    ""type"": ""Button"",
                    ""id"": ""bf675154-e0fc-4510-9e6e-9677cf1deb21"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Destruction Mode"",
                    ""type"": ""Button"",
                    ""id"": ""791cfa2c-1bbf-46af-ad59-74abf21a4951"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""P Floor Mode"",
                    ""type"": ""Button"",
                    ""id"": ""100637e0-a587-4647-ac51-eb1f99fb130e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""P Wall Mode"",
                    ""type"": ""Button"",
                    ""id"": ""96f98c9b-c696-404c-918d-e5ca9e746b7b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""P Ice Mode"",
                    ""type"": ""Button"",
                    ""id"": ""e2ece5d1-1ae1-4666-95b5-3d36441a66fb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""P Coloured Tile Mode"",
                    ""type"": ""Button"",
                    ""id"": ""ef0bf8a2-98fa-4a12-a58f-6b49d1b354a4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""P Trampoline Mode"",
                    ""type"": ""Button"",
                    ""id"": ""4b574a3f-d6cf-4a39-9784-263d59fda33a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""b558e683-b2e7-445f-b60a-54dca4ba856d"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Movement Mode"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""471562e5-21ce-449f-ac65-4fbe9472aefb"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Destruction Mode"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""61453bd6-61ba-4219-8e67-8d7f6f134ad2"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""P Floor Mode"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a8cce685-16ac-4369-90b7-cf2686b3bb90"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""P Wall Mode"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""129a0848-3858-4ec2-b262-922789865e52"",
                    ""path"": ""<Keyboard>/5"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""P Ice Mode"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""de8d6535-7694-417c-9981-bc8d3b8e40dd"",
                    ""path"": ""<Keyboard>/6"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""P Coloured Tile Mode"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""403389f5-951c-41af-a626-8e5599581857"",
                    ""path"": ""<Keyboard>/7"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""P Trampoline Mode"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard and Mouse"",
            ""bindingGroup"": ""Keyboard and Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": true,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Main
        m_Main = asset.FindActionMap("Main", throwIfNotFound: true);
        m_Main_MovementMode = m_Main.FindAction("Movement Mode", throwIfNotFound: true);
        m_Main_DestructionMode = m_Main.FindAction("Destruction Mode", throwIfNotFound: true);
        m_Main_PFloorMode = m_Main.FindAction("P Floor Mode", throwIfNotFound: true);
        m_Main_PWallMode = m_Main.FindAction("P Wall Mode", throwIfNotFound: true);
        m_Main_PIceMode = m_Main.FindAction("P Ice Mode", throwIfNotFound: true);
        m_Main_PColouredTileMode = m_Main.FindAction("P Coloured Tile Mode", throwIfNotFound: true);
        m_Main_PTrampolineMode = m_Main.FindAction("P Trampoline Mode", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Main
    private readonly InputActionMap m_Main;
    private IMainActions m_MainActionsCallbackInterface;
    private readonly InputAction m_Main_MovementMode;
    private readonly InputAction m_Main_DestructionMode;
    private readonly InputAction m_Main_PFloorMode;
    private readonly InputAction m_Main_PWallMode;
    private readonly InputAction m_Main_PIceMode;
    private readonly InputAction m_Main_PColouredTileMode;
    private readonly InputAction m_Main_PTrampolineMode;
    public struct MainActions
    {
        private @InputMaster m_Wrapper;
        public MainActions(@InputMaster wrapper) { m_Wrapper = wrapper; }
        public InputAction @MovementMode => m_Wrapper.m_Main_MovementMode;
        public InputAction @DestructionMode => m_Wrapper.m_Main_DestructionMode;
        public InputAction @PFloorMode => m_Wrapper.m_Main_PFloorMode;
        public InputAction @PWallMode => m_Wrapper.m_Main_PWallMode;
        public InputAction @PIceMode => m_Wrapper.m_Main_PIceMode;
        public InputAction @PColouredTileMode => m_Wrapper.m_Main_PColouredTileMode;
        public InputAction @PTrampolineMode => m_Wrapper.m_Main_PTrampolineMode;
        public InputActionMap Get() { return m_Wrapper.m_Main; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MainActions set) { return set.Get(); }
        public void SetCallbacks(IMainActions instance)
        {
            if (m_Wrapper.m_MainActionsCallbackInterface != null)
            {
                @MovementMode.started -= m_Wrapper.m_MainActionsCallbackInterface.OnMovementMode;
                @MovementMode.performed -= m_Wrapper.m_MainActionsCallbackInterface.OnMovementMode;
                @MovementMode.canceled -= m_Wrapper.m_MainActionsCallbackInterface.OnMovementMode;
                @DestructionMode.started -= m_Wrapper.m_MainActionsCallbackInterface.OnDestructionMode;
                @DestructionMode.performed -= m_Wrapper.m_MainActionsCallbackInterface.OnDestructionMode;
                @DestructionMode.canceled -= m_Wrapper.m_MainActionsCallbackInterface.OnDestructionMode;
                @PFloorMode.started -= m_Wrapper.m_MainActionsCallbackInterface.OnPFloorMode;
                @PFloorMode.performed -= m_Wrapper.m_MainActionsCallbackInterface.OnPFloorMode;
                @PFloorMode.canceled -= m_Wrapper.m_MainActionsCallbackInterface.OnPFloorMode;
                @PWallMode.started -= m_Wrapper.m_MainActionsCallbackInterface.OnPWallMode;
                @PWallMode.performed -= m_Wrapper.m_MainActionsCallbackInterface.OnPWallMode;
                @PWallMode.canceled -= m_Wrapper.m_MainActionsCallbackInterface.OnPWallMode;
                @PIceMode.started -= m_Wrapper.m_MainActionsCallbackInterface.OnPIceMode;
                @PIceMode.performed -= m_Wrapper.m_MainActionsCallbackInterface.OnPIceMode;
                @PIceMode.canceled -= m_Wrapper.m_MainActionsCallbackInterface.OnPIceMode;
                @PColouredTileMode.started -= m_Wrapper.m_MainActionsCallbackInterface.OnPColouredTileMode;
                @PColouredTileMode.performed -= m_Wrapper.m_MainActionsCallbackInterface.OnPColouredTileMode;
                @PColouredTileMode.canceled -= m_Wrapper.m_MainActionsCallbackInterface.OnPColouredTileMode;
                @PTrampolineMode.started -= m_Wrapper.m_MainActionsCallbackInterface.OnPTrampolineMode;
                @PTrampolineMode.performed -= m_Wrapper.m_MainActionsCallbackInterface.OnPTrampolineMode;
                @PTrampolineMode.canceled -= m_Wrapper.m_MainActionsCallbackInterface.OnPTrampolineMode;
            }
            m_Wrapper.m_MainActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MovementMode.started += instance.OnMovementMode;
                @MovementMode.performed += instance.OnMovementMode;
                @MovementMode.canceled += instance.OnMovementMode;
                @DestructionMode.started += instance.OnDestructionMode;
                @DestructionMode.performed += instance.OnDestructionMode;
                @DestructionMode.canceled += instance.OnDestructionMode;
                @PFloorMode.started += instance.OnPFloorMode;
                @PFloorMode.performed += instance.OnPFloorMode;
                @PFloorMode.canceled += instance.OnPFloorMode;
                @PWallMode.started += instance.OnPWallMode;
                @PWallMode.performed += instance.OnPWallMode;
                @PWallMode.canceled += instance.OnPWallMode;
                @PIceMode.started += instance.OnPIceMode;
                @PIceMode.performed += instance.OnPIceMode;
                @PIceMode.canceled += instance.OnPIceMode;
                @PColouredTileMode.started += instance.OnPColouredTileMode;
                @PColouredTileMode.performed += instance.OnPColouredTileMode;
                @PColouredTileMode.canceled += instance.OnPColouredTileMode;
                @PTrampolineMode.started += instance.OnPTrampolineMode;
                @PTrampolineMode.performed += instance.OnPTrampolineMode;
                @PTrampolineMode.canceled += instance.OnPTrampolineMode;
            }
        }
    }
    public MainActions @Main => new MainActions(this);
    private int m_KeyboardandMouseSchemeIndex = -1;
    public InputControlScheme KeyboardandMouseScheme
    {
        get
        {
            if (m_KeyboardandMouseSchemeIndex == -1) m_KeyboardandMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard and Mouse");
            return asset.controlSchemes[m_KeyboardandMouseSchemeIndex];
        }
    }
    public interface IMainActions
    {
        void OnMovementMode(InputAction.CallbackContext context);
        void OnDestructionMode(InputAction.CallbackContext context);
        void OnPFloorMode(InputAction.CallbackContext context);
        void OnPWallMode(InputAction.CallbackContext context);
        void OnPIceMode(InputAction.CallbackContext context);
        void OnPColouredTileMode(InputAction.CallbackContext context);
        void OnPTrampolineMode(InputAction.CallbackContext context);
    }
}
