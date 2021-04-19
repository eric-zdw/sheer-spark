using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueButton : MonoBehaviour
{
    public StagePreviewCanvas preview;
    public GameObject mainMenuCanvas;
    public GameObject stagePreviewPanel;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(LoadNextStagePreview);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadNextStagePreview()
    {
        stagePreviewPanel.gameObject.SetActive(true);
        string nextStageName = PlayerPrefs.GetString("CurrentStage", "BLUE ROOM");
        preview.LoadStagePreviewAction(nextStageName);
        mainMenuCanvas.SetActive(false);
    }
}
