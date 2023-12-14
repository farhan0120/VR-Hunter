using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject[] zombiePrefabs;
    public Transform player;
    public Vector3 spawnAreaSize = new Vector3(100f, 10f, 100f);
    public float spawnHeight = 1f;
    public float spawnRate = 30f;
    public int maxSpawnAttempts = 10;
    public LayerMask blockingLayers;

    void Start()
    {
        InvokeRepeating("TrySpawnZombie", spawnRate, spawnRate);
    }

    void TrySpawnZombie()
    {
        if (Random.value <= 0.33f)
        {
            SpawnZombie();
        }
    }

    void SpawnZombie()
    {
        for (int i = 0; i < maxSpawnAttempts; i++)
        {
            Vector3 randomPosition = GetRandomPosition();
            if (IsPositionOpen(randomPosition))
            {
                GameObject zombiePrefab = zombiePrefabs[Random.Range(0, zombiePrefabs.Length)];
                GameObject spawnedZombie = Instantiate(zombiePrefab, randomPosition, Quaternion.identity);

                // Add RigidBody to the zombie
                Rigidbody zombieRigidbody = spawnedZombie.GetComponent<Rigidbody>();
                if (zombieRigidbody == null)
                {
                    zombieRigidbody = spawnedZombie.AddComponent<Rigidbody>();
                }

                // Add CapsuleCollider to the zombie
                CapsuleCollider zombieCollider = spawnedZombie.GetComponent<CapsuleCollider>();
                if (zombieCollider == null)
                {
                    zombieCollider = spawnedZombie.AddComponent<CapsuleCollider>();
                    zombieCollider.height = 2f;
                }

                Zombie zombieComponent = spawnedZombie.GetComponent<Zombie>();
                if (zombieComponent == null)
                {
                    zombieComponent = spawnedZombie.AddComponent<Zombie>();
                }
                zombieComponent.target = player;

                return;  // Exit once a zombie is spawned
            }
        }
    }

    Vector3 GetRandomPosition()
    {
        float x = Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2);
        float z = Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2);
        return new Vector3(x, spawnHeight, z) + transform.position;
    }

    bool IsPositionOpen(Vector3 position)
    {
        Vector3 capsulePoint1 = position + Vector3.up * (spawnHeight - 1); // Bottom of the capsule
        Vector3 capsulePoint2 = position + Vector3.up * spawnHeight;       // Top of the capsule

        Collider[] hitColliders = Physics.OverlapCapsule(capsulePoint1, capsulePoint2, 0.5f, blockingLayers);
        return hitColliders.Length == 0;
    }
}
