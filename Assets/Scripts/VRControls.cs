using UnityEngine;
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
    private SteamVR_TrackedObject trackedController;
    public static SteamVR_Controller.Device device;
    private GameObject headsetCamera;
    private Transform cameraRigPrefab;
    private GameObject controllerTip;

    //Menu variables
    private bool menuOn = false;
    private GameObject newMenu;

    private Rigidbody attachPoint;
    private FixedJoint attachJoint;

    //Laserpointer variables
    private GameObject laserPointer;
    private float laserDistY = 0;

    void Awake()
    {
        trackedController = GetComponent<SteamVR_TrackedObject>();
    }

    void Start()
    {
        InitializeController();
        InitializeHeadset();
        InitializePointer();
    }

    void InitializeController()
    {
        SphereCollider collider = this.gameObject.AddComponent<SphereCollider>();
        collider.radius = 0.03f;
        collider.isTrigger = true;

        attachPoint = this.transform.GetChild(0).Find("tip").GetChild(0).GetComponent<Rigidbody>();
    }

    void InitializeHeadset()
    {
        cameraRigPrefab = this.transform.parent.transform;
    }

    void InitializePointer()
    {
        controllerTip = this.transform.GetChild(0).Find("tip").gameObject;

        laserPointer = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        laserPointer.transform.parent = controllerTip.transform;
        laserPointer.transform.localScale = new Vector3(0.1f, laserDistY, 0.1f);
        laserPointer.transform.localPosition = Vector3.zero;

        SphereCollider endPoint = laserPointer.AddComponent<SphereCollider>();
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
        Debug.Log(cameraRigPrefab);

        if (holdingGun && gunInHand.GetComponent<VRShooting>().enabled == false)
        {
            gunInHand.GetComponent<VRShooting>().enabled = true;
        }

        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
        {
            StartCoroutine(Teleport());
        }
    }
    IEnumerator Teleport()
    {
        yield return null;
        Vector3 originalPos = cameraRigPrefab.transform.position;
        Debug.Log(originalPos);

        Vector3 tpPoint = TPRay();

        while (tpPoint == originalPos)
        {
            tpPoint = TPRay();
        }

        if (tpPoint != originalPos)
        {
            if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
            {
                originalPos = tpPoint;
            }
        }
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
