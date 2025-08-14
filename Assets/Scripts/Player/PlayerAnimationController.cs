using UnityEngine;
using UnityEngine.SceneManagement;
using StarterAssets;

public class PlayerAnimatorController : MonoBehaviour
{
    private Animator animator;
    private ThirdPersonController controller;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        controller = GetComponentInParent<ThirdPersonController>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Set animation path based on scene
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Set animation path according to scene
        if (scene.name == "Scene2")
        {
            animator.SetBool("isWanted", true);
            Debug.Log("You are now wanted!");
        }
        else
        {
            animator.SetBool("isWanted", false);
        }
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

        // Jumping
        bool isGrounded = controller.Grounded;
        bool isJumping = Input.GetKey(KeyCode.Space) && controller.Grounded;
        animator.SetBool("Jump", isJumping);     

        // Set animator parameters
        animator.SetBool("isWalking", isWalking && !isRunning && !isCrouching && isGrounded);
        animator.SetBool("isRunning", isRunning && !isCrouching && isGrounded);
        animator.SetBool("isCrouching", isCrouching && isGrounded);

        // Attack trigger (on left click down)
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("isAttacking");
        }
    }
}
