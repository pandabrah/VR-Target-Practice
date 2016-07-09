using UnityEngine;
using System.Collections;

public class ModeSwitchButton : MonoBehaviour {

    public GameObject gameModeGroup;

    private GameObject arcadeShooting;
    private GameObject aimPractice;

    private bool btnSelected = false;
    private bool isTestZone = false;
    private Color buttonOffColor;
    private Color buttonOnColor;
    private TextMesh buttonTxt;

    private SteamVR_Controller.Device controller;

    void Start()
    {
        GrabModes();
        buttonOffColor = this.GetComponent<Renderer>().material.color;
        buttonOnColor = Color.white;

        buttonTxt = this.transform.GetChild(0).GetComponent<TextMesh>();
    }

    void GrabModes()
    {
        arcadeShooting = gameModeGroup.transform.Find("Arcade Shooting").gameObject;
        aimPractice = gameModeGroup.transform.Find("Aim Practice").gameObject;
    }

    void OnColliderEnter()
    {
        this.GetComponent<Renderer>().material.SetColor("_Color", buttonOnColor);
        btnSelected = true;
    }

    void OnColliderExit()
    {
        this.GetComponent<Renderer>().material.SetColor("_Color", buttonOffColor);
        btnSelected = false;
    }

    void FixedUpdate()
    {
        controller = VRControls.device;

        if (btnSelected = true && controller.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            if (arcadeShooting.activeSelf == false)
            {
                arcadeShooting.SetActive(true);
                aimPractice.SetActive(false);
                
            }

            else if (arcadeShooting.activeSelf == true)
            {
                arcadeShooting.SetActive(false);
                aimPractice.SetActive(true);
            }
        }
    }

    void UpdateText()
    {
        if (arcadeShooting.activeSelf == true)
        {
            buttonTxt.text = ("To Aim Practice");
        }
        else
            buttonTxt.text = ("To Arcade Shooting");
    }
}
