using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class AssignCam : NetworkBehaviour
{
    Canvas canvas;

    private void Start() {
        
        if(IsOwner) {
            this.gameObject.transform.localScale = new Vector3(0,0,0);
            return;
        }

        if(!IsOwner){
            canvas = GetComponent<Canvas>();
            canvas.worldCamera = GameObject.Find("Camera").GetComponent<Camera>();
        }
    }
}
