using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public Animator animator;
    [SerializeField] public float maxHitpoints;
    float hitPoints;
    public HealthbarBehvior healthbar;
    public GameObject DeathText; 
    private bool isDead = false;
    private bool isInvulnerable = false;
    [SerializeField] private float invulnerableDuration = 1f;
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip DeathSound;
    private Vector3 respawnPosition;
    void Start()
    {
        hitPoints = maxHitpoints;
        respawnPosition = transform.position;
        if (healthbar != null)
            healthbar.SetHealth(hitPoints, maxHitpoints);

        if (DeathText != null)
            DeathText.SetActive(false);
    }

    void Update()
    {
       
        if (isDead)
        {
    

            if (Input.GetKeyDown(KeyCode.R))
            {
                Debug.Log("r tangiang ayaw gumana");
                Revive();
                StartCoroutine(InvulnerabilityTimer());
            }
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead || isInvulnerable) return;

        hitPoints -= damage;
        if (healthbar != null)
            healthbar.SetHealth(hitPoints, maxHitpoints);

        SoundManager.instance.PlaySound(hurtSound);
        animator.SetTrigger("Hurt");

        if (hitPoints <= 0)
            Die();
    }


    void Die()
    {
        SoundManager.instance.PlaySound(DeathSound);
        isDead = true;
        Debug.Log("Player died!");

        animator.SetBool("Death", true);

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;
        }

        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;

        PlayerMovement move = GetComponent<PlayerMovement>();
        if (move != null)
            move.enabled = false;

        PlayerCombatScript combat = GetComponent<PlayerCombatScript>();
        if (combat != null)
            combat.enabled = false;

        if (healthbar != null)
            healthbar.gameObject.SetActive(false);

        if (DeathText != null)
            DeathText.SetActive(true); 
    }

    void Revive()
    {
        isDead = false;
        hitPoints = maxHitpoints;

        if (healthbar != null)
            healthbar.SetHealth(hitPoints, maxHitpoints);

        transform.position = respawnPosition; 

        animator.SetBool("Death", false);
        animator.SetTrigger("Recover");

        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = true;

        PlayerMovement move = GetComponent<PlayerMovement>();
        if (move != null)
            move.enabled = true;

        PlayerCombatScript combat = GetComponent<PlayerCombatScript>();
        if (combat != null)
            combat.enabled = true;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.gravityScale = 1;

        if (healthbar != null)
            healthbar.gameObject.SetActive(true);

        if (DeathText != null)
            DeathText.SetActive(false);

       
    }

    public bool IsDead()
    {
        return isDead;
    }
    private System.Collections.IEnumerator InvulnerabilityTimer()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(invulnerableDuration);
        isInvulnerable = false;
    
    }
    public void SetCheckpoint(Vector3 newCheckpoint)
    {
        respawnPosition = newCheckpoint;
       
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
           
        }
    }

}
