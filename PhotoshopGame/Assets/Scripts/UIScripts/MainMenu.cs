using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject upgradeScreen;

    public void PlayBtn() {
        SceneManager.LoadScene("SampleScene");
    }

    public void UpgradeBtn() {
        upgradeScreen.SetActive(true);
    }

    public void QuitBtn() {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void BackToMenuBtn() {
        upgradeScreen.SetActive(false);
    }
}
