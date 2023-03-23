using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class LifeCount : NetworkBehaviour
{
    Camera cam;
    [SerializeField] Vector3 offset;

    private void Start() {
        
        if(IsOwner) gameObject.transform.localScale = new Vector3(0,0,0);
        cam = GameObject.Find("Camera").GetComponent<Camera>();

    }

    private void Update() {
        
        if(IsOwner){
            if(GetComponent<Slider>().value == 0) Debug.Log("Game Over!");
            GameObject[] otherLifes = GameObject.FindGameObjectsWithTag("LifeCount");
            for(int i = 0; i < otherLifes.Length; i++){
                if(otherLifes[i] == gameObject) continue;
                if(otherLifes[i].GetComponent<Slider>().value > 0){
                    break;
                }
                if(i == otherLifes.Length-1) Debug.Log("you won!");
            }
        }
        if(IsOwner) return;
        cam = GameObject.Find("Camera").GetComponent<Camera>();
        transform.rotation = Quaternion.LookRotation(transform.position-cam.transform.position);
        transform.position = transform.parent.position + offset;


    }


}