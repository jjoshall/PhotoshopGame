using UnityEngine;

public class RockSpawner : MonoBehaviour
{
    [SerializeField] private GameObject rockPrefab;

    private float timer;
    [SerializeField] private float spawnInterval = 2f;

    // Camera bounds
    private float minX = -9f, maxX = 9f, minY = -5f, maxY = 5f;

    private void Update() {
        timer += Time.deltaTime;

        if (timer >= spawnInterval) {
            SpawnRock();
            timer = 0f;
        }
    }

    private void SpawnRock() {
        if (rockPrefab == null) {
            Debug.Log("Rock prefab is not assigned.");
            return;
        }

        Vector3 spawnPosition = GetRandomEdgePosition();
        ObjectPoolManager.SpawnObject(rockPrefab, spawnPosition, Quaternion.identity);


    }

    private Vector3 GetRandomEdgePosition() {
        // Pick a random edge: 0 = left, 1 = right, 2 = top, 3 = bottom
        int edge = Random.Range(0, 4);

        switch(edge) {
            case 0: // Left
                return new Vector3(minX - 1f, Random.Range(minY, maxY), 0f);
            case 1: // Right
                return new Vector3(maxX + 1f, Random.Range(minY, maxY), 0f);
            case 2: // Top
                return new Vector3(Random.Range(minX, maxX), maxY + 1f, 0f);
            case 3: // Bottom
                return new Vector3(Random.Range(minX, maxX), minY - 1f, 0f);
            default:
                return Vector3.zero;
        }
    }
}
