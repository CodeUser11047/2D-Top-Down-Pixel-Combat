using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    [SerializeField] private GameObject destroyVfx;
    [SerializeField] private int startHealth = 2;
    private Flash flash;

    private int currentHP;
    private void Awake()
    {
        flash = GetComponent<Flash>();
        currentHP = startHealth;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<DamageSource>() || other.gameObject.GetComponent<Projectill>())
        {
            StartCoroutine(flash.FlashRoutine());
            currentHP--;
            ObjectPool.Instance.GetObject(destroyVfx, transform.position, Quaternion.identity);


            //Instantiate(destroyVfx, transform.position, Quaternion.identity);
            if (currentHP <= 0)
            {
                StopAllCoroutines();
                GetComponent<PickupSpawner>()?.DropItem();
                Destroy(gameObject, flash.GetrestoreDefaultMatTime());
            }
        }
    }

}
