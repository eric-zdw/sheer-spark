using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreviewManager : MonoBehaviour
{
    private bool isDoneFading = false;
    public UnityEngine.UI.Image fadePanel;
    public float fadeDuration = 3f;

    private void Start()
    {
        StartCoroutine(FadeOut());
        StartCoroutine(LoadStagePreview(0));
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
}
