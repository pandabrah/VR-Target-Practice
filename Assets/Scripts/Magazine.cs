using UnityEngine;
using System.Collections;

public class Magazine : MonoBehaviour
{

    //private float initialY;
    //private float prevY;

    //void OnEnable()
    //{
    //    prevY = transform.localPosition.y;
    //}

    private Vector3 objPos;

    public GameObject attachJoint;
    private Vector3 attachPoint;

    void Start()
    {
        objPos = transform.position;
        attachPoint = attachJoint.transform.position;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.name == "Cube")
        {
            if (gameObject.GetComponent<FixedJoint>() == null)
            {
                FixedJoint objFJoint = gameObject.AddComponent<FixedJoint>();
                objFJoint.connectedAnchor = attachPoint;
            }
        }

        Debug.Log("Cube entered");
    }

    void OnTriggerExit(Collider col)
    {
        FixedJoint objFJoint = gameObject.GetComponent<FixedJoint>();

        if (objFJoint != null)
            Destroy(objFJoint);
    }

    void FixedUpdate()
    {

    }
}
