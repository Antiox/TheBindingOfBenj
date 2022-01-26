using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace GameLibrary
{
    public class InputManagerScript : MonoBehaviour
    {
        public Vector2 Move { get; private set; }
        public Vector2 MousePosition { get; private set; }
        public bool Attack { get; private set; }

        private PlayerInput playerInput;
        private InputAction moveIA, mouseMoveIA, attackIA;

        void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
            moveIA = playerInput.actions["Move"];
            mouseMoveIA = playerInput.actions["MouseMove"];
            attackIA = playerInput.actions["Attack"];
        }

        private void OnEnable()
        {
            moveIA.performed += InputReceived;
            mouseMoveIA.performed += InputReceived;
            attackIA.performed += InputReceived;

            moveIA.Enable();
            mouseMoveIA.Enable();
            attackIA.Enable();
        }

        private void InputReceived(CallbackContext e)
        {
            switch (e.action.name)
            {
                case "MouseMove": EventManager.Instance.Dispatch(new OnMouseMoved(e.ReadValue<Vector2>())); break;
                case "Move": EventManager.Instance.Dispatch(new OnPlayerMoved(e.ReadValue<Vector2>())); break;
                case "Attack": EventManager.Instance.Dispatch(new OnPlayerAttacked(e.ReadValueAsButton())); break;
            }
        }

        private void OnDisable()
        {
            moveIA.Disable();
            mouseMoveIA.Disable();
            attackIA.Disable();
        }
    }
}
