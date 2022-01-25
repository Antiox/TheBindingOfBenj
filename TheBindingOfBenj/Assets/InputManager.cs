using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameLibrary
{
    public class InputManager : MonoBehaviour
    {
        public Vector2 move { get; private set; }

        public Vector2 mouseDelta { get; private set; }

        public bool attack { get; private set; }

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
            moveIA.performed += Move => move = Move.ReadValue<Vector2>();
            moveIA.Enable();

            mouseMoveIA.performed += MoveCamera => mouseDelta = MoveCamera.ReadValue<Vector2>();
            mouseMoveIA.Enable();

            attackIA.performed += Attack => attack = Attack.ReadValueAsButton();
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
