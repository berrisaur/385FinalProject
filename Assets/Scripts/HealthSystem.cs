using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public Slider healthSlider;
    public float maxHealth = 100f;
    public float currentHealth;

    // Delegate and event for health changes
    public delegate void OnHealthChangedDelegate(float currentHealth, float maxHealth);
    public event OnHealthChangedDelegate OnHealthChanged;

    void Start()
    {
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;

        OnHealthChanged?.Invoke(currentHealth, maxHealth); // Notify on start
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        healthSlider.value = currentHealth;
        
        OnHealthChanged?.Invoke(currentHealth, maxHealth); // Notify change


        if (currentHealth == 0)
        {
            SceneManager.LoadScene(2);
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        healthSlider.value = currentHealth;

        OnHealthChanged?.Invoke(currentHealth, maxHealth); // Notify change
    }
}
