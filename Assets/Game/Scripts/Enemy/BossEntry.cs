using UnityEngine;

public class BossEntry : MonoBehaviour
{
    public Boss boss;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            boss.StartBossAfterDelay();
            
        }
    }
}
