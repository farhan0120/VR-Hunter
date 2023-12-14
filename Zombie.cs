using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class Zombie : MonoBehaviour
{
    public int health = 100;
    public Transform target;
    public float speed = 5.0f;
    public float stoppingDistance = 1.0f;
    public float activationDistance = 15.0f;  // New variable for activation distance

    public GameObject impactEffectPrefab;

    private Rigidbody zombieRigidbody;

    public AudioClip zombieDeathSound;  // Add this line
    private AudioSource audioSource;  // Add this line

    void Start()
    {
        zombieRigidbody = GetComponent<Rigidbody>();
        if (zombieRigidbody == null)
        {
            zombieRigidbody = gameObject.AddComponent<Rigidbody>();
            zombieRigidbody.mass = 2f;
            zombieRigidbody.drag = 1f;
            zombieRigidbody.angularDrag = 1f;
            zombieRigidbody.useGravity = true;  // Ensure that gravity is enabled
        }

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = zombieDeathSound;
    }

    void Update()
    {
        if (target)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            if (distanceToTarget > stoppingDistance && distanceToTarget < activationDistance)
            {
                MoveTowardsTarget();
            }
        }
    }

    void MoveTowardsTarget()
    {
        if (target == null)
        {
            // If the target is null, the zombie has lost its target
            return;
        }
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        if (distanceToTarget > activationDistance)
        {
            // Zombie won't move if the main camera is beyond the activation distance
            return;
        }

        Vector3 targetPosition = new Vector3(target.position.x, transform.position.y, target.position.z);
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        transform.position += moveDirection * speed * Time.deltaTime;
        transform.LookAt(targetPosition);
    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Handle death, e.g., play death animation, spawn particles, etc.
        audioSource.Play();

        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SM_Wep_Broadsword_01"))
        {
            Sword sword = other.GetComponent<Sword>();

            if (sword != null)
            {
                // Assuming your Sword script has a damage value
                TakeDamage(sword.GetDamage());
                ShowImpactEffect(transform.position);
            }
        }
    }

    void ShowImpactEffect(Vector3 position)
    {
        Instantiate(impactEffectPrefab, position, Quaternion.identity);
    }
}
