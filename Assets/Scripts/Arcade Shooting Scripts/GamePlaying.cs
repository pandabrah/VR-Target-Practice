using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GamePlaying : MonoBehaviour {

    public GameObject timer;
    public GameObject startButton;
    public float initialTime = 60f;

    public float updatedTime;
    private StartGameSwitch startGameScript;
    private GamePlaying gamePlayingScript;

    private List<GameObject> listOfTargets;

    void Start()
    {
        startGameScript = this.GetComponent<StartGameSwitch>();
        gamePlayingScript = this.GetComponent<GamePlaying>();
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

        else if (updatedTime <= 0)
        {
            updatedTime = 0;
            timer.GetComponent<TextMesh>().text = ("Time Left: 0");

            this.transform.Find("Start Button").gameObject.SetActive(true);

            listOfTargets = this.transform.Find("SpawnZone").gameObject.GetComponent<TargetSpawner>().spawnedTargets;
            listOfTargets.Find(<GameObject>);
            ClearTargets(tgt)

            this.transform.Find("SpawnZone").gameObject.SetActive(false);
            startGameScript.enabled = true;
            gamePlayingScript.enabled = false;
        }
    }

    void ClearTargets(GameObject t)
    {
        foreach (GameObject tgt in spawnedTargets)
        {
            Destroy(t);
        }

        spawnedTargets.RemoveAll((tgt) => tgt == null);
    }
}
