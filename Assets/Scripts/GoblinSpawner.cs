using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoblinSpawner : MonoBehaviour
{
    public GameObject goblinPrefab;
    public Transform leftTowerSpawnPoint;
    public Transform rightTowerSpawnPoint;
    public int goblinsToSpawnPerTower = 5;
    public float spawnInterval = 2f;

    public AudioClip goblinDeathSound;
    public AudioClip victorySound;

    private readonly int totalGoblins = 10;
    private List<Goblin> activeGoblins = new List<Goblin>(); // List to track active goblins
    private bool canSpawn = false;
    private int goblinsSpawned = 0; // Counter for spawned goblins

    private AudioSource audioSource;

    void Start()
    {
        PlayerController.OnStaffCollected += StartSpawning; // Subscribe to the staff collected event

        audioSource = GetComponent<AudioSource>();
    }

    void OnDestroy()
    {
        PlayerController.OnStaffCollected -= StartSpawning; // Unsubscribe to prevent memory leaks
    }

    void StartSpawning()
    {
        if (!canSpawn)
        {
            canSpawn = true;
            SpawnGoblins();
        }
    }

    public void SpawnGoblins()
    {
        StartCoroutine(SpawnGoblinsFromTower(leftTowerSpawnPoint.position));
        StartCoroutine(SpawnGoblinsFromTower(rightTowerSpawnPoint.position));
    }

    private IEnumerator SpawnGoblinsFromTower(Vector3 spawnPosition)
    {
        for (int i = 0; i < goblinsToSpawnPerTower; i++)
        {
            if (goblinsSpawned < totalGoblins)
            {
                SpawnGoblin(spawnPosition);
                goblinsSpawned++;
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnGoblin(Vector3 spawnPosition)
    {
        GameObject newGoblinObj = Instantiate(goblinPrefab, spawnPosition, Quaternion.identity);
        newGoblinObj.transform.position = new Vector3(spawnPosition.x, 49.06106f, -2.42f); // Ensure fixed Y and Z position
        Goblin newGoblin = newGoblinObj.GetComponent<Goblin>();
        if (newGoblin != null)
        {
            activeGoblins.Add(newGoblin); // Add the new goblin to the active goblins list

            // Subscribe to the OnDeath event to handle goblin death
            newGoblin.OnDeath += () => HandleGoblinDeath(newGoblin);
        }
    }

    void HandleGoblinDeath(Goblin goblin)
    {
        // Remove the goblin from the active list
        activeGoblins.Remove(goblin);
        AudioSource.PlayClipAtPoint(goblinDeathSound, goblin.transform.position);

        // Check if all goblins are defeated
        if (activeGoblins.Count == 0 && goblinsSpawned >= totalGoblins)
        {
            PlayVictorySoundEffect();
            SceneManager.LoadScene("Finish");
        }
    }

    void PlayVictorySoundEffect()
    {
        AudioSource.PlayClipAtPoint(victorySound, Camera.main.transform.position);
    }
}