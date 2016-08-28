using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;

public class ButtonActions : MonoBehaviour
{

    private UnityAction modeSwitch;
    private UnityAction sceneSwitch;
    private UnityAction resetScore;

    private UpdateScoreboard scoreboardScript;

    public GameObject arcadeShooting;
    public GameObject aimPractice;

    void Awake()
    {
        modeSwitch = new UnityAction(SwitchModes);
        sceneSwitch = new UnityAction(SwitchScenes);
        resetScore = new UnityAction(ResetScore);
    }

    void OnEnable()
    {
        EventManager.StartListening("ModeSwitch", modeSwitch);
        EventManager.StartListening("SceneSwitch", sceneSwitch);
        EventManager.StartListening("ResetScore", resetScore);
    }

    void OnDisable()
    {
        EventManager.StopListening("ModeSwitch", modeSwitch);
        EventManager.StopListening("SceneSwitch", sceneSwitch);
        EventManager.StartListening("ResetScore", resetScore);
    }

    void SwitchModes()
    {
        if (arcadeShooting.activeInHierarchy != true)
        {
            arcadeShooting.SetActive(true);
            aimPractice.SetActive(false);
        }

        else if (arcadeShooting.activeInHierarchy == true)
        {
            arcadeShooting.SetActive(false);
            aimPractice.SetActive(true);
        }
    }

    void SwitchScenes()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (scene.name == "Experimenting")
            SceneManager.LoadScene("TestZone");

        else if (scene.name == "TestZone")
            SceneManager.LoadScene("Experimenting");
    }

    void ResetScore()
    {
        scoreboardScript.Reset();
    }
}
