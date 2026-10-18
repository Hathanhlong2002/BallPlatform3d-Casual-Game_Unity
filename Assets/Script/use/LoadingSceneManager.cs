using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using UnityEngine.UI;
public class LoadingSceneManager : MonoBehaviour
{
    public Slider loadingBar;       // Kéo trượt hoặc ProgressBar để hiển thị tiến trình loading
    public TextMeshProUGUI loadingText;  // TextMeshPro để hiển thị thông tin loading

    private void Start()
    {
        StartCoroutine(LoadSceneAsync());
    }

    private IEnumerator LoadSceneAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("LevelSelection");
        if (operation == null) yield break;

        operation.allowSceneActivation = false;
        float displayProgress = 0f;

        while (!operation.isDone)
        {
            // operation.progress ranges 0 to 0.9 before activation
            float targetProgress = Mathf.Clamp01(operation.progress / 0.9f);
            displayProgress = Mathf.MoveTowards(displayProgress, targetProgress, Time.deltaTime * 2f);

            if (loadingBar != null)
            {
                loadingBar.value = displayProgress;
            }
            if (loadingText != null)
            {
                loadingText.text = $"Loading: {(displayProgress * 100f):F0}% ...";
            }

            // Once loaded and progress reaches 100%, activate the scene
            if (operation.progress >= 0.9f && displayProgress >= 0.99f)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
