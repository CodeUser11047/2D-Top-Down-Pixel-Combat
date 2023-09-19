using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : Singleton<PlayerHealth>
{
    public bool IsDead { get; private set; }

    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float knockBackThrustAmount = 10f;
    [SerializeField] private float damageRecoveryTime = 1f;
    readonly int DEATH_HASH = Animator.StringToHash("Death");
    private Slider healthSlider;
    private KnockBack knockBack;
    private Flash flash;

    private int currentHealth;
    private bool isCanTakeDamge = true;
    const string HEALTH_SLIDER = "Health Slider";
    const string TOWN_TEXT = "Town";

    protected override void Awake()
    {
        base.Awake();

        knockBack = GetComponent<KnockBack>();
        flash = GetComponent<Flash>();
    }

    private void Start()
    {
        IsDead = false;
        currentHealth = maxHealth;

        UIFade.Instance.FadeToClear();

        UpdateHealthSlider();
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        EnemyAI enemy = other.gameObject.GetComponent<EnemyAI>();

        if (enemy)
        {
            TakeDamge(1, other.gameObject.transform);
        }
    }

    public void TakeDamge(int damageInt, Transform hitTransform)
    {
        if (!isCanTakeDamge) { return; }

        ScreenShakeManager.Instance.ShakeScreem();

        knockBack.GetKnockBack(hitTransform, knockBackThrustAmount);
        StartCoroutine(flash.FlashRoutine());
        isCanTakeDamge = false;
        currentHealth -= damageInt;

        StartCoroutine(DamageRecoveryRoutin());
        UpdateHealthSlider();
        CheckPlayerDeath();

        // Debug.Log(currentHealth);
    }

    private void CheckPlayerDeath()
    {
        if (currentHealth <= 0 && !IsDead)
        {
            IsDead = true;
            Destroy(ActiveWeapon.Instance.gameObject);
            currentHealth = 0;
            GetComponent<Animator>().SetTrigger(DEATH_HASH);

            StartCoroutine(DeathLoadScreenRoutine());
        }
    }

    public void GetHealth()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth++;
        }

        UpdateHealthSlider();
    }

    private IEnumerator DeathLoadScreenRoutine()
    {
        UIFade.Instance.FadeToBlack();
        yield return new WaitForSeconds(1f);
        Destroy(DialogManager.Instance.gameObject);
        Destroy(gameObject);
        SceneManager.LoadScene(TOWN_TEXT);

    }

    private IEnumerator DamageRecoveryRoutin()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        isCanTakeDamge = true;
    }

    private void UpdateHealthSlider()
    {
        if (healthSlider == null)
        {
            healthSlider = GameObject.Find(HEALTH_SLIDER).GetComponent<Slider>();
        }

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }
}
