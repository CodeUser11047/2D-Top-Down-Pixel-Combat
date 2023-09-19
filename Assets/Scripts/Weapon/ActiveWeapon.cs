using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class ActiveWeapon : Singleton<ActiveWeapon>
{
    public MonoBehaviour CurrentActiveWeapon { get; private set; }
    private PlayerControls playerControls;
    private float timeBetweenAttacks;
    private bool attackButtonDown, isAttacking = false;

    protected override void Awake()
    {
        base.Awake();

        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }
    private void Start()
    {
        //lambda
        playerControls.Combot.Attack.started += _ => StartAttacking();
        playerControls.Combot.Attack.canceled += _ => StopAttacking();

        AttackCoolDown();
    }

    private void Update()
    {
        if (!DialogManager.Instance.istalking)
            Attack();
    }

    private void AttackCoolDown()
    {
        isAttacking = true;

        StopAllCoroutines();

        StartCoroutine(TimeBetweenAttackRoutine());
    }

    private IEnumerator TimeBetweenAttackRoutine()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);
        isAttacking = false;
    }


    public void NewWeapon(MonoBehaviour newWeapon)
    {
        CurrentActiveWeapon = newWeapon;

        AttackCoolDown();

        timeBetweenAttacks = (CurrentActiveWeapon as IWeapon).GetWeaponInfo().weaponCooldown;
    }

    public void WeaponNull()
    {
        CurrentActiveWeapon = null;
    }

    private void StartAttacking()
    {
        attackButtonDown = true;
    }
    private void StopAttacking()
    {
        attackButtonDown = false;
    }

    private void Attack()
    {

        if (attackButtonDown && !isAttacking)
        {

            AttackCoolDown();

            (CurrentActiveWeapon as IWeapon)?.Attack();
        }
    }
}
