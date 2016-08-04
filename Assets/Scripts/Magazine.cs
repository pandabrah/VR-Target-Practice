using UnityEngine;
using System.Collections;

public class Magazine : MonoBehaviour
{

    void OnTriggerEnter(Collider col)
    {
        if (col.name == "Magazine")
        {
            if (!col.GetComponent<FixedJoint>())
            {
                col.gameObject.AddComponent<FixedJoint>();
            }


            if (col.GetComponent<FixedJoint>())
            {
                FixedJoint fJoint = col.GetComponent<FixedJoint>();

                fJoint.connectedBody = this.gameObject.GetComponent<Rigidbody>();
                fJoint.connectedAnchor = this.transform.position;
            }
        }

        else
            return;
        Debug.Log("Cube entered");
    }

    void FixedUpdate()
    {

    }
}
