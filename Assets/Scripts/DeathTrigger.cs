using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Trigger attached to enemies and certain obstacles, forcing the player to go back to the last reset point
    //For enemies, the trigger is actually attached to a child which is slightly larger than their collision box
public class DeathTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider activator) {
        if(activator.CompareTag("Player")) {
            WheelController myController = activator.GetComponentInParent<WheelController>();
            myController.StartCoroutine(myController.DeathCoroutine());
        }
    }
}