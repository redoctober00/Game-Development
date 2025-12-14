using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private int damage;

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;

    // References
    private Animator anim;
    private PlayerHealth playerHealth;
    private EnemyPatrol enemyPatrol;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        // If player exists AND is dead, stop everything
        if (playerHealth != null && playerHealth.IsDead())
        {
            anim.ResetTrigger("Attacking");
            if (enemyPatrol != null)
                enemyPatrol.enabled = true;

            return;
        }

        // Normal logic
        if (PlayerInSight())
        {
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                anim.SetTrigger("Attacking");
            }
        }

        if (enemyPatrol != null)
            enemyPatrol.enabled = !PlayerInSight();
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit =
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
        {
            playerHealth = hit.transform.GetComponent<PlayerHealth>();

            // Ignore detection if player is dead
            if (playerHealth != null && playerHealth.IsDead())
                return false;
        }

        return hit.collider != null;
    }

    private void DamagePlayer()
    {
        // Recast to confirm
        RaycastHit2D hit =
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
        {
            PlayerHealth player = hit.transform.GetComponent<PlayerHealth>();

            if (player != null)
            {
                // Don’t damage dead players
                if (player.IsDead()) return;

                player.TakeDamage(damage);
            }
        }
    }
    private void OnDrawGizmos()
    {
        if (boxCollider == null)
            return;

        Gizmos.color = Color.red;

        // Same bounds as your BoxCast
        Vector3 castOrigin = boxCollider.bounds.center +
                             transform.right * range * transform.localScale.x * colliderDistance;

        Vector3 castSize = new Vector3(
            boxCollider.bounds.size.x * range,
            boxCollider.bounds.size.y,
            boxCollider.bounds.size.z
        );

        Gizmos.DrawWireCube(castOrigin, castSize);
    }

}
