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
    public static bool holdingGun = false;
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

    private GameObject attachPointVisual;

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
        attachPoint = this.transform.GetChild(0).Find("tip").GetChild(0).GetComponent<Rigidbody>();

        SphereCollider sCollider = attachPoint.gameObject.AddComponent<SphereCollider>();
        sCollider.radius = 0.01f;
        sCollider.isTrigger = true;

        SphereCollider collider = this.gameObject.AddComponent<SphereCollider>();
        collider.radius = 0.01f;
        collider.isTrigger = true;
        collider.center = new Vector3(0f, -0.01f, 0.008f);

        if (attachPoint.tag != "Attach Point")
        {
            attachPoint.tag = "Attach Point";
        }

        //Places a visual where the attach point is
        attachPointVisual = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        attachPointVisual.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
        attachPointVisual.transform.localPosition = attachPoint.gameObject.transform.position;
        attachPointVisual.transform.SetParent(this.transform.GetChild(0).Find("tip"));

        //Check for duplicate colliders and delete them if they exist
        if (attachPointVisual.GetComponent<SphereCollider>())
        {
            Destroy(attachPointVisual.GetComponent<SphereCollider>());
        }

    }

    void InitializeHeadset()
    {
        cameraRigPrefab = this.transform.parent.transform;
        headsetCamera = this.transform.parent.Find("Camera (head)").gameObject;
    }

    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("Object Touched");
        Debug.Log("Object is " + collider);
        objectDetect = true;
        detectedObj = collider.gameObject;
    }

    void OnTriggerExit(Collider collider)
    {
        objectDetect = false;
    }

    void FixedUpdate()
    {
        device = SteamVR_Controller.Input((int)trackedController.index);

        if (objectDetect == true)
        {
            if (detectedObj.tag == ("Gun"))
                StartCoroutine(GrabGun(detectedObj));

            else if (detectedObj.tag == ("Grabbable"))
                GrabObject(detectedObj);
        }

        if (device.GetTouchDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
        {
            menuOn = menu.activeInHierarchy;
            SystemMenu(menu, menuOn);
        }
    }

    void Update()
    {
        if (holdingGun && gunInHand.GetComponent<VRShooting>().enabled == false)
            gunInHand.GetComponent<VRShooting>().enabled = true;

        //if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
        //    this.gameObject.GetComponent<Teleport>().enabled = true;
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

    void SystemMenu(GameObject menuObj, bool menuActive)
    {
        if (menuActive == false)
        {
            Ray cameraRay = new Ray(headsetCamera.transform.position, headsetCamera.transform.TransformDirection(Vector3.forward));

            menuObj.transform.position = cameraRay.GetPoint(0.5f);
            menuObj.transform.rotation = headsetCamera.transform.rotation * Quaternion.Euler(-90f, 0f, 0f);
            menuObj.SetActive(true);
        }

        else if (menuActive == true)
            menuObj.SetActive(false);
    }
}
