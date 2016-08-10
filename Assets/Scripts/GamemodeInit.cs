using UnityEngine;
using System.Collections;

public class GamemodeInit : MonoBehaviour {

    public GameObject arcadeShooting;
    public GameObject aimPractice;

    void Update()
    {
        arcadeShooting = this.transform.Find("Arcade Shooting").gameObject;
        aimPractice = this.transform.Find("Aim Practice").gameObject;
    }
}