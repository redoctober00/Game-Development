using UnityEngine;
using System.Collections;
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float jumpForce = 7.5f;
    [SerializeField] private AudioClip walkSound;
    [SerializeField] private AudioClip jumpSound;
 

    private Animator animator;
    private Rigidbody2D rb2d;
    private Sensor_Bandit groundSensor;
    private AudioSource audioSource;

    private bool isGrounded = false;
    private bool combatIdle = false;


    void Start()
    {
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Bandit>();

        // Setup AudioSource for walk sound
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = walkSound;
        audioSource.loop = true;
        audioSource.playOnAwake = false;

    }

    void Update()
    {
        CheckGroundStatus();
        HandleMovement();
        HandleAnimations();
    }

    private void CheckGroundStatus()
    {
        if (!isGrounded && groundSensor.State())
        {
            isGrounded = true;
            animator.SetBool("Grounded", true);
        }
        else if (isGrounded && !groundSensor.State())
        {
            isGrounded = false;
            animator.SetBool("Grounded", false);
        }

    }

    private void HandleMovement()
    {
        float inputX = Input.GetAxis("Horizontal");
      
        // Flip sprite
        if (inputX != 0)
            transform.localScale = new Vector3(inputX > 0 ? -1f : 1f, 1f, 1f);

        // Move character
        rb2d.velocity = new Vector2(inputX * moveSpeed, rb2d.velocity.y);

        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            Jump();

        // play when moving on ground, stop otherwise
        if (Mathf.Abs(inputX) > Mathf.Epsilon && isGrounded)
        {
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
        else
        {
            if (audioSource.isPlaying)
                audioSource.Stop();
        }
    }

    private void Jump()
    {
        SoundManager.instance.PlaySound(jumpSound);
        animator.SetTrigger("Jump");
        isGrounded = false;
        animator.SetBool("Grounded", false);
        rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
        groundSensor.Disable(0.2f);
    }

    private void HandleAnimations()
    {
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack");
            return;
        }

        if (Mathf.Abs(rb2d.velocity.x) > Mathf.Epsilon)
        {
            animator.SetInteger("AnimState", 2);
        }
        else if (combatIdle)
        {
            animator.SetInteger("AnimState", 1);
        }
        else
        {
            animator.SetInteger("AnimState", 0);
        }

        animator.SetFloat("AirSpeed", rb2d.velocity.y);
    }
  

}
