//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/InputSystem/AI_Player.inputactions
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

public partial class @AI_Player: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @AI_Player()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""AI_Player"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""b3691c22-e819-4c02-b232-91024b2a838b"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""6a9cf058-e268-4b88-85c1-80f93f34ca94"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Shooting"",
                    ""type"": ""Button"",
                    ""id"": ""800a3acf-7b3e-4b03-9567-838292b7ee87"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Recharge"",
                    ""type"": ""Button"",
                    ""id"": ""4101d216-dddc-485e-b2c9-bdfb6b457f21"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Select"",
                    ""type"": ""Button"",
                    ""id"": ""b7338e34-18f8-4f6d-b7f8-5c1dc3295901"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Lantern"",
                    ""type"": ""Button"",
                    ""id"": ""f8ccda02-5945-47bf-bca0-d06921849417"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""7270903c-09f4-42a4-a03a-1046217af483"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""b48b6948-be18-48f2-a9b7-1a6a3853fde5"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""3fc9d0f1-7631-441a-90ad-1b01db62fa2d"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""d7eae353-a20c-4f13-bc19-4c23cdc50b0f"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""86f622b7-0d3e-43fa-a15d-9a76bbec7d5c"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""ARROW"",
                    ""id"": ""094e3ce6-d33c-459b-a220-b63e6699999b"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""7d4f6e54-dbb6-4e0e-9b14-958f723d027f"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""3a95c2d8-00f0-4e2a-8145-a3d754753b01"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""048e69d0-97e9-4c3b-94be-1fb7f44cdd84"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""0a8bf62b-818d-47ad-b21c-6170586c4d55"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""27cdfcbb-d223-4002-9275-920f2600bdbf"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse"",
                    ""action"": ""Shooting"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fbbfa2e6-a329-4188-883d-b0bb73db2164"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f108a534-f568-4686-b8fd-6c41fc584536"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Recharge"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e90d3a7d-70f3-4223-9aec-ac8786c5e74a"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Lantern"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Vehicle"",
            ""id"": ""b50adbf3-b04b-4753-b24b-ddd4a3baf977"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""dc8a491a-776b-4cee-94b1-63f6ece773a0"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Space"",
                    ""type"": ""Value"",
                    ""id"": ""fff2dd59-7941-448c-91b9-d28d0157a453"",
                    ""expectedControlType"": ""Double"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Throttle"",
                    ""type"": ""Value"",
                    ""id"": ""1cc01e8f-6aeb-4a80-9945-1b53f53b9611"",
                    ""expectedControlType"": ""Double"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Brake"",
                    ""type"": ""Value"",
                    ""id"": ""e773e578-4538-4392-bfcc-0c66d27a8602"",
                    ""expectedControlType"": ""Double"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Balance"",
                    ""type"": ""Value"",
                    ""id"": ""ba25906d-962b-4ff3-939b-d3e4be7f6758"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Engine"",
                    ""type"": ""Button"",
                    ""id"": ""92919e00-4842-4036-9b4a-875c40ed6146"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Shift"",
                    ""type"": ""Button"",
                    ""id"": ""87266e87-99ab-4651-8ecd-0f679b59474f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""ff891b12-a371-4dde-913c-d7b44a981ec6"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""713519d9-db80-4156-8b0c-06eccedeeed8"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""9af7edc1-44fd-409b-943a-57a9e908b213"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""efc9caa5-005e-4fd0-b160-c3c5ed1b3549"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""371ba928-811f-4178-8129-49bf3f710eb9"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""72c824db-1d3a-457d-bbfd-4309879c2fff"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Space"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7b4af69c-73ef-43d8-a6a6-c8043aae43e2"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Throttle"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b184c12f-2aef-4f64-978e-54089a62cc12"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Brake"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""85fe719f-d0ba-457a-abfc-1963e86f1906"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Balance"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""3a7b4721-bc4c-4b5c-a7ff-81a52e05d305"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Balance"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""a966a26c-a2be-4790-9341-10015d75690c"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Balance"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""e751ad59-b3d6-4204-8f13-7ac45d8031f5"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Engine"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""53d24e93-903f-484b-b42c-3cd56b4b8c65"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shift"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Time"",
            ""id"": ""50cca428-6825-4fd9-a04b-5ed504d97e01"",
            ""actions"": [
                {
                    ""name"": ""TimeDilation"",
                    ""type"": ""Button"",
                    ""id"": ""f8d2e8fe-d612-4dc3-934c-f0bc1025c769"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""acc693ba-114c-457f-88dd-49a51d706844"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse"",
                    ""action"": ""TimeDilation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Mouse"",
            ""bindingGroup"": ""Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Move = m_Player.FindAction("Move", throwIfNotFound: true);
        m_Player_Shooting = m_Player.FindAction("Shooting", throwIfNotFound: true);
        m_Player_Recharge = m_Player.FindAction("Recharge", throwIfNotFound: true);
        m_Player_Select = m_Player.FindAction("Select", throwIfNotFound: true);
        m_Player_Lantern = m_Player.FindAction("Lantern", throwIfNotFound: true);
        // Vehicle
        m_Vehicle = asset.FindActionMap("Vehicle", throwIfNotFound: true);
        m_Vehicle_Move = m_Vehicle.FindAction("Move", throwIfNotFound: true);
        m_Vehicle_Space = m_Vehicle.FindAction("Space", throwIfNotFound: true);
        m_Vehicle_Throttle = m_Vehicle.FindAction("Throttle", throwIfNotFound: true);
        m_Vehicle_Brake = m_Vehicle.FindAction("Brake", throwIfNotFound: true);
        m_Vehicle_Balance = m_Vehicle.FindAction("Balance", throwIfNotFound: true);
        m_Vehicle_Engine = m_Vehicle.FindAction("Engine", throwIfNotFound: true);
        m_Vehicle_Shift = m_Vehicle.FindAction("Shift", throwIfNotFound: true);
        // Time
        m_Time = asset.FindActionMap("Time", throwIfNotFound: true);
        m_Time_TimeDilation = m_Time.FindAction("TimeDilation", throwIfNotFound: true);
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
    private readonly InputAction m_Player_Move;
    private readonly InputAction m_Player_Shooting;
    private readonly InputAction m_Player_Recharge;
    private readonly InputAction m_Player_Select;
    private readonly InputAction m_Player_Lantern;
    public struct PlayerActions
    {
        private @AI_Player m_Wrapper;
        public PlayerActions(@AI_Player wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Player_Move;
        public InputAction @Shooting => m_Wrapper.m_Player_Shooting;
        public InputAction @Recharge => m_Wrapper.m_Player_Recharge;
        public InputAction @Select => m_Wrapper.m_Player_Select;
        public InputAction @Lantern => m_Wrapper.m_Player_Lantern;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Add(instance);
            @Move.started += instance.OnMove;
            @Move.performed += instance.OnMove;
            @Move.canceled += instance.OnMove;
            @Shooting.started += instance.OnShooting;
            @Shooting.performed += instance.OnShooting;
            @Shooting.canceled += instance.OnShooting;
            @Recharge.started += instance.OnRecharge;
            @Recharge.performed += instance.OnRecharge;
            @Recharge.canceled += instance.OnRecharge;
            @Select.started += instance.OnSelect;
            @Select.performed += instance.OnSelect;
            @Select.canceled += instance.OnSelect;
            @Lantern.started += instance.OnLantern;
            @Lantern.performed += instance.OnLantern;
            @Lantern.canceled += instance.OnLantern;
        }

        private void UnregisterCallbacks(IPlayerActions instance)
        {
            @Move.started -= instance.OnMove;
            @Move.performed -= instance.OnMove;
            @Move.canceled -= instance.OnMove;
            @Shooting.started -= instance.OnShooting;
            @Shooting.performed -= instance.OnShooting;
            @Shooting.canceled -= instance.OnShooting;
            @Recharge.started -= instance.OnRecharge;
            @Recharge.performed -= instance.OnRecharge;
            @Recharge.canceled -= instance.OnRecharge;
            @Select.started -= instance.OnSelect;
            @Select.performed -= instance.OnSelect;
            @Select.canceled -= instance.OnSelect;
            @Lantern.started -= instance.OnLantern;
            @Lantern.performed -= instance.OnLantern;
            @Lantern.canceled -= instance.OnLantern;
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

    // Vehicle
    private readonly InputActionMap m_Vehicle;
    private List<IVehicleActions> m_VehicleActionsCallbackInterfaces = new List<IVehicleActions>();
    private readonly InputAction m_Vehicle_Move;
    private readonly InputAction m_Vehicle_Space;
    private readonly InputAction m_Vehicle_Throttle;
    private readonly InputAction m_Vehicle_Brake;
    private readonly InputAction m_Vehicle_Balance;
    private readonly InputAction m_Vehicle_Engine;
    private readonly InputAction m_Vehicle_Shift;
    public struct VehicleActions
    {
        private @AI_Player m_Wrapper;
        public VehicleActions(@AI_Player wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Vehicle_Move;
        public InputAction @Space => m_Wrapper.m_Vehicle_Space;
        public InputAction @Throttle => m_Wrapper.m_Vehicle_Throttle;
        public InputAction @Brake => m_Wrapper.m_Vehicle_Brake;
        public InputAction @Balance => m_Wrapper.m_Vehicle_Balance;
        public InputAction @Engine => m_Wrapper.m_Vehicle_Engine;
        public InputAction @Shift => m_Wrapper.m_Vehicle_Shift;
        public InputActionMap Get() { return m_Wrapper.m_Vehicle; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(VehicleActions set) { return set.Get(); }
        public void AddCallbacks(IVehicleActions instance)
        {
            if (instance == null || m_Wrapper.m_VehicleActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_VehicleActionsCallbackInterfaces.Add(instance);
            @Move.started += instance.OnMove;
            @Move.performed += instance.OnMove;
            @Move.canceled += instance.OnMove;
            @Space.started += instance.OnSpace;
            @Space.performed += instance.OnSpace;
            @Space.canceled += instance.OnSpace;
            @Throttle.started += instance.OnThrottle;
            @Throttle.performed += instance.OnThrottle;
            @Throttle.canceled += instance.OnThrottle;
            @Brake.started += instance.OnBrake;
            @Brake.performed += instance.OnBrake;
            @Brake.canceled += instance.OnBrake;
            @Balance.started += instance.OnBalance;
            @Balance.performed += instance.OnBalance;
            @Balance.canceled += instance.OnBalance;
            @Engine.started += instance.OnEngine;
            @Engine.performed += instance.OnEngine;
            @Engine.canceled += instance.OnEngine;
            @Shift.started += instance.OnShift;
            @Shift.performed += instance.OnShift;
            @Shift.canceled += instance.OnShift;
        }

        private void UnregisterCallbacks(IVehicleActions instance)
        {
            @Move.started -= instance.OnMove;
            @Move.performed -= instance.OnMove;
            @Move.canceled -= instance.OnMove;
            @Space.started -= instance.OnSpace;
            @Space.performed -= instance.OnSpace;
            @Space.canceled -= instance.OnSpace;
            @Throttle.started -= instance.OnThrottle;
            @Throttle.performed -= instance.OnThrottle;
            @Throttle.canceled -= instance.OnThrottle;
            @Brake.started -= instance.OnBrake;
            @Brake.performed -= instance.OnBrake;
            @Brake.canceled -= instance.OnBrake;
            @Balance.started -= instance.OnBalance;
            @Balance.performed -= instance.OnBalance;
            @Balance.canceled -= instance.OnBalance;
            @Engine.started -= instance.OnEngine;
            @Engine.performed -= instance.OnEngine;
            @Engine.canceled -= instance.OnEngine;
            @Shift.started -= instance.OnShift;
            @Shift.performed -= instance.OnShift;
            @Shift.canceled -= instance.OnShift;
        }

        public void RemoveCallbacks(IVehicleActions instance)
        {
            if (m_Wrapper.m_VehicleActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IVehicleActions instance)
        {
            foreach (var item in m_Wrapper.m_VehicleActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_VehicleActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public VehicleActions @Vehicle => new VehicleActions(this);

    // Time
    private readonly InputActionMap m_Time;
    private List<ITimeActions> m_TimeActionsCallbackInterfaces = new List<ITimeActions>();
    private readonly InputAction m_Time_TimeDilation;
    public struct TimeActions
    {
        private @AI_Player m_Wrapper;
        public TimeActions(@AI_Player wrapper) { m_Wrapper = wrapper; }
        public InputAction @TimeDilation => m_Wrapper.m_Time_TimeDilation;
        public InputActionMap Get() { return m_Wrapper.m_Time; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(TimeActions set) { return set.Get(); }
        public void AddCallbacks(ITimeActions instance)
        {
            if (instance == null || m_Wrapper.m_TimeActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_TimeActionsCallbackInterfaces.Add(instance);
            @TimeDilation.started += instance.OnTimeDilation;
            @TimeDilation.performed += instance.OnTimeDilation;
            @TimeDilation.canceled += instance.OnTimeDilation;
        }

        private void UnregisterCallbacks(ITimeActions instance)
        {
            @TimeDilation.started -= instance.OnTimeDilation;
            @TimeDilation.performed -= instance.OnTimeDilation;
            @TimeDilation.canceled -= instance.OnTimeDilation;
        }

        public void RemoveCallbacks(ITimeActions instance)
        {
            if (m_Wrapper.m_TimeActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(ITimeActions instance)
        {
            foreach (var item in m_Wrapper.m_TimeActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_TimeActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public TimeActions @Time => new TimeActions(this);
    private int m_KeyboardSchemeIndex = -1;
    public InputControlScheme KeyboardScheme
    {
        get
        {
            if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
            return asset.controlSchemes[m_KeyboardSchemeIndex];
        }
    }
    private int m_MouseSchemeIndex = -1;
    public InputControlScheme MouseScheme
    {
        get
        {
            if (m_MouseSchemeIndex == -1) m_MouseSchemeIndex = asset.FindControlSchemeIndex("Mouse");
            return asset.controlSchemes[m_MouseSchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnShooting(InputAction.CallbackContext context);
        void OnRecharge(InputAction.CallbackContext context);
        void OnSelect(InputAction.CallbackContext context);
        void OnLantern(InputAction.CallbackContext context);
    }
    public interface IVehicleActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnSpace(InputAction.CallbackContext context);
        void OnThrottle(InputAction.CallbackContext context);
        void OnBrake(InputAction.CallbackContext context);
        void OnBalance(InputAction.CallbackContext context);
        void OnEngine(InputAction.CallbackContext context);
        void OnShift(InputAction.CallbackContext context);
    }
    public interface ITimeActions
    {
        void OnTimeDilation(InputAction.CallbackContext context);
    }
}
