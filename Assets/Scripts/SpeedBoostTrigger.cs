using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//When the moves onto one of these spots, they get a short acceleration boost
public class SpeedBoostTrigger : MonoBehaviour
{
    [SerializeField] private float speedTime;
    void OnTriggerEnter(Collider activator) {
        if(activator.CompareTag("Player")) {
            activator.GetComponentInParent<WheelController>().speedBoost = speedTime;
        }
    }
}
