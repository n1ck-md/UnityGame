using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void NewGame()
    {
        SaveManager.ClearSave();              
        GameState.isLoadingFromSave = false; 
        SceneManager.LoadScene("FirstMapScene");
    }

    public void ContinueGame()
    {
        if (SaveManager.HasSavedGame())
        {
            SaveData data = SaveManager.LoadGame();
            GameState.isLoadingFromSave = true; 
            SceneManager.LoadScene(data.sceneName);
        }
        else
        {
            Debug.LogWarning("No save file found!");
        }
    }

    public void OpenSettings()
    {
        Debug.Log("Settings panel should appear.");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game closed.");
    }
}
