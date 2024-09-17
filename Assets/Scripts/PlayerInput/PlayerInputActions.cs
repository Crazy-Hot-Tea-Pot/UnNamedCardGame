//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Scripts/PlayerInput/PlayerInputActions.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerInputActions: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputActions"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""9783f88b-59c5-48cb-9c32-a2e8c1c6bb1f"",
            ""actions"": [
                {
                    ""name"": ""Select"",
                    ""type"": ""Button"",
                    ""id"": ""2b4b888f-8225-43ef-8f69-9ce35a4ac3f9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""c39f1c30-cbf0-41cf-bd32-81596613ae1a"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Camera Controls"",
            ""id"": ""80c3423b-559a-42a2-850b-dff08de73c29"",
            ""actions"": [
                {
                    ""name"": ""RotateCamera"",
                    ""type"": ""Button"",
                    ""id"": ""49337c1c-dbc0-4816-ac85-80052830c030"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""e6939c3a-9d94-4eef-9673-5de7a224aba1"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Zoom"",
                    ""type"": ""Value"",
                    ""id"": ""14e449f6-402b-4f30-a20e-4cceed1db8c8"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Pan"",
                    ""type"": ""PassThrough"",
                    ""id"": ""bedce9e2-d8ed-4b64-9491-d363640e241d"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""PanKeys"",
                    ""type"": ""Value"",
                    ""id"": ""19766862-45ed-4372-ad0c-6d85417f56e6"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""be8864b5-afc1-4e02-a9f3-abf16fa71c2e"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": ""Hold"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RotateCamera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c69e392e-99db-41f4-aee8-e6ef8cda9bb8"",
                    ""path"": ""<Pointer>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d56fb80e-92f2-48e4-97a3-14d64af110c9"",
                    ""path"": ""<Mouse>/scroll/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Zoom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""One Modifier"",
                    ""id"": ""3c3fdf09-ee77-4140-ba3b-9e065b09af9e"",
                    ""path"": ""OneModifier"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pan"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""modifier"",
                    ""id"": ""87a43d0e-33a3-42c6-be5e-00cff0574c87"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pan"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""binding"",
                    ""id"": ""0aa7f528-582a-4128-af4f-6e3aa52e0861"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pan"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""cf89e576-8f70-4cf5-b435-aeb893c2135b"",
                    ""path"": ""Keyboard -> ArrowUp,ArrowDown,ArrowLeft,ArrowRight"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PanKeys"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""41e1e649-f7bb-4c05-b748-08af83e96ee9"",
                    ""path"": ""Keyboard -> W,S,A,D"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PanKeys"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Select = m_Player.FindAction("Select", throwIfNotFound: true);
        // Camera Controls
        m_CameraControls = asset.FindActionMap("Camera Controls", throwIfNotFound: true);
        m_CameraControls_RotateCamera = m_CameraControls.FindAction("RotateCamera", throwIfNotFound: true);
        m_CameraControls_Look = m_CameraControls.FindAction("Look", throwIfNotFound: true);
        m_CameraControls_Zoom = m_CameraControls.FindAction("Zoom", throwIfNotFound: true);
        m_CameraControls_Pan = m_CameraControls.FindAction("Pan", throwIfNotFound: true);
        m_CameraControls_PanKeys = m_CameraControls.FindAction("PanKeys", throwIfNotFound: true);
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

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Player
    private readonly InputActionMap m_Player;
    private List<IPlayerActions> m_PlayerActionsCallbackInterfaces = new List<IPlayerActions>();
    private readonly InputAction m_Player_Select;
    public struct PlayerActions
    {
        private @PlayerInputActions m_Wrapper;
        public PlayerActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Select => m_Wrapper.m_Player_Select;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Add(instance);
            @Select.started += instance.OnSelect;
            @Select.performed += instance.OnSelect;
            @Select.canceled += instance.OnSelect;
        }

        private void UnregisterCallbacks(IPlayerActions instance)
        {
            @Select.started -= instance.OnSelect;
            @Select.performed -= instance.OnSelect;
            @Select.canceled -= instance.OnSelect;
        }

        public void RemoveCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerActions @Player => new PlayerActions(this);

    // Camera Controls
    private readonly InputActionMap m_CameraControls;
    private List<ICameraControlsActions> m_CameraControlsActionsCallbackInterfaces = new List<ICameraControlsActions>();
    private readonly InputAction m_CameraControls_RotateCamera;
    private readonly InputAction m_CameraControls_Look;
    private readonly InputAction m_CameraControls_Zoom;
    private readonly InputAction m_CameraControls_Pan;
    private readonly InputAction m_CameraControls_PanKeys;
    public struct CameraControlsActions
    {
        private @PlayerInputActions m_Wrapper;
        public CameraControlsActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @RotateCamera => m_Wrapper.m_CameraControls_RotateCamera;
        public InputAction @Look => m_Wrapper.m_CameraControls_Look;
        public InputAction @Zoom => m_Wrapper.m_CameraControls_Zoom;
        public InputAction @Pan => m_Wrapper.m_CameraControls_Pan;
        public InputAction @PanKeys => m_Wrapper.m_CameraControls_PanKeys;
        public InputActionMap Get() { return m_Wrapper.m_CameraControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CameraControlsActions set) { return set.Get(); }
        public void AddCallbacks(ICameraControlsActions instance)
        {
            if (instance == null || m_Wrapper.m_CameraControlsActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_CameraControlsActionsCallbackInterfaces.Add(instance);
            @RotateCamera.started += instance.OnRotateCamera;
            @RotateCamera.performed += instance.OnRotateCamera;
            @RotateCamera.canceled += instance.OnRotateCamera;
            @Look.started += instance.OnLook;
            @Look.performed += instance.OnLook;
            @Look.canceled += instance.OnLook;
            @Zoom.started += instance.OnZoom;
            @Zoom.performed += instance.OnZoom;
            @Zoom.canceled += instance.OnZoom;
            @Pan.started += instance.OnPan;
            @Pan.performed += instance.OnPan;
            @Pan.canceled += instance.OnPan;
            @PanKeys.started += instance.OnPanKeys;
            @PanKeys.performed += instance.OnPanKeys;
            @PanKeys.canceled += instance.OnPanKeys;
        }

        private void UnregisterCallbacks(ICameraControlsActions instance)
        {
            @RotateCamera.started -= instance.OnRotateCamera;
            @RotateCamera.performed -= instance.OnRotateCamera;
            @RotateCamera.canceled -= instance.OnRotateCamera;
            @Look.started -= instance.OnLook;
            @Look.performed -= instance.OnLook;
            @Look.canceled -= instance.OnLook;
            @Zoom.started -= instance.OnZoom;
            @Zoom.performed -= instance.OnZoom;
            @Zoom.canceled -= instance.OnZoom;
            @Pan.started -= instance.OnPan;
            @Pan.performed -= instance.OnPan;
            @Pan.canceled -= instance.OnPan;
            @PanKeys.started -= instance.OnPanKeys;
            @PanKeys.performed -= instance.OnPanKeys;
            @PanKeys.canceled -= instance.OnPanKeys;
        }

        public void RemoveCallbacks(ICameraControlsActions instance)
        {
            if (m_Wrapper.m_CameraControlsActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(ICameraControlsActions instance)
        {
            foreach (var item in m_Wrapper.m_CameraControlsActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_CameraControlsActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public CameraControlsActions @CameraControls => new CameraControlsActions(this);
    public interface IPlayerActions
    {
        void OnSelect(InputAction.CallbackContext context);
    }
    public interface ICameraControlsActions
    {
        void OnRotateCamera(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnZoom(InputAction.CallbackContext context);
        void OnPan(InputAction.CallbackContext context);
        void OnPanKeys(InputAction.CallbackContext context);
    }
}
