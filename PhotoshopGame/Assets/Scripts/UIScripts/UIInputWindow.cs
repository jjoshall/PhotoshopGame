using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInputWindow : MonoBehaviour
{
    [SerializeField] private GameObject highscoreScreen;
    [SerializeField] private Button okBtn;
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private GameObject errorTxt;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) {
            OnOkBtnClicked();
        }
    }

    public void OnOkBtnClicked() {
        string playerName = nameInput.text.Trim().ToUpper();

        if (string.IsNullOrEmpty(playerName)) {
            errorTxt.SetActive(true);
            return;
        }
        if (playerName.Length < 3) {
            errorTxt.SetActive(true);
            return;
        }

        Debug.Log("Player Name: " + playerName);

        DeathTimer.Instance.SetPlayerName(playerName);

        highscoreScreen.SetActive(true);
        gameObject.SetActive(false);
    }
}
