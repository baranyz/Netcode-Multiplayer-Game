using UnityEngine;
using Unity.Netcode;

public class DestroyExplosion : NetworkBehaviour
{
    NetworkVariable<float> countDown = new NetworkVariable<float>();

    private void Update() {
        
        if(!IsServer) return;

        countDown.Value += Time.deltaTime;

        if(countDown.Value > 2){
            NetworkManager.Destroy(this.gameObject);
        }

    }
}
