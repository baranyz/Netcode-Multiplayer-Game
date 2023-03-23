using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Netcode;
using Unity.Netcode.Components;

public class BallThrow : NetworkBehaviour
{
    NetworkVariable<bool> isTouch = new NetworkVariable<bool>(writePerm: NetworkVariableWritePermission.Owner);
    Camera cam;
    public float throwForce = 10f;
    public float maxPullDistance = 2f;
    [SerializeField] GameObject explosionEffect;
    private bool isHolding = false;
    private Vector2 startTouchPosition;
    private Vector3 startBallPosition;
    GameObject exp, ballPos;
    TrailRenderer trail;
    GameObject glow;
    float glowScale;
    
    private void Start()
    {
        GameObject[] glows = GameObject.FindGameObjectsWithTag("Glow");
        foreach(GameObject g in glows){
            if(g.transform.parent.IsChildOf(transform.parent)){
                glow = g;
                glow.transform.localScale = new Vector3(0.3f,0.03f,0.3f);
                glowScale = 0.3f;
            }
        }
        ballPos = GameObject.FindGameObjectWithTag("BallPos");

        if(!IsOwner) return;

        trail = GetComponent<TrailRenderer>();
        cam = GameObject.Find("Camera").GetComponent<Camera>();
        trail.enabled = false;
    }

    private void Update()
    {

        if(isTouch.Value && glowScale > 0.05f){
            glowScale -= Time.deltaTime/3;
            glow.transform.localScale = new Vector3(glowScale,default,glowScale);
        }
        else if(isTouch.Value){
            glow.transform.localScale = new Vector3(0,0,0);
        }
        GetComponent<Rigidbody>().isKinematic = false;

        if(!IsOwner) return;

        TouchPullThrow();
      
        if(Input.GetKeyDown(KeyCode.Space)){
            DestroyServerRpc();
   
        } 
        if(isTouch.Value == false) {
            transform.SetPositionAndRotation(ballPos.transform.position, ballPos.transform.rotation);
            
        }
    }

    private void TouchPullThrow(){
        if (Input.touchCount > 0)
        {   
            Touch touch = Input.GetTouch(0);
            Vector3 touchPos = cam.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 1));

            if (touch.phase == TouchPhase.Began)
            {
                startTouchPosition = touch.position;
                startBallPosition = transform.localPosition;

                RaycastHit hit;
                if (Physics.Raycast(cam.ScreenPointToRay(touch.position), out hit) && hit.transform == transform)
                {
                    isTouch.Value = true;
                    isHolding = true;
                }
            }
            else if (touch.phase == TouchPhase.Ended && isHolding)
            {
                isTouch.Value = true;
                isHolding = false;
                Vector3 direction = new Vector3(touch.position.x - startTouchPosition.x, (touch.position.y - startTouchPosition.y)*5,
                    touch.position.y - startTouchPosition.y) / 2500;
                    direction.z = direction.y/2;
                Rigidbody rb = GetComponent<Rigidbody>();
                rb.AddRelativeForce(direction * -throwForce, ForceMode.Impulse);
                trail.enabled = true;
            }

            if (isHolding)
            {
                Vector3 direction = new Vector3(touch.position.x - startTouchPosition.x, touch.position.y - startTouchPosition.y,
                    touch.position.y - startTouchPosition.y) / Screen.height;
                float distance = direction.magnitude;
                direction.Normalize();

                if (distance > maxPullDistance)
                {
                    distance = maxPullDistance;
                }

                transform.localPosition = startBallPosition + direction * distance;
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if(isTouch.Value && IsOwner && !other.transform.IsChildOf(transform.parent)) ExpSpawnServerRpc();
    }

    [ServerRpc]
    public void ExpSpawnServerRpc(){
        exp = Instantiate(explosionEffect, new Vector3(transform.position.x,transform.position.y, transform.position.z), Quaternion.identity);
        exp.GetComponent<NetworkObject>().SpawnWithOwnership(GetComponent<NetworkObject>().OwnerClientId);
        GetComponent<NetworkObject>().Despawn();
    }

    [ServerRpc]
    public void DestroyServerRpc(){
        
        GetComponent<NetworkObject>().Despawn();
    }

}
