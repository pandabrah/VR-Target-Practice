using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GamePlaying : MonoBehaviour
{

    public GameObject timer;
    public GameObject startButton;
    public float initialTime = 60f;

    public float updatedTime;
    private StartGameSwitch startGameScript;
    private GamePlaying gamePlayingScript;
    private TargetSpawner targetSpawnerScript;

    private List<GameObject> listOfTargets;
    private GameObject tg;

    void Start()
    {
        startGameScript = this.GetComponent<StartGameSwitch>();
        gamePlayingScript = this.GetComponent<GamePlaying>();
        targetSpawnerScript = this.transform.Find("SpawnZone").gameObject.GetComponent<TargetSpawner>();
    }

    void OnEnable()
    {
        updatedTime = initialTime;
    }

    void Update()
    {
        updatedTime -= Time.deltaTime;

        if (updatedTime > 0)
        {
            timer.GetComponent<TextMesh>().text = ("Time Left: " + updatedTime.ToString("0"));
        }

        else if (updatedTime < 0)
        {
            updatedTime = 0;
            timer.GetComponent<TextMesh>().text = ("Time Left: 0");

            this.transform.Find("Start Button").gameObject.SetActive(true);

            listOfTargets = targetSpawnerScript.spawnedTargets;
            ClearTargets();

            this.transform.Find("SpawnZone").gameObject.SetActive(false);
            startGameScript.enabled = true;
            gamePlayingScript.enabled = false;
        }
    }

    void ClearTargets()
    {
        for (int i = listOfTargets.Count - 1; i >= 0; i--)
        {
            if (listOfTargets != null)
            {
                Destroy(listOfTargets[i].gameObject);
                listOfTargets.RemoveAt(i);
            }
        }

        //foreach (GameObject tgt in listOfTargets)
        //    Destroy(tgt.gameObject);
        //    //tgt.SetActive(false);
        //listOfTargets.Clear();
    }
}
