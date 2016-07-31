using UnityEngine;
using System.Collections;

public class Magazine : MonoBehaviour {

    //private float initialY;
    //private float prevY;

    //void OnEnable()
    //{
    //    prevY = transform.localPosition.y;
    //}

    private Vector3 objPos;

    void Start()
    {
        objPos = transform.position;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.name == "Cube")
        {
            transform.SetParent(col.transform);
        }

        Debug.Log("Cube entered");
    }

    void OnTriggerExit(Collider col)
    {
        if (transform.parent)
        {
            transform.SetParent(null);
        }
    }

    void FixedUpdate()
    {
        if (transform.parent)
        {
            //objPos = transform.localPosition;
            //initialY = prevY;

            objPos.x = 0.2f;


            //objPos = new Vector3(0f, locY, locZ);

            //prevY = objPos.y;
            //float deltaY = (prevY - initialY) / Time.deltaTime;
        }
    }
}
