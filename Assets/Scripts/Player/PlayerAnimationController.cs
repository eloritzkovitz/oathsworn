using UnityEngine;
using UnityEngine.SceneManagement;
using StarterAssets;

public class PlayerAnimatorController : MonoBehaviour
{
    private Animator animator;
    private ThirdPersonController controller;

    private AudioSource audioSource;
    private AudioClip footstepsSound;
    private AudioClip jumpSound;

    private bool wasJumping = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        controller = GetComponentInParent<ThirdPersonController>();
        SceneManager.sceneLoaded += OnSceneLoaded;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        // Load player sounds from Resources (no extension)
        footstepsSound = Resources.Load<AudioClip>("Audio/SFX/footsteps");
        jumpSound = Resources.Load<AudioClip>("Audio/SFX/gasp");
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Set animation path based on scene
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Scene2")
        {
            animator.SetFloat("IsWanted", scene.name == "Scene2" ? 1f : 0f);
            Debug.Log("You are now wanted!");
        }
        else
        {
            animator.SetFloat("IsWanted", 0f);
        }
    }

    private void Update()
    {
        // Movement keys
        bool isWalking = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) ||
                         Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);

        bool isRunning = Input.GetKey(KeyCode.LeftShift) && isWalking;
        bool isCrouching = Input.GetKey(KeyCode.C);
        bool isGrounded = controller.Grounded;

        // Calculate movement speed for blend tree
        float moveSpeed = 0f;
        if (isWalking && isGrounded)
            moveSpeed = isRunning ? 1f : 0.5f; // 1 = run, 0.5 = walk

        animator.SetFloat("MoveSpeed", moveSpeed);
        animator.SetBool("isCrouching", isCrouching && isGrounded);

        // Jumping (trigger only when key is pressed down)
        bool isJumping = Input.GetKeyDown(KeyCode.Space) && isGrounded;
        if (isJumping)
        {
            animator.SetTrigger("Jump");
            if (jumpSound != null && audioSource != null)
                audioSource.PlayOneShot(jumpSound);
        }

        // Play footsteps sound when running
        if (isRunning && isGrounded)
        {
            if (footstepsSound != null && audioSource != null && !audioSource.isPlaying)
            {
                audioSource.clip = footstepsSound;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource != null && audioSource.isPlaying && audioSource.clip == footstepsSound)
            {
                audioSource.Stop();
            }
        }

        // Attack trigger (on left click down)
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("isAttacking");
        }
    }
}