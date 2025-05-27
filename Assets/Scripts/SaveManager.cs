using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    private static string saveKey = "save_game";

    public static void SaveGame(Transform player)
    {
        SaveData data = new SaveData();
        data.sceneName = SceneManager.GetActiveScene().name;
        data.playerX = player.position.x;
        data.playerY = player.position.y;
        data.playerZ = player.position.z;

        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(saveKey, json);
        PlayerPrefs.Save();
    }

    public static bool HasSavedGame()
    {
        return PlayerPrefs.HasKey(saveKey);
    }

    public static SaveData LoadGame()
    {
        if (!HasSavedGame()) return null;

        string json = PlayerPrefs.GetString(saveKey);
        return JsonUtility.FromJson<SaveData>(json);
    }

    public static void ClearSave()
    {
        PlayerPrefs.DeleteKey(saveKey);
        PlayerPrefs.Save();
        Debug.Log("Save cleared! Exists now? " + PlayerPrefs.HasKey(saveKey));
        
    }
}
