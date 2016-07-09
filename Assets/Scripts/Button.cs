using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour {

    private bool btnSelected = false;
    private bool isTestZone = false;

    private SteamVR_Controller.Device controller;
    private Color buttonOffColor;
    private Color buttonOnColor;
    private TextMesh buttonTxt;


    void Start()
    {
        buttonOffColor = this.GetComponent<Renderer>().material.color;
        buttonOnColor = Color.white;
    }

    void OnTriggerEnter()
    {
        this.GetComponent<Renderer>().material.SetColor("_Color", buttonOnColor);
        btnSelected = true;
    }

    void OnTriggerExit()
    {
        this.GetComponent<Renderer>().material.SetColor("_Color", buttonOffColor);
        btnSelected = false;
    }

    void FixedUpdate()
    {
        controller = VRControls.device;
        buttonTxt = this.transform.GetChild(0).GetComponent<TextMesh>();

        UpdateScene();
        UpdateText();

        if (btnSelected = true && controller.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            if (isTestZone == false)
            {
                SceneManager.LoadScene("TestZone");
            }

            else if (isTestZone == true)
            {
                SceneManager.LoadScene("Experimenting");
            }
        }

    }

    void UpdateScene()
    {
        if (SceneManager.GetActiveScene().name == ("TestZone"))
        {
            isTestZone = true;
        }

        else
            isTestZone = false;
    }

    void UpdateText()
    {
        if (isTestZone == false)
        {
            buttonTxt.text = ("To TestZone");
        }

        else if (isTestZone == true)
        {
            buttonTxt.text = ("To Experimenting");
        }
    }
}
