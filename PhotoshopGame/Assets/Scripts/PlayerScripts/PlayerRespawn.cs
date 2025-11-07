using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject respawnPoint;
    [SerializeField] private TextMeshProUGUI respawnText;
    [SerializeField] private PlayerMovement playerMovement;

    private void Start() {
        if (PlayerDeathBar.Instance != null)
            PlayerDeathBar.Instance.OnPlayerDeath += HandleRespawn;
    }

    private void OnDisable() {
        if (PlayerDeathBar.Instance != null)
            PlayerDeathBar.Instance.OnPlayerDeath -= HandleRespawn;
    }

    private void HandleRespawn() {
        if (playerMovement != null) {
            playerMovement.CancelDragOnDeath();
        }

        player.SetActive(false);
        StartCoroutine(RespawnCountdown());
    }

    private IEnumerator RespawnCountdown() {
        int countdown = 3;
        respawnText.gameObject.SetActive(true);

        while (countdown > 0) {
            respawnText.text = $"Respawning in {countdown}...";
            yield return new WaitForSeconds(1f);
            countdown--;
        }

        // Clear the text and respawn
        respawnText.text = "";
        RespawnPlayer();
    }

    private void RespawnPlayer() {
        respawnText.gameObject.SetActive(false);

        player.transform.position = respawnPoint.transform.position;

        PlayerDeathBar.Instance.ResetHealth();
        playerMovement.ResetLaunchPermission();

        player.SetActive(true);
    }
}
