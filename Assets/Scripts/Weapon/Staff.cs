using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Staff : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponInfo weaponInfo;
    [SerializeField] private GameObject magicLaser;
    [SerializeField] private Transform magicLaserSpawnPos;

    private Animator myAnimator;
    readonly int ATTACK_HASH = Animator.StringToHash("Attack");
    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        MouseFollowWithOffest();
    }

    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }


    public void Attack()
    {
        myAnimator.SetTrigger(ATTACK_HASH);
    }

    public void SpwanStaffProjectilAnimeEvent()
    {
        GameObject _magicLaser = ObjectPool.Instance.GetObject(magicLaser,magicLaserSpawnPos.position,Quaternion.identity);
        //GameObject _magicLaser = Instantiate(magicLaser, magicLaserSpawnPos.position, Quaternion.identity);
        _magicLaser.GetComponent<MagicLaser>().UpdateLaserRange(weaponInfo.weaponRange);
    }

    private void MouseFollowWithOffest()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(PlayerController.Instance.transform.position);
        float angle = Mathf.Atan2(mousePos.y - PlayerController.Instance.transform.position.y, mousePos.x - PlayerController.Instance.transform.position.x) * Mathf.Rad2Deg;
        if (mousePos.x < playerScreenPoint.x)
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 180, angle);
        }
        else
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
