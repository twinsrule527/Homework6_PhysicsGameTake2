using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Camera moves with the player, but does not change rotation
public class CameraControl : MonoBehaviour
{
    [SerializeField] private Transform myPlayer;
    private Vector3 cameraOffset;
    void Start()
    {
        cameraOffset = transform.position - myPlayer.transform.position;
    }

    void LateUpdate()
    {
        transform.position = myPlayer.transform.position + cameraOffset;
    }
}
