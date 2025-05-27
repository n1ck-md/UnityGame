using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class PasswordChecker : MonoBehaviour
{
    public TMP_InputField inputField;
    public GameObject gateToOpen; 
    public GameObject uiPanel;
    public string correctWord = "dream";
    public TextMeshProUGUI feedbackText; 
    public AudioSource audioSource; 
    public AudioClip correctSound;


    public void CheckPassword()
    {
        if (inputField.text.Trim().ToLower() == correctWord.ToLower())
        {

            if (audioSource != null && correctSound != null)
            {
                audioSource.PlayOneShot(correctSound);
            }
            gateToOpen.SetActive(false);  
            uiPanel.SetActive(false);    
            Time.timeScale = 1f;
        }
        else
        {
            if (feedbackText != null)
                feedbackText.text = "Don't fool yourself";
        }
    }

    public void ClosePanel()
    {
        uiPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
