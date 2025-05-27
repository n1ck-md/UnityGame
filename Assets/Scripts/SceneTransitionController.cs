using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneTransitionController : MonoBehaviour
{
    public Image fadePanel;
    public float fadeDuration = 1f;
    private bool isTransitioning = false;

    void Start()
    {
        fadePanel.color = new Color(0, 0, 0, 0);
    }

    public void StartGame(string sceneName)
    {
        SaveManager.ClearSave();
        GameState.isLoadingFromSave = false;

        if (!isTransitioning)
        {
            StartCoroutine(FadeAndLoad(sceneName));
        }
    }

    public void ContinueGame(string sceneName)
    {
        if (SaveManager.HasSavedGame())
        {
            SaveData data = SaveManager.LoadGame();
            GameState.isLoadingFromSave = true; 
            //SceneManager.LoadScene(data.sceneName);
            if (!isTransitioning)
            {
                StartCoroutine(FadeAndLoad(sceneName));
            }
        }
        else
        {
            Debug.LogWarning("No save file found!");
        }
    }


    IEnumerator FadeAndLoad(string sceneName)
    {
        isTransitioning = true;

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / fadeDuration);
            fadePanel.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        SceneManager.LoadScene(sceneName);

    }
}
