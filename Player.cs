using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject playerMovingParticles;

    private PlayerInput playerInput;
    private CharacterController characterController;
    private Animator animator;

    private float walkSpeed = 5f;
    private float runSpeed = 8f;

    private bool isWalking;
    private bool isRunning;

    private Vector2 move;


    private void Awake()
    {
        playerInput = new PlayerInput();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

    }

    public void HandleAnimation()
    {
        if (move != Vector2.zero)
        {
            animator.SetBool("IsWalking", true);
            if (Input.GetKey(KeyCode.Space))
            {
                animator.SetBool("IsRunning", true);
                playerMovingParticles.SetActive(true);
            }
            else
            {
                playerMovingParticles.SetActive(false);
                animator.SetBool("IsRunning", false);
            }
        }
        else
        {
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsRunning", false);
        }
    }

    private void Update()
    {
        HandleAnimation();
        MovementPlayer();
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    public void MovementPlayer()
    {
        Vector3 movement = new Vector3(move.x, 0f, move.y).normalized;

        if (movement != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, 0.15f);
        }

        float speed = animator.GetBool("IsRunning") ? runSpeed : walkSpeed;
        characterController.Move(movement * speed * Time.deltaTime);
    }

}
