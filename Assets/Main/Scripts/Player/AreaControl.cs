using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public class AreaControl : NetworkBehaviour
{
    [SerializeField] float forwspeed = 3, turnSpeed = 10;

    private void Start() {

        transform.position = new Vector3(50,0,90);
    }

  
    private void Update() {
        
        //console or touch movement will added
        transform.Translate(0,0,Input.GetAxis("Vertical")*Time.deltaTime*forwspeed);
        transform.Rotate(0,Input.GetAxis("Horizontal")*Time.deltaTime*turnSpeed,0);
    }

}

