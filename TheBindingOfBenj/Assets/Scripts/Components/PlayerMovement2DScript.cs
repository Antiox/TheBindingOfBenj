using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement2DScript : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    [SerializeField] private float movementSpeed = 20f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        var horizontalAxis = Input.GetAxis("Horizontal");
        var verticalAxis = Input.GetAxis("Vertical");
        var direction = new Vector2(horizontalAxis, verticalAxis);
        _rigidbody.MovePosition(_rigidbody.position + direction * Time.deltaTime * movementSpeed);
    }
}
