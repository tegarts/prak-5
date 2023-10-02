using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    MovementLogic movementLogic;

    private void Start()
    {
        movementLogic = this.GetComponentInParent<MovementLogic>();
    }
    private void OnTriggerEnter(Collider other)
    {
        movementLogic.GroundedChanger();
    }

}
