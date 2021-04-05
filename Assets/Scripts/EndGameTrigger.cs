using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Script that triggers when the player ends the game giving them an overlay
public class EndGameTrigger : MonoBehaviour
{
    [SerializeField] private GameObject EndGameScreen;
    void OnTriggerEnter(Collider activator) {
        EndGameScreen.SetActive(true);
    }
}
