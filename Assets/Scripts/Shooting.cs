﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Shooting : MonoBehaviour
{

    public Texture bulletDecal;
    public Text targetHitText;
    private GUIStyle guiFont = new GUIStyle();

    private int targetHitCounter = 0;
    private bool holdingGun = false;
    private bool gunCanPickUp = false;
    private int ammoCount = 17;
    private GameObject heldGun = null;

    void Awake()
    {
        UpdateHitCounter();
    }

    void FixedUpdate()
    {
        RaycastHit hit;
        Ray targetRay = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 100, Color.green);

        if (holdingGun == true && Input.GetKeyDown(KeyCode.E))
        {
            DropWeapon(heldGun);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetAmmoCount(ammoCount);
        }

        if (Physics.Raycast(targetRay, out hit))
        {
            if (hit.collider.tag == "Gun")
            {
                gunCanPickUp = true;
                if (holdingGun == false && Input.GetKeyDown(KeyCode.E))
                {
                    PickUpWeapon(hit.collider.gameObject);
                }
            }

            else if (hit.collider.tag != "Gun")
            {
                gunCanPickUp = false;
            }

            if (holdingGun == true && Input.GetButtonDown("Fire1"))
            {
                Debug.Log("Object Hit: " + hit.collider.gameObject.name);

                UpdateAmmoCount(ammoCount);
                if (ammoCount == 0)
                {
                    return;
                }

                else if (hit.collider.tag != ("Target"))
                {
                    Instantiate(bulletDecal, hit.point, hit.transform.localRotation);
                }

                else if (hit.collider.tag == ("Target"))
                {
                    StartCoroutine(TargetHitReset(hit.collider.gameObject));
                    ++targetHitCounter;
                    UpdateHitCounter();
                }
            }
        }
    }

    void UpdateAmmoCount(int ammo)
    {
        ammo -= ammo;
    }

    void ResetAmmoCount(int ammo)
    {
        ammo = 17;
    }

    void PickUpWeapon(GameObject gun)
    {
        Destroy(gun.GetComponent<Rigidbody>());
        gun.transform.SetParent(Camera.main.transform);
        holdingGun = true;
        heldGun = gun;

        //Position for original "detailed" model
        //gun.transform.localPosition = new Vector3(0.48f, -0.43f, 0.84f);
        //gun.transform.localRotation = Quaternion.Euler(0, 270, 0);

        //Position for simple animated model
        gun.transform.localPosition = new Vector3(0.6f, -0.5f, 1.1f);
        gun.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    void DropWeapon(GameObject gun)
    {
        gun.AddComponent<Rigidbody>();
        Camera.main.transform.DetachChildren();
        holdingGun = false;
    }

    void UpdateHitCounter()
    {
        targetHitText.text = "Targets Hit: " + targetHitCounter.ToString();
    }

    IEnumerator TargetHitReset(GameObject target)
    {
        float duration = 10.0f;

        if (target.name == ("TargetUp"))
        {
            Vector3 startPosition = target.transform.position;
            Vector3 endPosition = new Vector3(target.transform.position.x, 0.4446654f, target.transform.position.z);

            for (float i = 0; i < duration; i += Time.deltaTime)
            {
                Vector3 newPosition = Vector3.Lerp(startPosition, endPosition, i / duration);
                target.transform.position = newPosition;
            }

            yield return new WaitForSeconds(2.0f);

            for (float i = 0; i < duration; i += Time.deltaTime)
            {
                Vector3 newPosition = Vector3.Lerp(endPosition, startPosition, i / duration);
                target.transform.position = newPosition;
            }
        }

        else if (target.name == ("TargetSide"))
        {
            Vector3 startPosition = target.transform.position;
            Vector3 endPosition = new Vector3(-6.398f, target.transform.position.y, target.transform.position.z);

            for (float i = 0; i < duration; i += Time.deltaTime)
            {
                Vector3 newPosition = Vector3.Lerp(startPosition, endPosition, i / duration);
                target.transform.position = newPosition;
            }

            yield return new WaitForSeconds(2.0f);

            for (float i = 0; i < duration; i += Time.deltaTime)
            {
                Vector3 newPosition = Vector3.Lerp(endPosition, startPosition, i / duration);
                target.transform.position = newPosition;
            }
        }
    }

    void OnGUI()
    {
        guiFont.fontSize = 20;
        float xMin = (Screen.width / 2);
        float yMin = (Screen.height / 1.5f);
        if (gunCanPickUp)
            GUI.Label(new Rect(xMin, yMin, 500, 200), "Press E to pick up gun", guiFont);
    }
}
