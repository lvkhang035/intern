using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class finishTrigger : MonoBehaviour{

    public controller Controller;

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Finish")
            Controller.hasFinished = true;
            
    }

}
