using System.Collections;
using System.Collections.Generic;
using DTWorldz.SaveSystem;
using DTWorldz.ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AsyncSceneLoader : MonoBehaviour
{
    [SerializeField]
    private Text progressText;
    [SerializeField]
    private Slider slider;
    private Canvas canvas;
    private AsyncOperation operation;

    public PlayerAreaStack PlayerAreaStack;

    void Awake()
    {
        canvas = GetComponentInChildren<Canvas>();
        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(string sceneName, bool areaStackIsActive, bool save)
    {
        if (PlayerAreaStack != null)
        {
            PlayerAreaStack.IsActive = areaStackIsActive;
        }

        if (save)
        {
            var saveSystemManager = GameObject.FindObjectOfType<SaveSystemManager>();
            saveSystemManager.SaveGame();
        }
        
        UpdateUI(0);
        Time.timeScale = 0f;
        canvas.enabled = true;
        StartCoroutine(BeginLoad(sceneName));
    }

    private IEnumerator BeginLoad(string sceneName)
    {
        operation = SceneManager.LoadSceneAsync(sceneName);
        float progress = 0f;
        while (!operation.isDone)
        {
            progress = Mathf.Clamp01(operation.progress / 0.9f);
            UpdateUI(progress);

            yield return null;
        }

        UpdateUI(progress);
        Time.timeScale = 1f;
        operation = null;
        canvas.enabled = false;
    }

    private void UpdateUI(float progress)
    {
        slider.value = progress;
        progressText.text = (int)(progress * 100f) + "%";
    }
}
