using UnityEngine;

public class RockSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabsToSpawn;

    private float timer;
    [SerializeField] private float spawnInterval = 3f;

    private void Update() {
        timer += Time.deltaTime;

        if (timer >= spawnInterval) {
            SpawnRock();
            timer = 0f;
        }
    }

    private void SpawnRock() {
        if (prefabsToSpawn == null) {
            Debug.Log("Prefab is not assigned.");
            return;
        }

        Vector2 spawnPosition = GetRandomPosition();

        GameObject spawnedPrefab = GetRandomPrefab();

        ObjectPoolManager.SpawnObject(spawnedPrefab, spawnPosition, Quaternion.identity, ObjectPoolManager.PoolType.GameObjects);
    }

    private Vector2 GetRandomPosition() {
        Vector2 spawnPosition = new Vector2(Random.Range(-25, 25), Random.Range(-4, 25));

        return spawnPosition;
    }

    private GameObject GetRandomPrefab() {
        int val = Random.Range(0, 11);

        if (val <= 6) {
            return prefabsToSpawn[0];
        }
        else {
            return prefabsToSpawn[1];
        }
    }
}
