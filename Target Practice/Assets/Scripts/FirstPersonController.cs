using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour {

    public float movementSpeed = 8.0f;
    public float mouseSensitivity = 1f;
    public float upDownRange = 60.0f;
    public float jumpSpeed = 3.0f;
    float rotUpDown = 0;
    public Texture2D crosshair;

    float verticalVelocity = 0;
    CharacterController characterController;

	// Use this for initialization
	void Start () {
        Screen.lockCursor = true;
        characterController = GetComponent<CharacterController>();
	}
	
    void OnGUI()
    {
        float xMin = (Screen.width / 2) - (crosshair.width / 2);
        float yMin = (Screen.height / 2) - (crosshair.height / 2);
        GUI.DrawTexture(new Rect(xMin, yMin, crosshair.width, crosshair.height), crosshair);
    }

	// Update is called once per frame
	void FixedUpdate () {
        // Character View Movement

        float rotLeftRight = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(0, rotLeftRight, 0);

        rotUpDown -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        float desiredUpDown = Mathf.Clamp(rotUpDown, -upDownRange, upDownRange);
        Camera.main.transform.localRotation = Quaternion.Euler(desiredUpDown, 0, 0);

        // Character Movement
        float forwardSpeed = Input.GetAxis("Vertical") * movementSpeed;
        float sideSpeed = Input.GetAxis("Horizontal") * movementSpeed;

        verticalVelocity += Physics.gravity.y * Time.deltaTime;

        if (characterController.isGrounded && Input.GetButtonDown("Jump"))
        {
            verticalVelocity = jumpSpeed;
        }

        Vector3 speed = new Vector3(sideSpeed, verticalVelocity, -forwardSpeed);

        speed = transform.rotation * speed;

        characterController.Move(speed * Time.deltaTime); 
	}
}
