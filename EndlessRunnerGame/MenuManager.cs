using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private string gameSceneName;

    [Header("UI objects")]
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private GameObject highScorePanel;
    [SerializeField] private GameObject settingsPanel;
    bool infoOpen;
    bool highScoreOpen;
    bool settingsOpen;

    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void ToggleInfo()
    {
        infoPanel.SetActive(!infoOpen);
        infoOpen = !infoOpen;
    }

    public void ToggleHighScore()
    {
        highScorePanel.SetActive(!highScoreOpen);
        highScoreOpen = !highScoreOpen;
    }

    public void ToggleSettings()
    {
        settingsPanel.SetActive(!settingsOpen);
        settingsOpen = !settingsOpen;
    }
}
