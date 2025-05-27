using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    private bool isPaused = false;
    public GameObject settingsPanel;
    public Transform player;


    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void QuitToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuScene");
    }

    public void SaveGame()
    {
        // Placeholder for save logic
        SaveManager.SaveGame(player);
        Debug.Log("Game Saved");
    }

    public void LoadGame()
    {
        SaveData data = SaveManager.LoadGame();
        if (data != null)
        {
            Time.timeScale = 1f;
            GameState.isLoadingFromSave = true;
            SceneManager.LoadScene(data.sceneName);
        }
        else
        {
            Debug.Log("No saved game to load.");
        }
    }


    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
        // pauseMenuUI.SetActive(false);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        // pauseMenuUI.SetActive(true);
    }
}
