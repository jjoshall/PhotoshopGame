using UnityEngine;

public class RockSpawner : MonoBehaviour
{
    [SerializeField] private GameObject prefabToSpawn;

    private float timer;
    [SerializeField] private float spawnInterval = 3f;

    [SerializeField] private int waveNumber = 1;
    private int currentWave = 0;
    [SerializeField] private int enemiesPerWave = 10;

    private void Update() {
        timer += Time.deltaTime;

        if (timer >= spawnInterval) {
            SpawnRock();
            timer = 0f;
        }
    }

    private void SpawnRock() {
        if (prefabToSpawn == null) {
            Debug.Log("Prefab is not assigned.");
            return;
        }

        Vector2 spawnPosition = GetRandomPosition();

        ObjectPoolManager.SpawnObject(prefabToSpawn, spawnPosition, Quaternion.identity, ObjectPoolManager.PoolType.GameObjects);
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

    private void WaveSpawner() {

    }
}
