using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stamina : Singleton<Stamina>
{
    public int CurrentStamina { get; private set; }

    [SerializeField] private Sprite fullStaminaImage, EmptyStaminaImage;
    [SerializeField] private int timeBetweenStaminaRefresh = 3;
    private Transform staminaContainer;
    private int startingStamia = 3;
    private int maxStamia;
    const string STAMINA_CONTAINER_TEXT = "Stamina Container";

    protected override void Awake()
    {
        base.Awake();

        maxStamia = startingStamia;
        CurrentStamina = startingStamia;
    }
    private void Start()
    {
        staminaContainer = GameObject.Find(STAMINA_CONTAINER_TEXT).transform;
    }
    public void UseStamia()
    {
        CurrentStamina--;
        UpdateStaminaImager();
    }

    public void RefreshStamia()
    {
        if (CurrentStamina < maxStamia)
        {
            CurrentStamina++;
        }
        UpdateStaminaImager();
    }

    private IEnumerator RefreshStamiaRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenStaminaRefresh);
            RefreshStamia();
        }
    }

    private void UpdateStaminaImager()
    {
        for (int i = 0; i < maxStamia; i++)
        {
            if (i <= CurrentStamina - 1)
            {
                staminaContainer.GetChild(i).GetComponent<Image>().sprite = fullStaminaImage;
            }
            else
            {
                staminaContainer.GetChild(i).GetComponent<Image>().sprite = EmptyStaminaImage;
            }
        }
        if (CurrentStamina < maxStamia)
        {
            StopAllCoroutines();
            StartCoroutine(RefreshStamiaRoutine());
        }
    }

}
