using UnityEngine;
using System.Collections;

public class StartGameSwitch : MonoBehaviour {

    public GameObject startButton;
    public GameObject countdown;
    public AudioClip beepSound;

    private float timerDuration;
    private AudioSource source;

    void Awake()
    {
        source = GetComponent<AudioSource>();
        
    }

    void Update()
    {
        if (startButton == null)
        {
            StartCoroutine(StartCountdown());
            this.GetComponent<StartGameSwitch>().enabled = false;
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
