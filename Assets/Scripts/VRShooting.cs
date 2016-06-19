﻿using UnityEngine;
using System.Collections;

public class VRShooting : MonoBehaviour
{
    public GameObject bulletDecal;
    public GameObject bulletShell;

    private GameObject gun;
    private int targetHitCounter;
    private bool holdingGun;
    private bool gunCanPickUp;
    private static int ammoCount;
    private Vector3 gunEjectChamber;

    private int HK45Ammo;
    private int GlockAmmo;

    private Animation anim;

    void Start()
    {
        gun = this.gameObject;

        HK45Ammo = 12;
        GlockAmmo = 17;

        if (this.name == ("HK45"))
        {
            ammoCount = HK45Ammo;
        }

        else
            ammoCount = GlockAmmo;
    }

    void FixedUpdate()
    {
        Ray targetRay = new Ray(gun.transform.Find("Aim").position, gun.transform.Find("Aim").forward);
        RaycastHit hit;

        if (Physics.Raycast(targetRay, out hit))
        {

            //if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
            //{
            //    if (gun.GetComponent<Animation>())
            //    {
            //        Animation anim = gun.GetComponent<Animation>();
            //        anim.Play("Shooting");
            //    }

            //    if (Physics.Raycast(targetRay, out hit))
            //    {
            //        if (hit.collider.tag != ("Target"))
            //        {
            //            Instantiate(bulletDecal, hit.point, Quaternion.identity);
            //        }


            //        //If target is shot, update score, break target, and respawn target
            //        else if (hit.collider.tag == ("Target"))
            //        {
            //            StartCoroutine(TargetHitReset(hit.collider.gameObject));
            //        }
            //    }
            //}
            //else
            //    return;
        }
    }

    //void UpdateHitCounter()
    //{
    //    targetHitText.text = "Targets Hit: " + targetHitCounter.ToString();
    //}

    void BulletEject(GameObject bullet, GameObject gun)
    {
        float randVelocity = Random.Range(200.0f, 300.0f);

        gunEjectChamber = gun.transform.TransformPoint(1.0f, 1.0f, 1.0f) + new Vector3(-1.0f, -1.0f, -0.7f);
        GameObject bulletShell = Instantiate(bullet, gunEjectChamber, Quaternion.Euler(0f, 180f, 0f)) as GameObject;
        bulletShell.AddComponent<BulletholeDecay>();

        bulletShell.AddComponent<Rigidbody>();
        bulletShell.GetComponent<Rigidbody>().AddRelativeForce(randVelocity, randVelocity, 0f);
        bulletShell.GetComponent<Rigidbody>().AddRelativeTorque(10000.0f, 10000.0f, 10000.0f);
    }
}
