// GENERATED AUTOMATICALLY FROM 'Assets/InputManager/InputManager.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InputManager : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputManager()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputManager"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""89efd78b-de18-487e-8b94-df9ee805d946"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""8e955b84-29bc-41b6-a840-04fe1f2dfcac"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""64aaab3b-98b7-4418-a29f-62f74e646841"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Left Trigger Press"",
                    ""type"": ""Button"",
                    ""id"": ""dc4b88d3-c775-4412-9606-127726703849"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Left Trigger Move"",
                    ""type"": ""Value"",
                    ""id"": ""e0f66b9f-8be4-4b86-bf36-52dab95e74da"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Left Trigger Touch"",
                    ""type"": ""Value"",
                    ""id"": ""69a6cedb-6999-4385-9072-f7001cae9fc0"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Left Grip Press"",
                    ""type"": ""Button"",
                    ""id"": ""2c08fd0f-7b06-4887-9e6d-974124658542"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Left Grip Move"",
                    ""type"": ""Value"",
                    ""id"": ""29c9ee99-a92c-4c7c-a37d-0bac799b5028"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Left Thumb Touch"",
                    ""type"": ""Value"",
                    ""id"": ""34f748e2-b1ae-4860-912d-b4050a556211"",
                    ""expectedControlType"": ""Analog"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Right Trigger Press"",
                    ""type"": ""Button"",
                    ""id"": ""7d3b48d5-4756-4f55-9bc4-4d1c944ec441"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Right Trigger Move"",
                    ""type"": ""Value"",
                    ""id"": ""53c5c11a-3c3b-4cb5-abde-ee37383c2665"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Right Trigger Touch"",
                    ""type"": ""Value"",
                    ""id"": ""1dc88e76-190d-4fe0-8128-2c87e3d1163d"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Right Grip Press"",
                    ""type"": ""Button"",
                    ""id"": ""6268f116-9962-4389-af32-ba57f6ae93d2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Right Grip Move"",
                    ""type"": ""Value"",
                    ""id"": ""225617dd-2eaa-4b67-a964-d2059a3892e8"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Right Thumb Touch"",
                    ""type"": ""Value"",
                    ""id"": ""05e96109-9078-4e75-9871-b357bde91a74"",
                    ""expectedControlType"": ""Analog"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Keyboard (Test)"",
                    ""id"": ""c7682a11-5c42-4bc5-a50a-9cdead1a45d5"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""fe536c2d-0cb9-4171-820b-ab862861459e"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""3051cfea-1536-4fe4-9359-5f13de0b77dd"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""4fe25273-1dfc-4093-ac23-9a4c3acb7169"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""307ba5db-ec40-4a95-8f6f-bcfaad2e86ac"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""1bd2ecbf-467b-492e-aa48-45d0c6599579"",
                    ""path"": ""<XRController>{LeftHand}/thumbstick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Keyboard (Test)"",
                    ""id"": ""372f43e7-92d5-4f88-8f57-1818f497c9a5"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""ced740c5-1867-4241-a594-eef0f4cc9eca"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""07ffc25a-f224-47f3-b5b5-86db10ed6db6"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""4b0843ec-73ef-492d-ba67-7c37e39f8b7e"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""56b2d9ca-30fa-448a-926a-d41842fe1ff4"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""d2fb3172-9caa-4b87-8ed8-71cf37ea13e7"",
                    ""path"": ""<XRController>{RightHand}/thumbstick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""222b481e-7ec2-4de7-9267-620750733f17"",
                    ""path"": ""<XRController>{LeftHand}/triggerPressed"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Left Trigger Press"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7c4ea827-2dd4-4e4b-aa01-79c9358ea00b"",
                    ""path"": ""<XRController>{LeftHand}/trigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Left Trigger Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""18a23ca0-3200-4767-bf48-b48c43f44fa5"",
                    ""path"": ""<XRController>{LeftHand}/gripPressed"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Left Grip Press"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bdfea179-73e4-4c60-a0de-acaa87e306e5"",
                    ""path"": ""<XRController>{LeftHand}/grip"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Left Grip Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bebb9177-f0af-4051-ae6d-80b5c1d6eda3"",
                    ""path"": ""<XRController>{RightHand}/triggerPressed"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Right Trigger Press"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9363e127-1c39-41a7-abc0-3c195d3e38e7"",
                    ""path"": ""<XRController>{RightHand}/trigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Right Trigger Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1f28dbfc-397f-4176-8011-643dad4daebe"",
                    ""path"": ""<XRController>{RightHand}/grip"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Right Grip Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""db25d6c2-bee7-4448-981b-6db762b9e8e4"",
                    ""path"": ""<XRController>{LeftHand}/triggerTouched"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Left Trigger Touch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c93f14d8-359f-4876-9524-edcaa3abfdb2"",
                    ""path"": ""<XRController>{RightHand}/gripPressed"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Right Grip Press"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ce797192-464b-4303-99fa-f2e5cbec58ab"",
                    ""path"": ""<XRController>{RightHand}/triggerTouched"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Right Trigger Touch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ef638081-fc39-4c83-8b23-b041669f7459"",
                    ""path"": ""<XRInputV1::Oculus::OculusTouchControllerRight>{RightHand}/thumbtouch"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Right Thumb Touch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""506aa0df-4ddb-46ae-a706-126f391485c3"",
                    ""path"": ""<XRInputV1::Oculus::OculusTouchControllerLeft>{LeftHand}/thumbtouch"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Left Thumb Touch"",
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
        m_Player_Movement = m_Player.FindAction("Movement", throwIfNotFound: true);
        m_Player_Look = m_Player.FindAction("Look", throwIfNotFound: true);
        m_Player_LeftTriggerPress = m_Player.FindAction("Left Trigger Press", throwIfNotFound: true);
        m_Player_LeftTriggerMove = m_Player.FindAction("Left Trigger Move", throwIfNotFound: true);
        m_Player_LeftTriggerTouch = m_Player.FindAction("Left Trigger Touch", throwIfNotFound: true);
        m_Player_LeftGripPress = m_Player.FindAction("Left Grip Press", throwIfNotFound: true);
        m_Player_LeftGripMove = m_Player.FindAction("Left Grip Move", throwIfNotFound: true);
        m_Player_LeftThumbTouch = m_Player.FindAction("Left Thumb Touch", throwIfNotFound: true);
        m_Player_RightTriggerPress = m_Player.FindAction("Right Trigger Press", throwIfNotFound: true);
        m_Player_RightTriggerMove = m_Player.FindAction("Right Trigger Move", throwIfNotFound: true);
        m_Player_RightTriggerTouch = m_Player.FindAction("Right Trigger Touch", throwIfNotFound: true);
        m_Player_RightGripPress = m_Player.FindAction("Right Grip Press", throwIfNotFound: true);
        m_Player_RightGripMove = m_Player.FindAction("Right Grip Move", throwIfNotFound: true);
        m_Player_RightThumbTouch = m_Player.FindAction("Right Thumb Touch", throwIfNotFound: true);
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

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_Movement;
    private readonly InputAction m_Player_Look;
    private readonly InputAction m_Player_LeftTriggerPress;
    private readonly InputAction m_Player_LeftTriggerMove;
    private readonly InputAction m_Player_LeftTriggerTouch;
    private readonly InputAction m_Player_LeftGripPress;
    private readonly InputAction m_Player_LeftGripMove;
    private readonly InputAction m_Player_LeftThumbTouch;
    private readonly InputAction m_Player_RightTriggerPress;
    private readonly InputAction m_Player_RightTriggerMove;
    private readonly InputAction m_Player_RightTriggerTouch;
    private readonly InputAction m_Player_RightGripPress;
    private readonly InputAction m_Player_RightGripMove;
    private readonly InputAction m_Player_RightThumbTouch;
    public struct PlayerActions
    {
        private @InputManager m_Wrapper;
        public PlayerActions(@InputManager wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_Player_Movement;
        public InputAction @Look => m_Wrapper.m_Player_Look;
        public InputAction @LeftTriggerPress => m_Wrapper.m_Player_LeftTriggerPress;
        public InputAction @LeftTriggerMove => m_Wrapper.m_Player_LeftTriggerMove;
        public InputAction @LeftTriggerTouch => m_Wrapper.m_Player_LeftTriggerTouch;
        public InputAction @LeftGripPress => m_Wrapper.m_Player_LeftGripPress;
        public InputAction @LeftGripMove => m_Wrapper.m_Player_LeftGripMove;
        public InputAction @LeftThumbTouch => m_Wrapper.m_Player_LeftThumbTouch;
        public InputAction @RightTriggerPress => m_Wrapper.m_Player_RightTriggerPress;
        public InputAction @RightTriggerMove => m_Wrapper.m_Player_RightTriggerMove;
        public InputAction @RightTriggerTouch => m_Wrapper.m_Player_RightTriggerTouch;
        public InputAction @RightGripPress => m_Wrapper.m_Player_RightGripPress;
        public InputAction @RightGripMove => m_Wrapper.m_Player_RightGripMove;
        public InputAction @RightThumbTouch => m_Wrapper.m_Player_RightThumbTouch;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                @Look.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLook;
                @Look.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLook;
                @Look.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLook;
                @LeftTriggerPress.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLeftTriggerPress;
                @LeftTriggerPress.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLeftTriggerPress;
                @LeftTriggerPress.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLeftTriggerPress;
                @LeftTriggerMove.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLeftTriggerMove;
                @LeftTriggerMove.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLeftTriggerMove;
                @LeftTriggerMove.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLeftTriggerMove;
                @LeftTriggerTouch.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLeftTriggerTouch;
                @LeftTriggerTouch.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLeftTriggerTouch;
                @LeftTriggerTouch.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLeftTriggerTouch;
                @LeftGripPress.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLeftGripPress;
                @LeftGripPress.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLeftGripPress;
                @LeftGripPress.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLeftGripPress;
                @LeftGripMove.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLeftGripMove;
                @LeftGripMove.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLeftGripMove;
                @LeftGripMove.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLeftGripMove;
                @LeftThumbTouch.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLeftThumbTouch;
                @LeftThumbTouch.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLeftThumbTouch;
                @LeftThumbTouch.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLeftThumbTouch;
                @RightTriggerPress.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRightTriggerPress;
                @RightTriggerPress.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRightTriggerPress;
                @RightTriggerPress.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRightTriggerPress;
                @RightTriggerMove.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRightTriggerMove;
                @RightTriggerMove.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRightTriggerMove;
                @RightTriggerMove.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRightTriggerMove;
                @RightTriggerTouch.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRightTriggerTouch;
                @RightTriggerTouch.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRightTriggerTouch;
                @RightTriggerTouch.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRightTriggerTouch;
                @RightGripPress.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRightGripPress;
                @RightGripPress.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRightGripPress;
                @RightGripPress.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRightGripPress;
                @RightGripMove.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRightGripMove;
                @RightGripMove.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRightGripMove;
                @RightGripMove.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRightGripMove;
                @RightThumbTouch.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRightThumbTouch;
                @RightThumbTouch.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRightThumbTouch;
                @RightThumbTouch.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRightThumbTouch;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Look.started += instance.OnLook;
                @Look.performed += instance.OnLook;
                @Look.canceled += instance.OnLook;
                @LeftTriggerPress.started += instance.OnLeftTriggerPress;
                @LeftTriggerPress.performed += instance.OnLeftTriggerPress;
                @LeftTriggerPress.canceled += instance.OnLeftTriggerPress;
                @LeftTriggerMove.started += instance.OnLeftTriggerMove;
                @LeftTriggerMove.performed += instance.OnLeftTriggerMove;
                @LeftTriggerMove.canceled += instance.OnLeftTriggerMove;
                @LeftTriggerTouch.started += instance.OnLeftTriggerTouch;
                @LeftTriggerTouch.performed += instance.OnLeftTriggerTouch;
                @LeftTriggerTouch.canceled += instance.OnLeftTriggerTouch;
                @LeftGripPress.started += instance.OnLeftGripPress;
                @LeftGripPress.performed += instance.OnLeftGripPress;
                @LeftGripPress.canceled += instance.OnLeftGripPress;
                @LeftGripMove.started += instance.OnLeftGripMove;
                @LeftGripMove.performed += instance.OnLeftGripMove;
                @LeftGripMove.canceled += instance.OnLeftGripMove;
                @LeftThumbTouch.started += instance.OnLeftThumbTouch;
                @LeftThumbTouch.performed += instance.OnLeftThumbTouch;
                @LeftThumbTouch.canceled += instance.OnLeftThumbTouch;
                @RightTriggerPress.started += instance.OnRightTriggerPress;
                @RightTriggerPress.performed += instance.OnRightTriggerPress;
                @RightTriggerPress.canceled += instance.OnRightTriggerPress;
                @RightTriggerMove.started += instance.OnRightTriggerMove;
                @RightTriggerMove.performed += instance.OnRightTriggerMove;
                @RightTriggerMove.canceled += instance.OnRightTriggerMove;
                @RightTriggerTouch.started += instance.OnRightTriggerTouch;
                @RightTriggerTouch.performed += instance.OnRightTriggerTouch;
                @RightTriggerTouch.canceled += instance.OnRightTriggerTouch;
                @RightGripPress.started += instance.OnRightGripPress;
                @RightGripPress.performed += instance.OnRightGripPress;
                @RightGripPress.canceled += instance.OnRightGripPress;
                @RightGripMove.started += instance.OnRightGripMove;
                @RightGripMove.performed += instance.OnRightGripMove;
                @RightGripMove.canceled += instance.OnRightGripMove;
                @RightThumbTouch.started += instance.OnRightThumbTouch;
                @RightThumbTouch.performed += instance.OnRightThumbTouch;
                @RightThumbTouch.canceled += instance.OnRightThumbTouch;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    public interface IPlayerActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnLeftTriggerPress(InputAction.CallbackContext context);
        void OnLeftTriggerMove(InputAction.CallbackContext context);
        void OnLeftTriggerTouch(InputAction.CallbackContext context);
        void OnLeftGripPress(InputAction.CallbackContext context);
        void OnLeftGripMove(InputAction.CallbackContext context);
        void OnLeftThumbTouch(InputAction.CallbackContext context);
        void OnRightTriggerPress(InputAction.CallbackContext context);
        void OnRightTriggerMove(InputAction.CallbackContext context);
        void OnRightTriggerTouch(InputAction.CallbackContext context);
        void OnRightGripPress(InputAction.CallbackContext context);
        void OnRightGripMove(InputAction.CallbackContext context);
        void OnRightThumbTouch(InputAction.CallbackContext context);
    }
}
