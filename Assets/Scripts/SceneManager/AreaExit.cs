using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaExit : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private string sceneTransitionName;
    [SerializeField] private float waitToLoadTime = 1f;

    private void Start()
    {
        // SceneManagment.Instance.OnExitScene += SceneManagement_OnExitScene;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>())
        {
            SceneManagment.Instance.SetTransitionName(sceneTransitionName);

            UIFade.Instance.FadeToBlack();
            StartCoroutine(LoadScreenRoutine());
        }
    }

    private IEnumerator LoadScreenRoutine()
    {
        while (waitToLoadTime >= 0)
        {
            waitToLoadTime -= Time.deltaTime;
            yield return null;
        }
        SceneManager.LoadScene(sceneToLoad);
    }

}
