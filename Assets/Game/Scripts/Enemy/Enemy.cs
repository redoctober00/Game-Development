
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator animator;
    float hitPoints;
    [SerializeField] public float maxHitpoints;
    public HealthbarBehvior healthbar;
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip DeathSound;
    void Start()
    {
        hitPoints = maxHitpoints;
        healthbar.SetHealth(hitPoints, maxHitpoints);
    }

    public void TakeDamage(float damage)
    {
        hitPoints -= damage;
        healthbar.SetHealth(hitPoints, maxHitpoints);
        SoundManager.instance.PlaySound(hurtSound);
        animator.SetTrigger("Hurt");

        if (hitPoints <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        Debug.Log("Enemy died!");
        SoundManager.instance.PlaySound(DeathSound);

        EnemyPatrol patrol = GetComponent<EnemyPatrol>();
        if (patrol != null)
            patrol.enabled = false;

        MeleeEnemy meleeEnemy = GetComponent<MeleeEnemy>();
        if (meleeEnemy != null)
            meleeEnemy.enabled = false;

    
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;
            rb.bodyType = RigidbodyType2D.Kinematic; 
        }


        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;

        animator.SetBool("isDead", true);
        animator.SetTrigger("Die");

     
        if (healthbar != null)
            healthbar.gameObject.SetActive(false);

        Destroy(gameObject, 5f); 
    }
}