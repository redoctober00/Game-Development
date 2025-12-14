using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
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

    [Header("Boss Intro Delay")]
    [SerializeField] private float startDelay = 10f;
    private bool canStartAttacking = false;

    [Header("Post-Death")]
    [SerializeField] private float deathDelay = 2f; 
    [SerializeField] private string startSceneName = "StartScene"; 

    private float cooldownTimer = Mathf.Infinity;

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
        if (!canStartAttacking) return;

        cooldownTimer += Time.deltaTime;

        // Make sure we have a reference to player health
        if (playerHealth != null && playerHealth.IsDead())
        {
            anim.ResetTrigger("Attacking");
            if (enemyPatrol != null)
                enemyPatrol.enabled = true;

            // Trigger return to menu
            StartCoroutine(ReturnToMenu());
            return;
        }

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

    public void StartBossAfterDelay()
    {
        StartCoroutine(DelayedBossStart());
    }

    private IEnumerator DelayedBossStart()
    {
        yield return new WaitForSeconds(startDelay);
        canStartAttacking = true;
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(
            boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
        {
            playerHealth = hit.transform.GetComponent<PlayerHealth>();

            if (playerHealth != null && playerHealth.IsDead())
                return false;
        }

        return hit.collider != null;
    }

    private void DamagePlayer()
    {
        RaycastHit2D hit = Physics2D.BoxCast(
            boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
        {
            PlayerHealth player = hit.transform.GetComponent<PlayerHealth>();
            if (player != null && !player.IsDead())
            {
                player.TakeDamage(damage);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (boxCollider == null) return;

        Gizmos.color = Color.red;

        Vector3 castOrigin = boxCollider.bounds.center +
                             transform.right * range * transform.localScale.x * colliderDistance;

        Vector3 castSize = new Vector3(
            boxCollider.bounds.size.x * range,
            boxCollider.bounds.size.y,
            boxCollider.bounds.size.z);

        Gizmos.DrawWireCube(castOrigin, castSize);
    }

 
    private IEnumerator ReturnToMenu()
    {
        canStartAttacking = false;           
        yield return new WaitForSeconds(deathDelay);  
        UnityEngine.SceneManagement.SceneManager.LoadScene(startSceneName);
    }

}
