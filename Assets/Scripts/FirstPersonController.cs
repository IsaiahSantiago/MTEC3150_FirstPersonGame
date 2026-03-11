using UnityEngine;

public class FirstPersonController : MonoBehaviour
{


    private CharacterController characterController;
    public float walkSpeed = 5;
    public float mouseSensitivity = 2;
    float verticalRotation;
    public float upDownRange = 80;

    private Camera cam;



    void Start()
    {
        characterController = GetComponent<CharacterController>();
        cam = Camera.main;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

    }



    void Update()
    {
        movement();
        MouseLook();


    }


    void movement() 
    {

        float verInput = Input.GetAxis("Vertical"); //Walking Vertically or Forward/back
        float horInput = Input.GetAxis("Horizontal"); //Walking horizontally or Strafing left and right
        float verSpeed = verInput * walkSpeed; //Vertical speed by walkSpeed value
        float horSpeed = horInput * walkSpeed; //Horizontal speed by walkSpeed value

        Vector3 horizontalMovement = new Vector3(horSpeed, 0, verSpeed);

        characterController.Move(horizontalMovement * Time.deltaTime);

    }

    void MouseLook() 
    {
        float mouseXRotation = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(0, mouseXRotation, 0);
        verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);
        cam.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);


    }






}
















