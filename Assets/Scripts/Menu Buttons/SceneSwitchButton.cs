using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneSwitchButton : MonoBehaviour {

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
        Debug.Log("btnSelected" + btnSelected);

        controller = VRControls.device;

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
