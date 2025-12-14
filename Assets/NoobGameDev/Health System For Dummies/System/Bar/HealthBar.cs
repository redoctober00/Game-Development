using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(FollowCameraRotation))]
public class HealthBar : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool isBillboarded = true;
    [SerializeField] private bool showHealthNumbers = true;

    private HealthSystemForDummies health;
    private Image fillImage;
    private Text healthText;
    private FollowCameraRotation billboard;

    private Coroutine animRoutine;

    private void Awake()
    {
        health = GetComponentInParent<HealthSystemForDummies>();
        fillImage = GetComponentInChildren<Image>();
        healthText = GetComponentInChildren<Text>();
        billboard = GetComponent<FollowCameraRotation>();
    }

    private void Start()
    {
        // This is the magic line: update when taking damage
        health.OnCurrentHealthChanged.AddListener(OnHealthChanged);

        UpdateInstant();
    }

    private void Update()
    {
        billboard.enabled = isBillboarded;
        healthText.enabled = showHealthNumbers;
        healthText.text = $"{health.CurrentHealth}/{health.MaximumHealth}";
    }

    private void OnHealthChanged(CurrentHealth h)
    {
        float targetFill = h.percentage / 100f;

        if (animRoutine != null)
            StopCoroutine(animRoutine);

        animRoutine = StartCoroutine(AnimateHealth(targetFill));
    }

    private IEnumerator AnimateHealth(float target)
    {
        float start = fillImage.fillAmount;
        float time = 0;
        float duration = health.AnimationDuration;

        while (time < duration)
        {
            fillImage.fillAmount = Mathf.Lerp(start, target, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        fillImage.fillAmount = target;
    }

    private void UpdateInstant()
    {
        fillImage.fillAmount = health.CurrentHealthPercentage / 100f;
        healthText.text = $"{health.CurrentHealth}/{health.MaximumHealth}";
    }
}
