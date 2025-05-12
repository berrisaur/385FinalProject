using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AutoHeal : MonoBehaviour
{
    public Slider healthSlider;
    public float maxHealth = 100f;
    public float currentHealth;
    public float healRate = 5f;
    public float healDelay = 3f;

    private float lastDamageTime;

    // public TMP_Text healthRate;

    void Start()
    {
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    void Update()
    {
        if (Time.time - lastDamageTime >= healDelay && currentHealth < maxHealth)
        {
            currentHealth += healRate * Time.deltaTime;
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
            healthSlider.value = currentHealth; // Update UI
            // healthRate.text = currentHealth.ToString();
        }

        // healthRate.text = currentHealth.ToString();

        
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        lastDamageTime = Time.time;
        healthSlider.value = currentHealth; // Update UI
    }
}
