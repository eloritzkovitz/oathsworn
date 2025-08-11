using UnityEngine;
using StarterAssets;

public class PlayerAnimatorController : MonoBehaviour
{
    private Animator animator;
    private ThirdPersonController controller;

    private void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponentInParent<ThirdPersonController>();
    }

    private void Update()
    {
        // Movement keys
        bool isWalking = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) ||
                         Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);

        // Running
        bool isRunning = Input.GetKey(KeyCode.LeftShift) && isWalking;

        // Crouching
        bool isCrouching = Input.GetKey(KeyCode.C);

        // Grounded status from controller
        bool isGrounded = controller.Grounded;

        // Check if jumping: not grounded and not falling
        bool isJumping = !isGrounded;

        // Set animator parameters
        animator.SetBool("isWalking", isWalking && !isRunning && !isCrouching && isGrounded);
        animator.SetBool("isRunning", isRunning && !isCrouching && isGrounded);
        animator.SetBool("isCrouching", isCrouching && isGrounded);
        animator.SetBool("isJumping", isJumping);

        // Attack trigger (on left click down)
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("isAttacking");
        }
    }
}
