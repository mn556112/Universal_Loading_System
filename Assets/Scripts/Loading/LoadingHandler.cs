using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CustomLoadingBar : MonoBehaviour
{
    public RectTransform leftBar;
    public RectTransform rightBar;
    public float maxBarWidth = 960f;
    public TextMeshProUGUI progressText;

    public float rotationSpeed = -600f; // 회전 속도
    public bool isDebugMode = true; // true면 디버그 모드 (10초간 진행)

    void Start()
    {
        if (isDebugMode)
            StartCoroutine(FakeLoadCoroutine());
        else
            StartCoroutine(LoadAsyncScene());
    }

    IEnumerator FakeLoadCoroutine()
    {
        float duration = 10f;
        float timer = 0f;
        leftBar.sizeDelta = new Vector2(0, leftBar.sizeDelta.y);
        rightBar.sizeDelta = new Vector2(0, rightBar.sizeDelta.y);
        while (timer < duration)
        {
            float progress = timer / duration;
            float currentWidth = maxBarWidth * progress;

            leftBar.sizeDelta = new Vector2(currentWidth, leftBar.sizeDelta.y);
            rightBar.sizeDelta = new Vector2(currentWidth, rightBar.sizeDelta.y);

            if (progressText != null)
                progressText.text = $"{(progress * 100f):F0}%";
            timer += Time.deltaTime;
            yield return null;
        }

        // 최종 100%로 고정
        leftBar.sizeDelta = new Vector2(maxBarWidth, leftBar.sizeDelta.y);
        rightBar.sizeDelta = new Vector2(maxBarWidth, rightBar.sizeDelta.y);
        if (progressText != null)
            progressText.text = "100%";
    }

    IEnumerator LoadAsyncScene()
    {
        string sceneToLoad = LoadingData.nextSceneName;
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);
        operation.allowSceneActivation = false;
        leftBar.sizeDelta = new Vector2(0, leftBar.sizeDelta.y);
        rightBar.sizeDelta = new Vector2(0, rightBar.sizeDelta.y);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            float currentWidth = maxBarWidth * progress;

            leftBar.sizeDelta = new Vector2(currentWidth, leftBar.sizeDelta.y);
            rightBar.sizeDelta = new Vector2(currentWidth, rightBar.sizeDelta.y);

            if (progressText != null)
                progressText.text = $"{(progress * 100f):F0}%";

            if (operation.progress >= 0.9f)
            {
                yield return new WaitForSeconds(0.1f);
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    void Update()
    {
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
}
