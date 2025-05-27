using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections;

public class PoemDisplay : MonoBehaviour
{
    public GameObject poemPanel;
    public TMP_Text leftText;
    public TMP_Text rightText;

    public AudioSource backgroundMusic;    
    public AudioSource eerieMusic;           

    public float musicFadeDuration = 2f;    

    private PlayerControls controls;
    private bool isPoemActive = false;
    private bool canClose = false;

    private string leftPoem = @"A red string tied around my wrist,
and a knot that cannot be undone,
takes me back to when we kissed,
to the moment our love had begun.

My tears should have been scalding hot,
but my skin has long gone numb,
and as my wounds began to rot,
I learned to accept who I've become.";

    private string rightPoem = @"It is difficult to look at this mirror,
without seeing a face so ridden with guilt,
my desires have never been clearer,
yet my heart can never be fulfilled.

My fault, I should have been wiser,
Maybe your burden would have been lighter
I was too ignorant and too selfish to care,
about all the responsibilities you had to bear.

I thought of giving up and was about to quit
this life weighed down by endless hurt,
but how dare I leave a world,
that still has you in it?";

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Interact.performed += ctx => TryClosePoem();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    public void ShowPoem()
    {
        ShowPoem(true);
    }

    public void ShowPoem(bool stream)
    {
        if (isPoemActive) return;

        poemPanel.SetActive(true);
        Time.timeScale = 0f;

        isPoemActive = true;
        canClose = false;

        leftText.text = "";
        rightText.text = "";

        if (stream)
            StartCoroutine(TypePoemCoroutine());
        else
        {
            leftText.text = leftPoem;
            rightText.text = rightPoem;
            StartCoroutine(EnableCloseAfterDelay(0.5f));
        }

        // Start fading music
        StartCoroutine(FadeMusicToEerie());
    }

    private IEnumerator FadeMusicToEerie()
    {
        if (backgroundMusic == null || eerieMusic == null)
            yield break;

        if (!eerieMusic.isPlaying)
        {
            eerieMusic.volume = 0f;
            eerieMusic.Play();
        }

        float timer = 0f;
        float startVolumeBackground = backgroundMusic.volume;
        float targetVolumeEerie = 1f;

        while (timer < musicFadeDuration)
        {
            timer += Time.unscaledDeltaTime; 

            float t = timer / musicFadeDuration;
            backgroundMusic.volume = Mathf.Lerp(startVolumeBackground, 0f, t);
            eerieMusic.volume = Mathf.Lerp(0f, targetVolumeEerie, t);

            yield return null;
        }

        backgroundMusic.volume = 0f;
        backgroundMusic.Pause();

        eerieMusic.volume = targetVolumeEerie;
    }

    private IEnumerator FadeMusicToBackground()
    {
        if (backgroundMusic == null || eerieMusic == null)
            yield break;

        if (!backgroundMusic.isPlaying)
        {
            backgroundMusic.volume = 0f;
            backgroundMusic.Play();
        }

        float timer = 0f;
        float targetVolumeBackground = 1f;
        float startVolumeEerie = eerieMusic.volume;

        while (timer < musicFadeDuration)
        {
            timer += Time.unscaledDeltaTime;

            float t = timer / musicFadeDuration;
            eerieMusic.volume = Mathf.Lerp(startVolumeEerie, 0f, t);
            backgroundMusic.volume = Mathf.Lerp(0f, targetVolumeBackground, t);

            yield return null;
        }

        eerieMusic.volume = 0f;
        eerieMusic.Pause();

        backgroundMusic.volume = targetVolumeBackground;
    }

    private IEnumerator TypePoemCoroutine()
    {
        yield return StartCoroutine(StreamText(leftText, leftPoem));
        yield return new WaitForSecondsRealtime(1.5f);
        yield return StartCoroutine(StreamText(rightText, rightPoem));
        yield return StartCoroutine(EnableCloseAfterDelay(2f));
    }

    private IEnumerator StreamText(TMP_Text textComponent, string fullText)
    {
        foreach (char c in fullText)
        {
            textComponent.text += c;
            yield return new WaitForSecondsRealtime(0.02f);
        }
    }

    private IEnumerator EnableCloseAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        canClose = true;
    }

    private void TryClosePoem()
    {
        if (!isPoemActive || !canClose) return;

        poemPanel.SetActive(false);
        Time.timeScale = 1f;

        isPoemActive = false;

        StartCoroutine(FadeMusicToBackground());
    }
}
