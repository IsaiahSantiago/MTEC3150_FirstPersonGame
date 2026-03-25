using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    /*
         //Making a variable for our new "controlling our character" value 
    private CharacterController controller;

    //Player movement Speed, Gravity to pull the player down and how high we can jump.
    public float speed = 12f;
    public float gravity = -9.18f * 2;
    public float jumpHeight = 3f;

    //This is a public transform to check if we're touching the ground, how far till we touch it and see if we CAN jump.
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    //Velocity of the jump.
    Vector3 Velocity;

    //to check if we're on the ground via true or false boolean. Same with if we're moving or not.
    bool isGrounded;
    bool isMoving;
    //bool isJumping;

    //Storing our last position to know if we've moved or are moving.
    private Vector3 lastPosition = new Vector3(0f, 0f, 0f);      
     
     */

    private CharacterController characterController;
    public float walkSpeed = 5;
    private float currentSpeed;
    public float sprintSpeed = 10;

    public float jumpForce = 5;

    public float gravity = 9.81f; //* 2; // Gravity is squared


    public float mouseSensitivity = 2;
    float verticalRotation;
    public float upDownRange = 80;

    private Camera cam; //Our Main Camera

    private Vector3 hitPoint; //world space position of our raycast hit point
    public ParticleSystem impactPS;
    //Giving a public float a range will give us a slider in the inspector mentu
    [Range (10, 30)] public int particleCount = 20;
    



    //Velocity of current movement.
    Vector3 currentMovement;



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
        Sprinting();
        Jumping();

        if (ObjectInFocus() != null)
        {
            //Can also be debug.log(ObjectInFocus().name)
            print(ObjectInFocus().name);

        }

        if (Input.GetMouseButtonDown(0)) 
        {
            impactPS.transform.position = hitPoint; //confirm hit point and move particles to the hitpoint.
            impactPS.Emit(particleCount); //Emits/plays particle animation and emits how many particles we specified in count.
        }


    }


    void movement() 
    {

        //Our horizontal is our X and our Vertical movment is our Z ( X and Z )
        //float verInput = Input.GetAxis("Vertical"); //Walking Vertically or Forward/back
        //float horInput = Input.GetAxis("Horizontal"); //Walking horizontally or Strafing left and right
        //float verSpeed = verInput * walkSpeed; //Vertical speed by walkSpeed value
        //float horSpeed = horInput * walkSpeed; //Horizontal speed by walkSpeed value

        float verticalInput = Input.GetAxis("Vertical"); //Walking Vertically or Forward/back
        float horizontalInput = Input.GetAxis("Horizontal"); //Walking horizontally or Strafing left and right
        float horizontalSpeed = horizontalInput * currentSpeed;
        float verticalSpeed = verticalInput * currentSpeed;

        //Vector3 horizontalMovement = new Vector3(horSpeed, 0, verSpeed);
        Vector3 horizontalMovement = new Vector3(horizontalSpeed, 0, verticalSpeed);
        horizontalMovement = transform.rotation * horizontalMovement;
        //verticalMovement = transform.rotation * verticalMovement;


        currentMovement.x = horizontalMovement.x;
        currentMovement.z = horizontalMovement.z;

        characterController.Move(currentMovement * Time.deltaTime);


        //if (KeyCode('SPACE') = true) 
        //{
        //    horSpeed * float(1.25), (verSpeed + (verSpeed * 25%)));
        //}


    }

    void Sprinting() 
    {

        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = sprintSpeed;

        }
        else 
        {
            currentSpeed = walkSpeed;
        }
   
    }


    void Jumping() 
    {
        if (characterController.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                currentMovement.y = jumpForce;
            }
        }
        else 
        {
            currentMovement.y -= gravity * Time.deltaTime;
        }


    }




    void MouseLook() 
    {
        float mouseXRotation = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(0, mouseXRotation, 0);
        verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);
        cam.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);



    }

    //Public functions require a return
    public GameObject ObjectInFocus()
    {
        GameObject result = null;

        RaycastHit hit; //Data for where ever the ray is cast

        //We can set a distance or NoDistance which is infinite raycasting <-- In update functions this can put pressure on computers.
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit)) //"out hit" means if this ray touches ANYTHING with a collider.
        {
            result = hit.transform.gameObject;

            hitPoint = hit.point; //Where the laser hits the object
        }


        return result;
    
    }




    //New code here







}

















/*
    checking if grounded is true or not per frame.
   //This will check if the ground check position at our feet is in the range of an invisibal ball under us that checks if there's a floor
   to begin with 
isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
// resetting the default velocity if we're not jumping
if (isGrounded && Velocity.y < 0)
{
    Velocity.y = -2f;
}

//Getting the actual inputs for the Y and Z axis
float x = Input.GetAxis("Horizontal");
float z = Input.GetAxis("Vertical");

//Storing these inputs inside of a new moving vector.
//Now we can multiply the input by the direction.  TRANSFORM RIGHT = Moving left or right and TRANSFORM FORWARD = Moving back and forth
Vector3 move = transform.right * x + transform.forward * z; //(right - Red axis, forward - Blue axis )

//Actually moving the player
controller.Move(move * speed * Time.deltaTime); //Remember that time.deltatime makes it consistent with the unity framerate

//Check if the player can Jump!
if (Input.GetButtonDown("Jump") && isGrounded)
{
    //Code for when we're actually jumping / Going Up
    Velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
}

//Falling down or applying gravity 
Velocity.y += gravity * Time.deltaTime;

//Executing the jump
controller.Move(Velocity * Time.deltaTime);

//Check if we're actually moving:
if (lastPosition != gameObject.transform.position && isGrounded == true)
{
    isMoving = true; //Reffering to movement of the player on the ground.         
}
else
{
    isMoving = false; //when we're still

}

//Tracking the last position and calling it the last position for when we stop moving 
lastPosition = gameObject.transform.position;





//void Destroy() 
//{
//    if (Physics.RaycastHit = true)
//    {
//        ObjectInFocus();

//        Destroy(gameObject);
//        //Instantiate(Destroy.GameObject, transform.position, transform.rotation);
//        //Instantiate(Destroy.Particle, transform.position, transform.rotation);


//    }


//    //if (Physics.Raycast.hit = true)
//    //{
//    //    Destroy'GameObject'.transform;
//    //    result = hitPoint.Destroy.GameObject.transform;
//    //}

//}



*/











