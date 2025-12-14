using System.Collections;
using UnityEngine;

public class Traps : MonoBehaviour
{
    public bool isStaticTrap = false;
    public bool rotate = true;     

    public float speed;
    Vector3 targetPos;

    public GameObject ways;
    public Transform[] wayPoints;

    int pointIndex = 0;
    int pointCount = 0;
    int direction = 1;

    public float waitTime;
    int speedMultiplier = 1;
    public float rotateSpeed = 300f;

    public float damageCooldown = 0.4f;
    bool canDamage = true;

    private void Awake()
    {
        if (isStaticTrap) return;
        if (ways == null) return;

        wayPoints = new Transform[ways.transform.childCount];
        for (int i = 0; i < ways.transform.childCount; i++)
        {
            wayPoints[i] = ways.transform.GetChild(i);
        }
    }

    private void Start()
    {
        if (isStaticTrap) return;
        if (ways == null) return;

        pointCount = wayPoints.Length;
        pointIndex = 1;
        targetPos = wayPoints[pointIndex].position;

        StartCoroutine(WaitNextPoint());
    }

    private void Update()
    {
        // rotate only if enabled
        if (rotate)
            transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);

        if (isStaticTrap) return;
        if (ways == null) return;

        var step = speedMultiplier * speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, step);

        if (transform.position == targetPos)
        {
            NextPoint();
        }
    }

    void NextPoint()
    {
        StartCoroutine(WaitNextPoint());

        if (pointIndex == pointCount - 1)
            direction = -1;

        if (pointIndex == 0)
            direction = 1;

        pointIndex += direction;
        targetPos = wayPoints[pointIndex].position;
    }

    IEnumerator WaitNextPoint()
    {
        speedMultiplier = 0;
        yield return new WaitForSeconds(waitTime);
        speedMultiplier = 1;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && canDamage)
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(100f);
                StartCoroutine(DamageDelay());
            }
        }
    }

    IEnumerator DamageDelay()
    {
        canDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        canDamage = true;
    }
}
