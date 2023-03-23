using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.Netcode;
public class SceneLoader : NetworkBehaviour
{

    public static SceneLoader Instance{get;set;}
    [SerializeField] GameObject canvas;
    [SerializeField] Slider slider;

    private void Awake() {
        
        if(Instance == null){
            Instance = this;
            NetworkManager.DontDestroyOnLoad(this);
        }
        else{
            Destroy(this);
        }
    }


    public async void LoadScene(ulong clientId, string sceneName, LoadSceneMode loadSceneMode, AsyncOperation scene){


        scene.allowSceneActivation = false;

        canvas.SetActive(true);

        while(scene.progress < .9f){
            await Task.Delay(100);
            slider.value = scene.progress;
        }
        await Task.Delay(300);
        scene.allowSceneActivation = true;

        canvas.SetActive(false);
    }
}
