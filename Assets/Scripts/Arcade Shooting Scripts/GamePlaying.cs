using UnityEngine;
using System.Collections;

public class GamePlaying : MonoBehaviour {

    public GameObject timer;
    public GameObject startButton;
    public float initialTime = 60f;

    public float updatedTime;
    private Vector3 startButtonSpawn;
    private Quaternion startButtonRotation;
    private StartGameSwitch startGameScript;
    private GamePlaying gamePlayingScript;

    void Start()
    {
        startButtonSpawn = new Vector3(-1.1f, 1.69f, -3.82f);
        startButtonRotation = Quaternion.Euler(0f, 0f, 0f);

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



            Animation anim = this.transform.Find("Start Button").gameObject.GetComponent<Animation>();
            anim["TargetBreak"].time = 0;
            this.transform.Find("Start Button").gameObject.SetActive(true);

            //GameObject go = (GameObject)Instantiate(startButton, startButtonSpawn, startButtonRotation);
            //go.transform.parent = gameObject.transform;
            //go.transform.name = ("Start Button");

            this.transform.Find("SpawnZone").gameObject.SetActive(false);
            startGameScript.enabled = true;
            gamePlayingScript.enabled = false;
        }
    }
}
