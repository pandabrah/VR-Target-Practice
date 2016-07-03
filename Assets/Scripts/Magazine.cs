using UnityEngine;
using System.Collections;

public class Magazine : MonoBehaviour {

    //private float initialY;
    //private float prevY;

    //void OnEnable()
    //{
    //    prevY = transform.localPosition.y;
    //}

    void FixedUpdate()
    {
        Vector3 objPos = transform.localPosition;
        //initialY = prevY;

        objPos = new Vector3(0f, objPos.y, objPos.z);
        

        //objPos = new Vector3(0f, locY, locZ);

        //prevY = objPos.y;
        //float deltaY = (prevY - initialY) / Time.deltaTime;
    }
}
