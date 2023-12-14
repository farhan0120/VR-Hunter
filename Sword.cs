using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class Sword : MonoBehaviour
{
    

    public int GetDamage()
    {
        // Adjust this as needed
        return 50;
    }

    public GameObject impactEffectPrefab;  // Add this line
    public XRBaseControllerInteractor controllerInteractor;  // Reference to the XR controller interactor
    public float hapticDuration = 0.2f;  // Duration of haptic feedback in seconds




    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Sword OnCollisionEnter Triggered");  // Add this line

        // Check if the collided object is a zombie
        Zombie zombie = collision.gameObject.GetComponent<Zombie>();

        if (zombie != null)
        {
            // If the collided object is a zombie, deal damage
            zombie.TakeDamage(GetDamage());

            // Log a message for testing
            Debug.Log("Sword collided with Zombie");

            
            // You can add additional effects or logic here, e.g., play a hit sound, show hit particles, etc.
            ShowImpactEffect(collision.contacts[0].point);
            // Trigger haptic feedback
            TriggerHapticFeedback();

        }
    }


     void ShowImpactEffect(Vector3 position)
    {
        Debug.Log("Impact Effect Triggered");  // Add this line

        // Instantiate the impact effect prefab at the collision point
        Instantiate(impactEffectPrefab, position, Quaternion.identity);

    }

    void TriggerHapticFeedback()
    {
        if (controllerInteractor != null)
        {
            StartCoroutine(DoHapticFeedback());
        }
    }

    IEnumerator DoHapticFeedback()
    {
        controllerInteractor.SendHapticImpulse(0.5f, hapticDuration);  // Adjust the intensity as needed

        yield return null;
    }
    
}
