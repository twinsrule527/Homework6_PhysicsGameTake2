using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Controller for the player - Uses wheels and a jump function
    //Heavily referenced in-class demo of wheel colliders as well as Unity Tutorial page (https://docs.unity3d.com/Manual/WheelColliderTutorial.html) to do this
public class WheelController : MonoBehaviour
{
    [SerializeField] private WheelCollider leftWheel;
    [SerializeField] private WheelCollider rightWheel;
    [SerializeField] private WheelCollider frontWheel;
    [SerializeField] private GameObject rightWheelImage;
    [SerializeField] private GameObject leftWheelImage;
    [SerializeField] private GameObject frontWheelImage;
    //Player Controls
    private float rotControl;
    private float accControl;

    //Multipliers to Control Amounts (GameFeel)
    [SerializeField] private float maxMotorFront;//Separate motor powers from front and back wheels
    [SerializeField] private float maxMotorBack;
    [SerializeField] private float maxSteer;
    [SerializeField] private float jumpPower;
    private bool jump;//Whether the object should jump at this moment
    void Start()
    {
        //Center of Mass is lowered so it moves more easily on two wheels
        GetComponent<Rigidbody>().centerOfMass += new Vector3(0f, -0.5f, 0f);
    }

    
    //Player Input occurs here
    void Update()
    {
        rotControl = Input.GetAxis("Horizontal");
        accControl = Input.GetAxis("Vertical");
        //Player can attempt to jump - will check to see if wheels are on the ground
        if(Input.GetKeyDown(KeyCode.Space)) {
            if(rightWheel.isGrounded || leftWheel.isGrounded) {
                jump = true;
            }
        }
    }

    //Movement occurs here
    void FixedUpdate() {
        float steering = rotControl * maxSteer;
        //leftWheel.steerAngle = steering;
        //rightWheel.steerAngle = steering;
        leftWheel.motorTorque = accControl * maxMotorBack;
        rightWheel.motorTorque = accControl * maxMotorBack;
        
        frontWheel.steerAngle = steering;
        frontWheel.motorTorque = accControl * maxMotorFront;
        RotateWheelImage(rightWheelImage, rightWheel);
        RotateWheelImage(leftWheelImage, leftWheel);
        RotateWheelImage(frontWheelImage, frontWheel);

        if(jump) {
            //Jumps directly upwards
            jump = false;
            Vector3 jumpForce = new Vector3(0f, 1f, 0f) * jumpPower;
            GetComponent<Rigidbody>().AddForce(jumpForce,ForceMode.Impulse);
        }
    }

    //Have the visible form of the wheel match your collider movement
        //From the class code
    public void RotateWheelImage(GameObject wheel, WheelCollider collider) {
        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);
        wheel.transform.position = position;
        wheel.transform.rotation = rotation;
        wheel.transform.rotation *= Quaternion.Euler(0f, 0f, 90f);
    }
}
