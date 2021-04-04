using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Enemy moves on a simple system, moving back and forth between 2+ locations
public class EnemyMovement : MonoBehaviour
{
    //This first vector dictates the size of area that this enemy moves around in, using a character controller
    [SerializeField] private Vector3[] moveArray;//Locations it is moving towards
    [SerializeField] private int startTarget;//first target it aims at
    private int currentTarget;//This number says which target the enemy should be currently aiming for
    [SerializeField] private float speed;
    private Vector3 direction;
    private const float GRAVITY = 20f;
    private CharacterController myController;
    void Start()
    {
        currentTarget = startTarget;
        direction = Random.insideUnitCircle.normalized;
        myController = GetComponent<CharacterController>();
    }

    [SerializeField] private float distanceChangeDirection;//How far enemy should be from target befor ethey change direction (for when issues arise)
    void FixedUpdate()
    {
            //Partially based on Character Controller from class
        //Moves towards its designated target - once it gets close enough, it heads for a different target
       
       //First checks if the enemy is a certain distance or closer to their target
            //Uses a vector2 which is a combo of x and z coords, bc those are the horizontal coordinates
        Vector2 checkPos = new Vector2(transform.position.x - moveArray[currentTarget].x, transform.position.z - moveArray[currentTarget].z);
        if(checkPos.magnitude <= distanceChangeDirection) {
            currentTarget = (currentTarget + 1) % moveArray.Length;
        }
       if(myController.isGrounded) {
           direction = (moveArray[currentTarget] - transform.position).normalized * speed;
           direction.y = 0;
       }
       else {
           float tempY = direction.y;
           direction = (moveArray[currentTarget] - transform.position).normalized * speed;
           direction.y = tempY;
       }
       direction.y -= GRAVITY * Time.fixedDeltaTime;
       myController.Move(direction * Time.fixedDeltaTime);
    }
}
