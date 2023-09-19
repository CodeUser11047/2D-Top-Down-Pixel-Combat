using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEntrance : MonoBehaviour
{
    [SerializeField] private string transitionName;

    private void Start()
    {
        if (transitionName == SceneManagment.Instance.SceneTransitionName)
        {
            UIFade.Instance.FadeToClear();
            PlayerController.Instance.transform.position = this.transform.position;
            CameraController.Instance.SetPlayerCamerFollow();
        }
    }
}
