using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyPanel : MonoBehaviour
{
    public GameObject[] meters;
    private PlayerBehaviour player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateEnergyMeters() {
        for (int i = 0; i < 6; i++) {
            for (int j = 0; j < 6; j++) {
                if (player.powerupEnergy[i] >= (j + 1)) {
                    meters[i].transform.GetChild(j).GetComponent<UnityEngine.UI.Image>().enabled = true;
                }
                else {
                    meters[i].transform.GetChild(j).GetComponent<UnityEngine.UI.Image>().enabled = false;
                }
            }
        }

        bool whiteIsReady = true;
        foreach (int energy in player.powerupEnergy) {
            if (energy < 2) {
                whiteIsReady = false;
            }
        }
        
        if (whiteIsReady) StartCoroutine(WhiteIsReady());
        else StopCoroutine(WhiteIsReady());
    }

    private IEnumerator WhiteIsReady() {
        Color[] colors = new Color[7];
        colors[0] = new Color(1f, .25f, .25f, .4f);
        colors[1] = new Color(1f, .5f, .25f, .4f);
        colors[2] = new Color(1f, .828125f, .25f, .4f);
        colors[3] = new Color(.25f, 1f, .25f, .4f);
        colors[4] = new Color(.25f, .5f, 1f, .4f);
        colors[5] = new Color(.75f, .25f, 1f, .4f);
        colors[6] = new Color(1f, 1f, 1f, .4f);

        float t = 0f;

        while (true) {
            t = (Mathf.Sin(Time.time * 2f) + 1f) * 0.5f;
            for (int i = 0; i < 6; i++) {
                Color blendedColor = Color.Lerp(colors[i], colors[6], t);
                for (int j = 0; j < 2; j++) {
                    meters[i].transform.GetChild(j).GetComponent<UnityEngine.UI.Image>().color = blendedColor;
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator ColorIsReady(int index) {
        while (true) {
            yield return new WaitForEndOfFrame();
        }
    }
}
