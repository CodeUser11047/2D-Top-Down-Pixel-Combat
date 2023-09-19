using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponInfo weaponInfo;
    [SerializeField] private GameObject arrow;
    [SerializeField] private Transform arrowSpawnPos;
    [SerializeField] private float arrowSpeed;
    private Animator myAnimator;

    readonly int FIRE_HASH = Animator.StringToHash("Fire");
    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
    }
    public void Attack()
    {
        myAnimator.SetTrigger(FIRE_HASH);
        GameObject _arrow = ObjectPool.Instance.GetObject(arrow,arrowSpawnPos,transform.rotation);

        //GameObject _arrow = Instantiate(arrow, arrowSpawnPos.position, transform.rotation);
        _arrow.GetComponent<Projectill>().UpdateProjectileRange(weaponInfo.weaponRange);
    }

    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }
}
