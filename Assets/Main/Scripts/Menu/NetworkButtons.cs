using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using TMPro;

public class NetworkButtons : NetworkBehaviour
{
    [SerializeField] Button onHost, onClient;
    [SerializeField] TMP_InputField textMesh;
   
    private void OnEnable() {
        
        onHost.onClick.AddListener(() => {
            RelayManager.Instance.CreateRelay();
        });
        
        //temp
        onClient.onClick.AddListener(() => {
            RelayManager.Instance.JoinRelay(textMesh.text);
        });
    }
    
}
