using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MainMenuPanel : MonoBehaviour
{
    private Button startButton;
    private Button SettingsButton;
    private Button progressButton;
    private Button exitButton;

    private void Awake()
    {
        startButton = transform.Find("StartButton").GetComponent<Button>();
        SettingsButton = transform.Find("SettingsButton").GetComponent<Button>();
        progressButton = transform.Find("ProgressButton").GetComponent<Button>();
        exitButton = transform.Find("ExitButton").GetComponent<Button>();
    }
    private void Start()
    {
        startButton.onClick.AddListener(OnStartButtonClicked);
        SettingsButton.onClick.AddListener(OnSettingsButtonClick);
        progressButton.onClick.AddListener(OnprogressButtonOnClick);
        exitButton.onClick.AddListener(OnExitButtonClick);
    }
    private void OnStartButtonClicked()
    {
        SceneManager.LoadScene(1);
    }
    private void OnSettingsButtonClick()
    {

    }
    private void OnprogressButtonOnClick()
    {

    }
    private void OnExitButtonClick()
    {
        Application.Quit();
    }
}
