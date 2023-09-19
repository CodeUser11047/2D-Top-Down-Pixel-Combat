using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public int startHealth = 3;
    [SerializeField] private GameObject deathVFXPrefab;
    [HideInInspector]
    public int currentHealth;
    private KnockBack knockBack;
    private Flash flash;
    private bool isBoos;
    [SerializeField] private GameObject healthSliderPar;

    private void Awake()
    {
        knockBack = GetComponent<KnockBack>();
        flash = GetComponent<Flash>();
    }

    private void Start()
    {
        currentHealth = startHealth;
        if (GetComponent<EnemyAI>().state == EnemyState.Boss)
        {
            isBoos = true;
            healthSliderPar = GameObject.Find("BossHeart Container");
            for (int i = 0; i < healthSliderPar.transform.childCount; i++)
            {
                healthSliderPar.transform.GetChild(i).gameObject.SetActive(true);
            }

            UpdateHealthSlider();
        }

    }

    private void OnDisable()
    {
        if (healthSliderPar != null)
        {
            for (int i = 0; i < healthSliderPar.transform.childCount; i++)
            {
                healthSliderPar.transform.GetChild(i).gameObject.SetActive(false);
            }

        }

    }

    public void TakeDamge(int damage)
    {
        StartCoroutine(flash.FlashRoutine());
        currentHealth -= damage;

        if (isBoos)
            UpdateHealthSlider();

        knockBack.GetKnockBack(PlayerController.Instance.transform, 15f);
        if (currentHealth <= 0)
        {
            if (isBoos)
            {
                for (int i = 0; i < healthSliderPar.transform.childCount; i++)
                {
                    healthSliderPar.transform.GetChild(i).gameObject.SetActive(false);
                }
            }

            StartCoroutine(DeathRoutine());
        }
    }

    private void UpdateHealthSlider()
    {
        Slider healthSlider = GameObject.Find("Boss Health Slider").GetComponent<Slider>();

        healthSlider.transform.parent.gameObject.SetActive(true);
        healthSlider.maxValue = startHealth;
        healthSlider.value = currentHealth;
    }

    private IEnumerator DeathRoutine()
    {
        yield return new WaitForSeconds(flash.GetrestoreDefaultMatTime());
        GetComponent<PickupSpawner>().DropItem();
        Death();
    }

    private void Death()
    {
        ObjectPool.Instance.GetObject(deathVFXPrefab, transform.position, Quaternion.identity);

        //Instantiate(deathVFXPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);

    }
}
