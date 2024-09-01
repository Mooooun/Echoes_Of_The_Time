using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerStats : MonoBehaviour
{
    [Header("Other elements references")]
    [SerializeField] private Animator animator;
    [SerializeField] private MoveBehaviour playerMovementScript;
    [SerializeField] private MonoBehaviour cameraScript; // Reference to the camera script
    [SerializeField] private GameObject deathMenu; // Reference to the death menu UI

    [Header("Health")]
    [SerializeField] private float maxHealth = 100f;
    public float currentHealth;

    [SerializeField] private Image healthBarFill;
    public float currentArmorPoints;

    public bool isDead = false;

    void Awake()
    {
        currentHealth = maxHealth; 
        deathMenu.SetActive(false); // Ensure the death menu is inactive at the start
    }

    void Update()
    {
        // Debug: Test damage application
        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(50f);
        }
    }

    public void TakeDamage(float damage, bool overTime = false)
    {
        if (overTime)
        {
            currentHealth -= damage * Time.deltaTime;
        }
        else
        {
            currentHealth -= damage * (1 - (currentArmorPoints / 100));
        }

        currentHealth = Mathf.Max(currentHealth, 0); // Prevent negative health

        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }

        UpdateHealthBarFill();
    }

    private void Die()
    {
        Debug.Log("Player died!");
        isDead = true;
        playerMovementScript.enabled = false; // Disable the movement script
        animator.SetTrigger("Die");

        // Disable the camera script
        if (cameraScript != null)
        {
            cameraScript.enabled = false;
        }

        // Start coroutine to pause the game and show death menu after a delay
        StartCoroutine(PauseGameAndShowDeathMenuAfterDelay(3f));
    }

    private IEnumerator PauseGameAndShowDeathMenuAfterDelay(float delay)
    {
        // Wait for the specified delay time (3 seconds)
        yield return new WaitForSecondsRealtime(delay);

        // Pause the game
        Time.timeScale = 0f;

        // Activate the death menu
        if (deathMenu != null)
        {
            deathMenu.SetActive(true);
        }
    }

    public void UpdateHealthBarFill()
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = currentHealth / maxHealth;
        }
    }

    public void ConsumeItem(float health)
    {
        currentHealth += health;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth; 
        }

        UpdateHealthBarFill();
    }

    // Method to resume the game and deactivate the death menu 
    public void ResumeGame()
    {
        // Hide the death menu
        if (deathMenu != null)
        {
            deathMenu.SetActive(false);
        }

        // Re-enable the camera script
        if (cameraScript != null)
        {
            cameraScript.enabled = true;
        }

        // Resume the game
        Time.timeScale = 1f;

        // Reset player stats and trigger the respawn animation
        ResetPlayer();
    }

    // Optional: Method to reset player stats or any other necessary parameters
    private void ResetPlayer()
    {
        currentHealth = maxHealth;
        isDead = false;
        playerMovementScript.enabled = true; // Re-enable the movement script

        UpdateHealthBarFill();

        // Trigger the respawn animation
        animator.SetTrigger("Respawn");
    }
}
