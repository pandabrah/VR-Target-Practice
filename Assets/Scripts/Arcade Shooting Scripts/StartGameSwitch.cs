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
        startGameScript = this.GetComponent<StartGameSwitch>();
        startButton = transform.Find("Start Button").gameObject;
    }

    void OnEnable()
    {
        AnimationClip anim = this.transform.Find("Start Button").gameObject.GetComponent<Animation>().GetClip("TargetBreak");
        anim.SampleAnimation(this.transform.Find("Start Button").gameObject, 0);
    }

    void Update()
    {

        //Debug.Log(startButton);
        if (startButton.activeSelf == false)
        {
            StartCoroutine(StartCountdown());

            TargetHitCounter.targetHitCount = 0;

            startGameScript.enabled = false;
        }

        else
            return;
    }

    IEnumerator StartCountdown()
    {
        var cText = countdown.GetComponent<TextMesh>();

        yield return new WaitForSeconds(1f);
        countdown.SetActive(true);

        cText.text = ("Shoot as many targets\n as you can\n in thirty seconds");
        yield return new WaitForSeconds(5f);

        for (int i = 3; i > 0; i--)
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
