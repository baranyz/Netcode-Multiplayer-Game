using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class FloatingText : NetworkBehaviour
{
    Camera cam;
    [SerializeField] Vector3 offset;
    TextMeshProUGUI textMesh;

    private void Start() {
        
        if(IsOwner) this.gameObject.SetActive(false);
        cam = GameObject.Find("Camera").GetComponent<Camera>();
        textMesh = GetComponent<TextMeshProUGUI>();
        if(!IsOwner){
            textMesh.SetText(transform.parent.transform.parent.GetComponent<NetworkObject>().OwnerClientId.ToString());
        }
    }

    private void Update() {
        
        if(IsOwner) return;
        cam = GameObject.Find("Camera").GetComponent<Camera>();
        transform.rotation = Quaternion.LookRotation(transform.position-cam.transform.position);
        transform.position = transform.parent.position + offset;
    }
}
