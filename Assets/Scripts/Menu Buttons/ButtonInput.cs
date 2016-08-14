using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ButtonInput : MonoBehaviour {

    private bool btnSelected = false;
    public static bool isTestZone = false;
    private Color buttonOffColor;
    private Color buttonOnColor;
    private TextMesh buttonTxt;

    private SteamVR_Controller.Device controller;

    private VRControls controlScript;

    public GameObject arcadeShooting;

    void Start()
    {
        buttonOffColor = this.GetComponent<Renderer>().material.color;
        buttonOnColor = Color.white;

        buttonTxt = this.transform.GetChild(0).GetComponent<TextMesh>();
    }

    void OnTriggerEnter(Collider col)
    {
        Debug.Log("Collider is " + col);

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

        UpdateText();
        UpdateScene();

        if (btnSelected == true && controlScript.holdingGun == false)
        {
            if (controller.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
                if (this.name == "Mode Button")
                    EventManager.TriggerEvent("ModeSwitch");
                else if (this.name == "Scene Button")
                    EventManager.TriggerEvent("SceneSwitch");
                else
                    return;
        }
    }

    void UpdateScene()
    {
        if (SceneManager.GetActiveScene().name == ("TestZone"))
            isTestZone = true;

        else
            isTestZone = false;
    }

    void UpdateText()
    {
        if (this.name == "Mode Button")
        {
        if (arcadeShooting.activeInHierarchy == true)
            buttonTxt.text = ("To Aim Practice");

        else
            buttonTxt.text = ("To Arcade Shooting");
        }

        if (this.name == "Scene Button")
        {
            if (isTestZone == false)
                buttonTxt.text = ("To TestZone");

            else if (isTestZone == true)
                buttonTxt.text = ("To Experimenting");
        }
    }
}
