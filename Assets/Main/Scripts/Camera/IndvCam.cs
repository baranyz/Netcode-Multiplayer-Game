using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class IndvCam : NetworkBehaviour
{

    private void Start() {
        
        if(!IsOwner){
            this.gameObject.SetActive(false);
        }
    }
}
