using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using UnityEngine.UI;

public class Life : NetworkBehaviour
{
    NetworkVariable<float> life = new NetworkVariable<float>(1,writePerm: NetworkVariableWritePermission.Server);
    Slider lifeBar;

    private void OnCollisionEnter(Collision other) {
        
        if(other.gameObject.tag == "Ball" && other.gameObject.transform.parent != transform.parent){
            if(IsOwner){
                LifeServerRpc();
            }
        }
    }

    [ServerRpc]
    public void LifeServerRpc(){
        life.Value -= 0.1f;
        LifeClientRpc(life.Value);
    }

    [ClientRpc]
    public void LifeClientRpc(float val){
        GameObject[] lifeCount = GameObject.FindGameObjectsWithTag("LifeCount");
        for(int i = 0; i < lifeCount.Length; i++){
            if(lifeCount[i].transform.parent.parent.GetComponent<NetworkObject>().OwnerClientId == 
            transform.parent.GetComponent<NetworkObject>().OwnerClientId){
                lifeBar = lifeCount[i]?.GetComponent<Slider>();
            }
        }
        if(lifeBar != null) lifeBar.value = val;
    }
}