using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
         
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.SetCheckpoint(transform.position);
                Debug.Log("Checkpoint reached at " + transform.position);
            }
            //else
            //{
            //    Debug.LogWarning("Player does not have PlayerHealth component!");
            //}
        }
    }
}
