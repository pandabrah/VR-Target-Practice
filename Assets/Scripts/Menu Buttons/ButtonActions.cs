using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class ButtonActions : MonoBehaviour {

    private UnityAction modeSwitch;

    public GameObject arcadeShooting;
    public GameObject aimPractice;

    void Awake()
    {
        modeSwitch = new UnityAction(SwitchModes);
    }

    void OnEnable()
    {
        EventManager.StartListening("ModeSwitch", modeSwitch);
    }

    void OnDisable()
    {
        EventManager.StopListening("ModeSwitch", modeSwitch);
    }

    void SwitchModes()
    {
        if (arcadeShooting.activeInHierarchy != true)
        {
            arcadeShooting.SetActive(true);
            aimPractice.SetActive(false);

            Debug.Log("Mode Switched");
        }

        else if (arcadeShooting.activeInHierarchy == true)
        {
            arcadeShooting.SetActive(false);
            aimPractice.SetActive(true);
        }
    }
}
