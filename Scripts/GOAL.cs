using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOAL : MonoBehaviour
{
    
    
    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            StageController.Instance.IsGoal = true;
        }
    }
}
