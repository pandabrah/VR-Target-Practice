using UnityEngine;
using System.Collections;

public class ModeSwitchButton : MonoBehaviour {
    
    private bool btnSelected = false;
    private Color buttonOffColor;
    private Color buttonOnColor;
    private TextMesh buttonTxt;

    private SteamVR_Controller.Device controller;

    public GameObject arcadeShooting;
    public GameObject aimPractice;

    void Start()
    {
        buttonOffColor = this.GetComponent<Renderer>().material.color;
        buttonOnColor = Color.white;

        buttonTxt = this.transform.GetChild(0).GetComponent<TextMesh>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag != "VR Controller")
            return;

        else if (col.tag == "VR Controller")
        {
            this.GetComponent<Renderer>().material.SetColor("_Color", buttonOnColor);
            btnSelected = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag != "VR Controller")
            return;

        else if (col.tag == "VR Controller")
        {
            this.GetComponent<Renderer>().material.SetColor("_Color", buttonOffColor);
            btnSelected = false;
        }
    }

    void FixedUpdate()
    {
        controller = VRControls.device;
        UpdateText();

        if (btnSelected == true && controller.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
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
        if (arcadeShooting.activeInHierarchy == true)
            buttonTxt.text = ("To Aim Practice");

        else
            buttonTxt.text = ("To Arcade Shooting");
    }
}
