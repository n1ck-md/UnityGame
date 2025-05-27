using UnityEngine;

public class PlayerLoader : MonoBehaviour
{
    public Vector3 defaultStartPosition;

    void Awake()
    {
        if (defaultStartPosition == Vector3.zero)
            defaultStartPosition = transform.position;
    }

    void Start()
    {
        if (GameState.isLoadingFromSave)
        {
            SaveData data = SaveManager.LoadGame();
            if (data != null)
            {
                transform.position = new Vector3(data.playerX, data.playerY, data.playerZ);
                //NU RESETARE FLAG AICI
                return;
            }
        }

        transform.position = defaultStartPosition;
    }
}
