using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LetterboxController : MonoBehaviour
{
    public RectTransform topBar;
    public RectTransform bottomBar;
    public float transitionTime = 0.5f;
    public float barHeight = 100f;

    public void ShowBars()
    {
        StopAllCoroutines();
        StartCoroutine(MoveBars(0, barHeight));
    }

    public void HideBars()
    {
        StopAllCoroutines();
        StartCoroutine(MoveBars(barHeight, 0));
    }

    private IEnumerator MoveBars(float from, float to)
    {
        float elapsed = 0f;
        while (elapsed < transitionTime)
        {
            float height = Mathf.Lerp(from, to, elapsed / transitionTime);
            topBar.sizeDelta = new Vector2(0, height);
            bottomBar.sizeDelta = new Vector2(0, height);
            elapsed += Time.deltaTime;
            yield return null;
        }

        topBar.sizeDelta = new Vector2(0, to);
        bottomBar.sizeDelta = new Vector2(0, to);
    }
}
