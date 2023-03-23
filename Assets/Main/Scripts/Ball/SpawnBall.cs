using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SpawnBall : NetworkBehaviour
{
    
    [SerializeField] GameObject ball;
    GameObject ballGo;
    GameObject ballPos;

    private void Start() {
        
        if(!IsOwner)return;

        SpawnServerRpc();
    }

    private void Update() {
        
        if(!IsOwner) return;

        SpawnServerRpc();
    }
    [ServerRpc]
    public void SpawnServerRpc(){
        if(ballGo == null){
            ballPos = GameObject.FindGameObjectWithTag("BallPos");
            ballGo = Instantiate(ball, ballPos.transform.position, transform.localRotation);
            ballGo.GetComponent<NetworkObject>().SpawnWithOwnership(GetComponent<NetworkObject>().OwnerClientId);
            ballGo.transform.SetParent(transform);
        }
    }
}
