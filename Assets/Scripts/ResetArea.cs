using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//This is a general script which is attached to various invisible reset areas - when triggered, they will set your reset position to their position
public class ResetArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider activator) {
        if(activator.CompareTag("Player")) {
            WheelController.ResetPos = transform.position;
            WheelController.ResetRot = transform.rotation;
        }
    }
}
