using UnityEngine;
using System.Collections;

public class Teleport : MonoBehaviour
{

    //SteamVR references
    private GameObject cameraRigPrefab;
    private SteamVR_Controller.Device controller;
    private GameObject controllerTip;

    //Laserpointer variables
    private GameObject laserPointer;
    public float laserDistY = 0.1f;


    void Start()
    {
        //InitializePointer();

        cameraRigPrefab = this.transform.parent.gameObject;
        controller = VRControls.device;
        controllerTip = this.transform.GetChild(0).Find("tip").gameObject;
    }

    void Update()
    {
        //Update laserpointer
        Debug.Log(controller);
        Debug.DrawRay(controllerTip.transform.position, controllerTip.transform.TransformDirection(Vector3.forward));

        if (controller.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            Vector3 originalPos = cameraRigPrefab.transform.position;
            Debug.Log(originalPos);

            Vector3 tpPoint = TPRay();

            Debug.Log(tpPoint);
            if (tpPoint != originalPos)
            {
                originalPos = tpPoint;
                cameraRigPrefab.transform.position = originalPos;
            }

            Debug.Log(originalPos);
            this.transform.GetComponent<Teleport>().enabled = false;
        }

        if (controller.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
        {
            this.transform.GetComponent<Teleport>().enabled = false;
        }
    }

    void InitializePointer()
    {
        controllerTip = this.transform.GetChild(0).Find("tip").gameObject;

        laserPointer = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        laserPointer.transform.parent = controllerTip.transform;
        laserPointer.transform.localScale = new Vector3(0.002f, laserDistY, 0.002f);
        laserPointer.transform.localPosition = Vector3.zero;
        laserPointer.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);

        SphereCollider endPoint = laserPointer.AddComponent<SphereCollider>();
    }

    Vector3 TPRay()
    {
        Ray tpRay = new Ray(controllerTip.transform.position, controllerTip.transform.TransformDirection(Vector3.forward));
        RaycastHit hit;

        if (Physics.Raycast(tpRay, out hit))
        {
            return hit.point;
        }

        else
            return cameraRigPrefab.transform.position;
    }
}
