using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VRHealthController : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public float hapticAmplitude = 0.5f;
    public float hapticDuration = 0.2f;

    private XRDirectInteractor directInteractor;

    void Start()
    {
        currentHealth = maxHealth;

        // Assuming you have XRDirectInteractor attached to the controller
        directInteractor = GetComponent<XRDirectInteractor>();
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            Die();
        }

        TriggerHapticFeedback();
    }

    void Die()
    {
        // Handle death state, e.g., respawn or game over logic
        currentHealth = maxHealth;
    }

    void TriggerHapticFeedback()
    {
        if (directInteractor != null)
        {
            // Trigger haptic feedback on the XR controller
            directInteractor.SendHapticImpulse(hapticAmplitude, hapticDuration);
        }
    }
}

