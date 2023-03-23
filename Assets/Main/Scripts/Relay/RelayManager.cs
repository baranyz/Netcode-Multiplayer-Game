using System.Windows.Input;
using System.ComponentModel.Design;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using System.Threading.Tasks;

public class RelayManager : MonoBehaviour
{
    public static RelayManager Instance{get;set;}

    private void Awake() {
        
        if(Instance == null){
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else{
            Destroy(this);
        }
    }
    private async void Start() {
    
        await Authenticate();
    }

    private async Task Authenticate(){

        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async void CreateRelay(){

        try{
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(5);
            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            Debug.Log($"join code: {joinCode}");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(
                allocation.RelayServer.IpV4,
                (ushort) allocation.RelayServer.Port,
                allocation.AllocationIdBytes,
                allocation.Key,
                allocation.ConnectionData
            );
            NetworkManager.Singleton.StartHost();
            NetworkManager.Singleton.SceneManager.OnLoad += SceneLoader.Instance.LoadScene;
            NetworkManager.Singleton.SceneManager.LoadScene("MainScene",loadSceneMode:UnityEngine.SceneManagement.LoadSceneMode.Additive);
        }
        catch(RelayServiceException e){
            Debug.Log($"error: {e}");
        }
    }
    public async void JoinRelay(string joinCode){

        try{
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(
                joinAllocation.RelayServer.IpV4,
                (ushort) joinAllocation.RelayServer.Port,
                joinAllocation.AllocationIdBytes,
                joinAllocation.Key,
                joinAllocation.ConnectionData,
                joinAllocation.HostConnectionData
            );
            NetworkManager.Singleton.StartClient();
            NetworkManager.Singleton.SceneManager.OnLoad += SceneLoader.Instance.LoadScene;
            NetworkManager.Singleton.SceneManager.LoadScene("MainScene", loadSceneMode:UnityEngine.SceneManagement.LoadSceneMode.Additive);
        }
        catch(RelayServiceException e){
            Debug.Log($"error: {e}");
        }
    }
}
