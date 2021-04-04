using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    public float speedBoost;//Whether the player has a speed boost from something
    private const float SPEED_BOOST_AMT = 5f;
    private bool jump;//Whether the object should jump at this moment
    private bool reset;//Can also be set by objects it collides with
    private bool resettable;//Says whether player is able to reset
    public static Vector3 ResetPos;//Where the player resets to if they die/press R
    public static Quaternion ResetRot;//Rotation the player resets to
    void Start()
    {
        resettable = true;
        //Center of Mass is lowered so it moves more easily on two wheels
        GetComponent<Rigidbody>().centerOfMass += new Vector3(0f, -0.5f, 0f);
        ResetPos = transform.position;
        ResetRot = transform.rotation;
    }

    
    //Player Input occurs here
    void Update()
    {
        if(speedBoost > 0) {
            speedBoost -= Time.deltaTime;
        }
        rotControl = Input.GetAxis("Horizontal");
        accControl = Input.GetAxis("Vertical");
        //Player can attempt to jump - will check to see if wheels are on the ground
        if(Input.GetKeyDown(KeyCode.Space)) {
            if(rightWheel.isGrounded || leftWheel.isGrounded) {
                jump = true;
            }
        }
        //When R is pressed, everything is reset
        if(Input.GetKeyDown(KeyCode.R) && resettable) {
            reset = true;//Is reset during fixed update, because I don't want to mess up the physics
        }
    }
        //TODO: Implement a simple additional move forward code to get out of stickage while on the edge of a platform

    //Movement occurs here
    void FixedUpdate() {
        float steering = rotControl * maxSteer;
        //leftWheel.steerAngle = steering;
        //rightWheel.steerAngle = steering;
        leftWheel.motorTorque = accControl * maxMotorBack;
        rightWheel.motorTorque = accControl * maxMotorBack;
        
        frontWheel.steerAngle = steering;
        frontWheel.motorTorque = accControl * maxMotorFront;
        //If it's speeding, increase the torque
        if(speedBoost > 0) {
            leftWheel.motorTorque *= SPEED_BOOST_AMT;
            rightWheel.motorTorque *= SPEED_BOOST_AMT;
            frontWheel.motorTorque *= SPEED_BOOST_AMT;
        }
        RotateWheelImage(rightWheelImage, rightWheel);
        RotateWheelImage(leftWheelImage, leftWheel);
        RotateWheelImage(frontWheelImage, frontWheel);

        if(jump) {
            //Jumps directly upwards
            jump = false;
            Vector3 jumpForce = new Vector3(0f, 1f, 0f) * jumpPower;
            GetComponent<Rigidbody>().AddForce(jumpForce,ForceMode.Impulse);
        }
        if(reset) {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            transform.position = ResetPos;
            transform.rotation = ResetRot;
            frontWheel.steerAngle = 0;
            frontWheel.motorTorque = 0;
            leftWheel.motorTorque = 0;
            rightWheel.motorTorque = 0;
            RotateWheelImage(rightWheelImage, rightWheel);
            RotateWheelImage(leftWheelImage, leftWheel);
            RotateWheelImage(frontWheelImage, frontWheel);
            reset = false;
            resettable = true;
            speedBoost = 0.5f;//Gets a very small speed boost when reset
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

    public const float TIME_TO_DIE = 1.5f;//Time it takes for the palyer to die/reset
    [SerializeField] private Image fadeToBlack;
    //This Coroutine causes you to reset from death after fading to black
    public IEnumerator DeathCoroutine() {
        //Only does stuff if the player is currently able to be reset
        if(resettable) {
            resettable = false;
            float curTime = 0;
            while(curTime < TIME_TO_DIE) {
                fadeToBlack.color = new Color(Color.black.r, Color.black.g, Color.black.b, curTime/TIME_TO_DIE);
                curTime += Time.deltaTime;
                yield return null;
            }
            fadeToBlack.color = Color.black;
            yield return new WaitForSeconds(0.2f);
            fadeToBlack.color = new Color(Color.black.r, Color.black.g, Color.black.b, 0);
            reset = true;
        }
    }
}
