using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SettingsInPause : MonoBehaviour
{
    public GameObject settingsPanel;
    public Slider volumeSlider;
    public Toggle fullscreenToggle;
    public AudioSource pingSound;

    private bool isDraggingSlider = false;

    void Start()
    {
        volumeSlider.value = AudioListener.volume;
        volumeSlider.onValueChanged.AddListener(SetVolume);

        EventTrigger trigger = volumeSlider.gameObject.AddComponent<EventTrigger>();

        var dragEntry = new EventTrigger.Entry { eventID = EventTriggerType.BeginDrag };
        dragEntry.callback.AddListener((_) => isDraggingSlider = true);
        trigger.triggers.Add(dragEntry);

        var endDragEntry = new EventTrigger.Entry { eventID = EventTriggerType.EndDrag };
        endDragEntry.callback.AddListener((_) =>
        {
            isDraggingSlider = false;
            PlayPingSound();
        });
        trigger.triggers.Add(endDragEntry);

        fullscreenToggle.isOn = Screen.fullScreen;
        fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
        fullscreenToggle.isOn = Screen.fullScreen;
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    public void SetVolume(float value)
    {
        AudioListener.volume = value;
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    private void PlayPingSound()
    {
        if (pingSound != null)
        {
            pingSound.PlayOneShot(pingSound.clip, AudioListener.volume);
        }
    }
}
