using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController Controller;

    [SerializeField]
    public float MovementSpeed = 2f;
    public float Speed = 12f;
    public float Gravity = -9.81f;
    public float JumpHeight = 3f;

    public Transform GroundCheck;
    public float GroundDistance = 4f;
    public LayerMask GroundMask;

    private Vector3 _velocity;

    void Start()
    {
        
    }

    void Update()
    {
        return;
        bool isGrounded = Physics.CheckSphere(GroundCheck.position, GroundDistance, GroundMask);

        if (isGrounded && _velocity.y < 0)
        {
            _velocity.y = -MovementSpeed;
        }

        var strafeMovement = Input.GetAxis("Horizontal");
        var forwardMovement = Input.GetAxis("Vertical");

        Vector3 move = transform.right * strafeMovement + transform.forward * forwardMovement;

        Controller.Move(move * Speed * Time.deltaTime);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            _velocity.y = Mathf.Sqrt(JumpHeight * -MovementSpeed * Gravity);
        }

        _velocity.y += Gravity * Time.deltaTime;

        Controller.Move(_velocity * Time.deltaTime);
    }
}
