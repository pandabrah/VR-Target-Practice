﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class VRControls : MonoBehaviour
{
    public GameObject bulletDecal;
    public GameObject targets;
    //public TargetInteraction targetScript;
    public GameObject menu;

    public float velocityMult = 3;

    public AudioClip gunShotSound;

    private bool objectDetect;
    private bool holdingGun;
    private GameObject gunInHand;
    private GameObject detectedObj;

    private SteamVR_TrackedObject trackedController;
    private SteamVR_Controller.Device device;
    private Rigidbody attachPoint;
    private Rigidbody menuAttachPoint;

    private FixedJoint attachJoint;
    private FixedJoint menuJoint;
    private Animation anim;

    private bool menuOn = false;
    private GameObject newMenu;

    private AudioSource source;

    void Awake()
    {
        trackedController = GetComponent<SteamVR_TrackedObject>();
    }

    void Start()
    {
        InitializeController();
    }

    void InitializeController()
    {
        SphereCollider collider = this.gameObject.AddComponent<SphereCollider>();
        collider.radius = 0.03f;
        collider.isTrigger = true;

        attachPoint = transform.GetChild(0).Find("tip").GetChild(0).GetComponent<Rigidbody>();
        menuAttachPoint = transform.GetChild(0).Find("button").GetChild(0).GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("Object Touched");
        objectDetect = true;
        detectedObj = collider.gameObject;
    }

    void OnTriggerExit(Collider collider)
    {
        objectDetect = false;
        Debug.Log("No Object Detected");
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
        if (holdingGun)
        {
            shootGun(gunInHand);
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
        yield return new WaitForFixedUpdate();
        if (attachJoint == null && device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            obj.transform.position = attachPoint.transform.position + new Vector3(0.03f, 0f, 0.05f);
            obj.transform.rotation = attachPoint.transform.rotation * Quaternion.Euler(45f, 0f, 0f);
            attachJoint = obj.AddComponent<FixedJoint>();
            attachJoint.connectedBody = attachPoint;

            gunInHand = obj.gameObject;
            holdingGun = true;

            if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
            {
                yield return null;
            }
        }

        else if (attachJoint != null && device.GetTouchDown(SteamVR_Controller.ButtonMask.Grip))
        {
            Destroy(attachJoint);
            attachJoint = null;
            Rigidbody objRB = gunInHand.GetComponent<Rigidbody>();

            Transform origin = trackedController.origin ? trackedController.origin : trackedController.transform.parent;
            if (origin != null)
            {
                objRB.velocity = origin.TransformVector(device.velocity) * velocityMult;
                objRB.angularVelocity = origin.TransformVector(device.angularVelocity) * velocityMult/3;
            }
            else
            {
                objRB.velocity = device.velocity * velocityMult;
                objRB.angularVelocity = device.angularVelocity * velocityMult/3;
            }

            objRB.maxAngularVelocity = objRB.angularVelocity.magnitude * velocityMult;

            holdingGun = false;
            gunInHand = null;
        }
    }

    void shootGun(GameObject gun)
    {
        Ray targetRay = new Ray(gun.transform.Find("Aim").position, gun.transform.Find("Aim").forward);
        RaycastHit hit;

        Quaternion gunRotation = gun.transform.rotation;

        //Gun firing interactions
        if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            source = gun.GetComponent<AudioSource>();
            source.PlayOneShot(gunShotSound, 1f);


            //Check if gun has animation, if it does play it every trigger pull
            if (gun.GetComponent<Animation>())
            {
                Animation anim = gun.GetComponent<Animation>();
                anim.Play("Shooting");
            }

            if (Physics.Raycast(targetRay, out hit))
            {
                if (hit.collider.tag != ("Target"))
                {
                    Instantiate(bulletDecal, hit.point, Quaternion.identity);
                }

                //If target is the start button, do not add to hit counter
                else if (hit.collider.tag == ("Target") && hit.collider.name == ("Start Button"))
                {
                    StartCoroutine(hit.collider.gameObject.GetComponent<TargetInteraction>().StartButtonInteraction());
                }

                //If target is shot, update score, break target, and respawn target
                else if (hit.collider.tag == ("Target"))
                {
                    StartCoroutine(hit.collider.gameObject.GetComponent<TargetInteraction>().TargetHitReset());
                    TargetHitCounter.targetHitCount += 1;
                }
            }

            //Confirm trigger is released before allowing another shot
            if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
            {
                return;
            }
        }

        //Check if there is a scoreboard and timer object attached to gun
        if (gun.transform.Find("Scoreboard").gameObject != null && gun.transform.Find("Timer").gameObject != null)
        {

            //If the left side of the gun is rotated towards the user, show the scoreboard and timer
            if (gunRotation.eulerAngles.y >= 260 && gunRotation.eulerAngles.y <= 300)
            {
                gun.transform.Find("Scoreboard").gameObject.SetActive(true);
                gun.transform.Find("Timer").gameObject.SetActive(true);
            }

            else
            {
                gun.transform.Find("Scoreboard").gameObject.SetActive(false);
                gun.transform.Find("Timer").gameObject.SetActive(false);
            }
        }
    }

    void SystemMenu(GameObject menuObj)
    {

        if (menuOn == false && newMenu == null)
        {
            menuObj.transform.position = menuAttachPoint.transform.position + new Vector3(0f, 0f, -0.01f);
            menuObj.transform.rotation = menuAttachPoint.transform.rotation * Quaternion.Euler(90f, 0f, 0f);

            newMenu = (GameObject)Instantiate(menuObj, menuObj.transform.position, menuObj.transform.rotation);

            menuJoint = newMenu.AddComponent<FixedJoint>();
            menuJoint.connectedBody = menuAttachPoint;

            menuOn = true;
        }

        else if (menuOn == true && newMenu != null)
        {
            Destroy(menuJoint);
            menuJoint = null;
            DestroyObject(newMenu);

            menuOn = false;
        }
    }
}
