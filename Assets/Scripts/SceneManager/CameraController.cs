using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : Singleton<CameraController>
{
    private CinemachineVirtualCamera cinemachineVirtualCamera;


    private void Start()
    {
        SceneManager.sceneLoaded += OnsceneLoad;
    }

    private void OnsceneLoad(Scene scene, LoadSceneMode loadSceneMode)
    {
        SetPlayerCamerFollow();
    }


    public void SetPlayerCamerFollow()
    {
        cinemachineVirtualCamera = FindObjectOfType<CinemachineVirtualCamera>().GetComponent<CinemachineVirtualCamera>();
        cinemachineVirtualCamera.Follow = PlayerController.Instance.transform;
    }

}
