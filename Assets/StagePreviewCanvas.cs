using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StagePreviewCanvas : MonoBehaviour
{
    private bool isDoneFading = false;
    public UnityEngine.UI.Image fadePanel;
    public float fadeDuration = 3f;

    private int currentSceneIndex;

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        //LoadStagePreviewAction();
    }

    public void LoadStagePreviewAction()
    {
        StartCoroutine(FadeOut());
        StartCoroutine(LoadStagePreview(1));
    }

    private IEnumerator LoadStagePreview(int index)
    {
        AsyncOperation loadingStage = SceneManager.LoadSceneAsync(index, LoadSceneMode.Single);
        loadingStage.allowSceneActivation = false;

        //wait for fadeout and scene loading to switch scenes.
        while (!isDoneFading)
        {
            yield return new WaitForSeconds(0.2f);
        }

        // done fading out; load rest of scene.
        loadingStage.allowSceneActivation = true;
        
        // wait for new scene to load before fadein
        while (SceneManager.GetActiveScene().buildIndex == currentSceneIndex)
        {
            yield return new WaitForSeconds(0.2f);
        }

        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeOut()
    {
        float timer = fadeDuration;
        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            float a = 1f - (timer / fadeDuration);
            fadePanel.color = new Color(0f, 0f, 0f, a);
            yield return new WaitForEndOfFrame();
        }

        fadePanel.color = new Color(0f, 0f, 0f, 1f);
        isDoneFading = true;
    }
    private IEnumerator FadeIn()
    {
        float timer = fadeDuration;
        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            float a = (timer / fadeDuration);
            fadePanel.color = new Color(0f, 0f, 0f, a);
            yield return new WaitForEndOfFrame();
        }

        fadePanel.color = new Color(0f, 0f, 0f, 0f);
    }
}
