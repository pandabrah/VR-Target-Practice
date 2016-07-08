﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class VRControls : MonoBehaviour
{
    public GameObject targets;
    public GameObject menu;

    public float velocityMult = 3;

    //Gun and object variables
    private bool objectDetect;
    private bool holdingGun;
    private GameObject gunInHand;
    private GameObject detectedObj;

    //SteamVR references
    public static SteamVR_Controller.Device device;
    private GameObject controllerTip;
    private SteamVR_TrackedObject trackedController;
    private GameObject headsetCamera;
    private Transform cameraRigPrefab;

    //Menu variables
    private bool menuOn = false;
    private GameObject newMenu;

    private Rigidbody attachPoint;
    private FixedJoint attachJoint;

    void Awake()
    {
        trackedController = GetComponent<SteamVR_TrackedObject>();
    }

    void Start()
    {
        InitializeController();
        InitializeHeadset();
    }

    void InitializeController()
    {
        SphereCollider collider = this.gameObject.AddComponent<SphereCollider>();
        collider.radius = 0.01f;
        collider.isTrigger = true;

        attachPoint = this.transform.GetChild(0).Find("tip").GetChild(0).GetComponent<Rigidbody>();
    }

    void InitializeHeadset()
    {
        cameraRigPrefab = this.transform.parent.transform;
    }

    void OnTriggerEnter(Collider collider)
    {
        //Debug.Log("Object Touched");
        objectDetect = true;
        detectedObj = collider.gameObject;
    }

    void OnTriggerExit(Collider collider)
    {
        objectDetect = false;
        //Debug.Log("No Object Detected");
    }

    void FixedUpdate()
    {
        device = SteamVR_Controller.Input((int)trackedController.index);

        if (objectDetect == true)
        {
            if (detectedObj.tag == ("Gun"))
            {
                StartCoroutine(GrabGun(detectedObj));
            }

            else if (detectedObj.tag == ("Grabbable"))
            {
                GrabObject(detectedObj);
            }
        }

        if (device.GetTouchDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
        {
            SystemMenu(menu);
        }

    }

    void Update()
    {
        if (holdingGun && gunInHand.GetComponent<VRShooting>().enabled == false)
        {
            gunInHand.GetComponent<VRShooting>().enabled = true;
        }

        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
        {
            this.gameObject.GetComponent<Teleport>().enabled = true;
        }
    }

    void GrabObject(GameObject obj)
    {
        if (attachJoint == null && device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            obj.transform.position = attachPoint.transform.position;
            obj.transform.rotation = attachPoint.transform.rotation;
            attachJoint = obj.AddComponent<FixedJoint>();
            attachJoint.connectedBody = attachPoint;
        }

        else if (attachJoint != null && device.GetTouchDown(SteamVR_Controller.ButtonMask.Grip))
        {
            Destroy(attachJoint);
            attachJoint = null;
            Rigidbody objRB = obj.GetComponent<Rigidbody>();

            Transform origin = trackedController.origin ? trackedController.origin : trackedController.transform.parent;
            if (origin != null)
            {
                objRB.velocity = origin.TransformVector(device.velocity) * velocityMult;
                objRB.angularVelocity = origin.TransformVector(device.angularVelocity) * velocityMult;
            }
            else
            {
                objRB.velocity = device.velocity * velocityMult;
                objRB.angularVelocity = device.angularVelocity * velocityMult;
            }

            objRB.maxAngularVelocity = objRB.angularVelocity.magnitude * velocityMult;
        }
    }

    IEnumerator GrabGun(GameObject obj)
    {
        yield return null;
        if (attachJoint == null && device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            obj.transform.position = attachPoint.transform.position;
            obj.transform.rotation = attachPoint.transform.rotation * Quaternion.Euler(45f, 0f, 0f);
            attachJoint = obj.AddComponent<FixedJoint>();
            attachJoint.connectedBody = attachPoint;

            gunInHand = obj.gameObject;
            holdingGun = true;
        }

        else if (attachJoint != null && device.GetTouchDown(SteamVR_Controller.ButtonMask.Grip))
        {
            Rigidbody objRB = gunInHand.GetComponent<Rigidbody>();

            Transform origin = trackedController.origin ? trackedController.origin : trackedController.transform.parent;
            if (origin != null)
            {
                objRB.velocity = origin.TransformVector(device.velocity) * velocityMult;
                objRB.angularVelocity = origin.TransformVector(device.angularVelocity) * velocityMult;
            }
            else
            {
                objRB.velocity = device.velocity * velocityMult;
                objRB.angularVelocity = device.angularVelocity * velocityMult;
            }

            objRB.maxAngularVelocity = objRB.angularVelocity.magnitude * velocityMult;

            Destroy(attachJoint);
            attachJoint = null;

            holdingGun = false;

            gunInHand.GetComponent<VRShooting>().enabled = false;
            gunInHand = null;
        }
    }

    void SystemMenu(GameObject menuObj)
    {
        Ray cameraRay = new Ray(headsetCamera.transform.position, headsetCamera.transform.TransformDirection(Vector3.forward));
        Vector3 menuAppearPoint = cameraRay.GetPoint(1f);

        if (menuOn == false && newMenu == null)
        {
            menuObj.transform.position = menuAppearPoint;
            Quaternion menuObjRotation = menuObj.transform.rotation;
            menuObjRotation = headsetCamera.transform.rotation * Quaternion.Euler(-90f, 0f, 0f);
            menuObjRotation.z = 0f;

            newMenu = (GameObject)Instantiate(menuObj, menuObj.transform.position, menuObj.transform.rotation);

            menuOn = true;

            Debug.Log("Menu Spawn attempted");
        }

        else if (menuOn == true && newMenu != null)
        {
            DestroyObject(newMenu);

            menuOn = false;
        }
    }
}
