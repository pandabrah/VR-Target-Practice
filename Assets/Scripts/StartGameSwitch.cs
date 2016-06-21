using UnityEngine;
using System.Collections;

public class StartGameSwitch : MonoBehaviour {

    public GameObject countdown;
    public AudioClip beepSound;

    private GameObject startButton;
    private float timerDuration;
    private AudioSource source;

    private StartGameSwitch startGameScript;

    void Start()
    {
        source = GetComponent<AudioSource>();
        startButton = gameObject.transform.Find("Start Button").gameObject;
        startGameScript = this.GetComponent<StartGameSwitch>();
    }

    void Update()
    {
        if (startButton == null)
        {
            StartCoroutine(StartCountdown());
            startGameScript.enabled = false;
        }

        else
            return;
    }

    IEnumerator StartCountdown()
    {
        int i;
        var cText = countdown.GetComponent<TextMesh>();

        yield return new WaitForSeconds(1f);
        countdown.SetActive(true);

        cText.text = ("Shoot as many targets\n as you can\n in thirty seconds");
        yield return new WaitForSeconds(5f);

        for (i = 3; i > 0; i--)
        {
            cText.text = ("Game Starts in " + i);
            source.PlayOneShot(beepSound, 1f);
            yield return new WaitForSeconds(1f);
        }

        source.pitch = 2;
        source.PlayOneShot(beepSound, 2f);

        transform.Find("SpawnZone").gameObject.SetActive(true);
        this.GetComponent<GamePlaying>().enabled = true;

        countdown.SetActive(false);
    }
}
