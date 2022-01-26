using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLibrary
{
    public class PlayerMovement : MonoBehaviour
    {
        private Rigidbody2D _rb;

        [SerializeField] private float _maxVelocity;
        [SerializeField] private float _speed;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            // rotation du joueur vers la souris
            //var vectorToTarget = mousePos - transform.position;
            //float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            //angle -= 90f;
            //Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            //transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 5);
        }

        private void FixedUpdate()
        {
            _rb.AddForce(Inputs.PlayerDirection * _speed);

            if (Inputs.PlayerDirection.magnitude == 0)
                _rb.velocity *= 0.9f;

            _rb.velocity = Vector2.ClampMagnitude(_rb.velocity, _maxVelocity);
        }
    }
}
