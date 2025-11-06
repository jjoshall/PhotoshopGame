using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine;

public class RockSpawner : MonoBehaviour
{
    public static RockSpawner Instance { get; private set; }

    [SerializeField] private GameObject prefabToSpawn;
    private readonly List<GameObject> activeEnemies = new List<GameObject>();
    [SerializeField] private GameObject player;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float spawnIntervalScaling = 0.75f;
    [SerializeField] private float waveTimeScaling = 5f;

    [Header("Wave Settings")]
    [SerializeField] private int totalWaves = 3;
    [SerializeField] private float timeForWave = 30f;
    [SerializeField] private TextMeshProUGUI waveNumTxt;
    [SerializeField] private GameObject waveIncomingTxt;
    [SerializeField] private GameObject upgradeScreen;

    private float spawnTimer = 0f;
    private float waveTimer = 0f;
    private int currentWave = 0;
    private bool waveActive = false;

    private void Awake() {
        // Basic singleton pattern
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start() {
        waveIncomingTxt.SetActive(false);
        waveNumTxt.text = "";
        StartCoroutine(WaveLoop());
    }

    public IEnumerator WaveLoop() {
        // Show wave incoming text
        //waveIncomingTxt.SetActive(true);
        //yield return new WaitForSeconds(3f);
        //waveIncomingTxt.SetActive(false);

        // Start wave
        currentWave++;
        waveNumTxt.text = currentWave.ToString();

        waveActive = true;
        waveTimer = timeForWave;
        spawnTimer = 0f;

        while (waveTimer > 0) {
            waveTimer -= Time.deltaTime;
            spawnTimer -= Time.deltaTime;

            if (spawnTimer <= 0) {
                spawnTimer = spawnInterval;
                SpawnRock();
            }

            yield return null;
        }

        // Wave ends
        ClearAllEnemies();
        waveActive = false;

        // Open shop
        ActivateUpgradeScreen();

        // Scale difficulty for next wave
        spawnInterval *= spawnIntervalScaling;
        timeForWave += waveTimeScaling;
    }

    private void SpawnRock() {
        if (prefabToSpawn == null) {
            Debug.Log("Prefab is not assigned.");
            return;
        }

        Vector2 spawnPosition = GetRandomPosition();

        GameObject enemy = ObjectPoolManager.SpawnObject(prefabToSpawn, spawnPosition, Quaternion.identity, ObjectPoolManager.PoolType.GameObjects);

        activeEnemies.Add(enemy);
    }

    private Vector2 GetRandomPosition() {
        Camera cam = Camera.main;
        float camHeight = 2f * cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;

        Vector2 camPos = cam.transform.position;

        int side = Random.Range(0, 3);

        Vector2 spawnPos = Vector2.zero;

        float offset = 1.5f; // how far outside the view to spawn

        switch (side) {
            case 0: // Left
                spawnPos = new Vector2(camPos.x - camWidth / 2 - offset,
                                       Random.Range(camPos.y - camHeight / 2, camPos.y + camHeight / 2));
                break;
            case 1: // Right
                spawnPos = new Vector2(camPos.x + camWidth / 2 + offset,
                                       Random.Range(camPos.y - camHeight / 2, camPos.y + camHeight / 2));
                break;
            case 2: // Bottom
                spawnPos = new Vector2(Random.Range(camPos.x - camWidth / 2, camPos.x + camWidth / 2),
                                       camPos.y - camHeight / 2 - offset);
                break;
        }

        return spawnPos;
    }

    private void ActivateUpgradeScreen() {
        upgradeScreen.SetActive(true);
        player.SetActive(false);
    }

    private void ClearAllEnemies() {
        foreach (GameObject enemy in activeEnemies) {
            ObjectPoolManager.ReturnObjectToPool(enemy, ObjectPoolManager.PoolType.GameObjects);
        }
        activeEnemies.Clear();
    }

    public void RemoveEnemy(GameObject enemy) {
        activeEnemies.Remove(enemy);
    }
}
