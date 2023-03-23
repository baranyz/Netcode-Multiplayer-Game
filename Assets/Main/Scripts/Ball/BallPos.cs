using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BallPos : NetworkBehaviour
{
    private void Start() {
        
        if(!IsOwner) this.gameObject.SetActive(false);
    }
}
