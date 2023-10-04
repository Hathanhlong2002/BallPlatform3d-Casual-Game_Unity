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
        // Bắt đầu tiến trình loading thông qua coroutine
        StartCoroutine(LoadSceneWithTiming());
    }

    IEnumerator LoadSceneWithTiming()
    {
        float loadingTime = 30.0f; // Thời gian loading mong muốn (30 giây)
        float elapsedTime = 0.0f;  // Thời gian đã trôi qua

        while (elapsedTime < loadingTime)
        {
            elapsedTime += Time.deltaTime;

            float progress = elapsedTime / loadingTime; // Tính phần trăm tương ứng

            // Cập nhật ProgressBar hoặc keo trượt và thông tin loading
            loadingBar.value = progress;
            loadingText.text = "Loading: " + (progress * 100).ToString("F0") + "% ..."; // Định dạng số nguyên trong TextMeshPro

            yield return null;
        }

        // Khi đã hoàn thành thời gian loading, tiến hành tải scene
        AsyncOperation operation = SceneManager.LoadSceneAsync("LevelSelection");
        while (!operation.isDone)
        {
            yield return null;
        }
    }
}
