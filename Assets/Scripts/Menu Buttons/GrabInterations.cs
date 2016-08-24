using UnityEngine;
using System.Collections;

public class GrabInterations : MonoBehaviour {

    private GameObject menuObj;
    private Color matColor;
    private float showZone = 200f;

    void Awake()
    {
        menuObj = this.transform.parent.parent.gameObject;
        matColor = this.GetComponent<Material>().color;
    }

    void OnTriggerEnter()
    {
        matColor.a = showZone;
        menuObj.tag = "Grabbable";
    }

    void OnTriggerExit()
    {
        matColor.a = 0f;
        menuObj.tag = "Untagged";
    }
}
