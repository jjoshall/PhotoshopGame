using UnityEngine;

public class RockSpawner : MonoBehaviour
{
    [SerializeField] private GameObject rockPrefab;

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
        if (rockPrefab == null) {
            Debug.Log("Rock prefab is not assigned.");
            return;
        }

        Vector2 spawnPosition = GetRandomPosition();

        ObjectPoolManager.SpawnObject(rockPrefab, spawnPosition, Quaternion.identity, ObjectPoolManager.PoolType.GameObjects);
    }

    private Vector2 GetRandomPosition() {
        Vector2 spawnPosition = new Vector2(Random.Range(-25, 25), Random.Range(-4, 25));

        return spawnPosition;
    }
}
