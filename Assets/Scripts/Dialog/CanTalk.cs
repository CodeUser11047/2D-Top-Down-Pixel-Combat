using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanTalk : MonoBehaviour
{
    [SerializeField] private TextAsset FirstdialogTextAsset;
    [SerializeField] private TextAsset NotFirstdialogTextAsset;
    [SerializeField] private bool haveTwoAsset;

    private bool CanPress;
    private bool isSecondTalk = false;

    private void Start()
    {
        CanPress = false;
    }

    private void Update()
    {
        if (CanPress && Input.GetKeyDown(KeyCode.F))
        {
            if (haveTwoAsset)
            {
                if (!isSecondTalk)
                {
                    DialogManager.Instance.StartTalk(FirstdialogTextAsset);
                    CanPress = false;
                    DialogManager.Instance.Tagger.SetActive(false);
                    isSecondTalk = true;
                    return;
                }
                else
                {
                    DialogManager.Instance.StartTalk(NotFirstdialogTextAsset);
                    CanPress = false;
                    DialogManager.Instance.Tagger.SetActive(false);
                }
            }
            else if (!haveTwoAsset)
            {
                DialogManager.Instance.StartTalk(FirstdialogTextAsset);
                CanPress = false;
                DialogManager.Instance.Tagger.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.GetComponent<PlayerController>())
        {
            CanPress = true;
            DialogManager.Instance.Tagger.SetActive(true);
            DialogManager.Instance.Tagger.GetComponent<Button>().onClick.AddListener(delegate { DialogManager.Instance.StartTalk(FirstdialogTextAsset); });
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.GetComponent<PlayerController>())
        {
            CanPress = false;
            DialogManager.Instance.Tagger.SetActive(false);
        }
    }
}
