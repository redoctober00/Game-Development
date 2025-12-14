using UnityEngine;
using System.Collections;

public class SceneEntry : MonoBehaviour
{
    public Transform entryPoint;
    public Transform stopPoint;
    public float walkSpeed = 0.7f;

    private Animator animator;
    private PlayerMovement movement;
    private Rigidbody2D rb;

    private float groundY;

    void Start()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();

        // Use the player's current Y as the ground level
        groundY = transform.position.y;

        // Place at entry point but keep Y grounded
        transform.position = new Vector3(entryPoint.position.x, groundY, transform.position.z);

        // Disable controls
        movement.enabled = false;

        // Freeze physics
        rb.isKinematic = true;
        rb.velocity = Vector2.zero;

        StartCoroutine(WalkIntoScene());
        transform.localScale = new Vector3(-1f, 1f, 1f);
    }

    IEnumerator WalkIntoScene()
    {
        animator.SetInteger("AnimState", 2);

        while (Mathf.Abs(transform.position.x - stopPoint.position.x) > 0.1f)
        {
            // Move only horizontally
            transform.position = new Vector3(
                Mathf.MoveTowards(transform.position.x, stopPoint.position.x, walkSpeed * Time.deltaTime),
                groundY,
                transform.position.z
            );

            yield return null;
        }

        animator.SetInteger("AnimState", 0);

        // Re-enable physics and controls
        rb.isKinematic = false;
        movement.enabled = true;
    }
}
