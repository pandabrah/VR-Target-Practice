using UnityEngine;
using System.Collections;

public class GamePlaying : MonoBehaviour {

    public GameObject timer;
    public float initialTime = 60f;

    public float updatedTime;

    void Start()
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

            transform.Find("SpawnZone").gameObject.SetActive(false);
            this.GetComponent<StartGameSwitch>().enabled = true;
            this.GetComponent<GamePlaying>().enabled = false;
        }
    }
}
