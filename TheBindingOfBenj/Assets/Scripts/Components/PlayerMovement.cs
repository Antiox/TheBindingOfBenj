using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLibrary
{
    public class PlayerMovement : MonoBehaviour
    {
        private InputManager _inputManager;
        private Rigidbody2D _rb;

        [SerializeField] private float _maxVelocity;
        [SerializeField] private float _speed;

        private void Awake()
        {
            _inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
            _rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            var movement = _inputManager.move;

            _rb.AddForce(movement * _speed);

            if (movement.magnitude == 0)
                _rb.velocity *= 0.9f;

            _rb.velocity = Vector2.ClampMagnitude(_rb.velocity, _maxVelocity);
        }

    }
}
