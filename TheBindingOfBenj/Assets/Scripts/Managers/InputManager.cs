using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameLibrary
{
    public class InputManager : MonoBehaviour
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
            moveIA.performed += value => Move = value.ReadValue<Vector2>();
            moveIA.Enable();

            mouseMoveIA.performed += value => MousePosition = value.ReadValue<Vector2>();
            mouseMoveIA.Enable();

            attackIA.performed += value => Attack = value.ReadValueAsButton();
            attackIA.Enable();
        }

        private void OnDisable()
        {
            moveIA.Disable();
            mouseMoveIA.Disable();
            attackIA.Disable();
        }

    }
}
