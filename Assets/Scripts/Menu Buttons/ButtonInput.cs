using UnityEngine;
using System.Collections;

public class ButtonInput : MonoBehaviour {

    private bool btnSelected = false;
    private Color buttonOffColor;
    private Color buttonOnColor;
    private TextMesh buttonTxt;

    private SteamVR_Controller.Device controller;

    void Start()
    {
        buttonOffColor = this.GetComponent<Renderer>().material.color;
        buttonOnColor = Color.white;

        buttonTxt = this.transform.GetChild(0).GetComponent<TextMesh>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag != "Attach Point")
            return;

        else if (col.tag == "Attach Point")
        {
            this.GetComponent<Renderer>().material.SetColor("_Color", buttonOnColor);
            btnSelected = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag != "Attach Point")
            return;

        else if (col.tag == "Attach Point")
        {
            this.GetComponent<Renderer>().material.SetColor("_Color", buttonOffColor);
            btnSelected = false;
        }
    }

    void FixedUpdate()
    {
        controller = VRControls.device;

        if (btnSelected == true && controller.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            EventManager.TriggerEvent("ModeSwitch");
        }
    }
}
