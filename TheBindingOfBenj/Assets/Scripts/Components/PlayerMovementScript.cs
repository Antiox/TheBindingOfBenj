using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLibrary
{
    public class PlayerMovementScript : MonoBehaviour
    {
        private Rigidbody2D _rb;
        private Animator _animator;

        [SerializeField] private float _maxVelocity;
        [SerializeField] private float _acceleration;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _animator = GetComponentInChildren<Animator>();
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
            var direction = Inputs.PlayerDirection;

            _rb.AddForce(direction * _acceleration);

            // aucun input
            if (direction.magnitude == 0)
            {
                _rb.velocity *= 0.9f;
                _animator.SetBool("IsMoving", false);
            }
            else
            {
                _animator.SetFloat("X", direction.x);
                _animator.SetFloat("Y", direction.y);
                _animator.SetBool("IsMoving", true);
            }
            
            _rb.velocity = Vector2.ClampMagnitude(_rb.velocity, _maxVelocity);
        }

        
        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.tag == Tags.DeactivatedDoor)
            {
                var room = collider.gameObject.transform.parent.parent.GetComponent<RoomScript>();
                EventManager.Instance.Dispatch(new OnPlayerRoomChanged(room));
            }
        }
    }
}
