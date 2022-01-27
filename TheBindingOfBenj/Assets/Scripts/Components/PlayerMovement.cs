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


        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.tag == "Door")
            {
                var room = collider.gameObject.transform.parent.parent.GetComponent<RoomScript>();

                // à revoir
                switch (collider.name)
                {
                    case "UW_Door":
                        transform.position = collider.transform.position + Vector3.up * 2;
                        room = room.Neighbours["Up"];
                        break;
                    case "RW_Door":
                        transform.position = collider.transform.position + Vector3.right * 2;
                        room = room.Neighbours["Right"];
                        break;
                    case "DW_Door":
                        transform.position = collider.transform.position + Vector3.down * 2;
                        room = room.Neighbours["Down"];
                        break;
                    case "LW_Door":
                        transform.position = collider.transform.position + Vector3.left * 2;
                        room = room.Neighbours["Left"];
                        break;
                    default:
                        break;
                }
                EventManager.Instance.Dispatch(new OnPlayerRoomChanged(new Vector2(room.Coordinates.x * room.Size.x, room.Coordinates.y * room.Size.y)));
            }
        }
    }
}
