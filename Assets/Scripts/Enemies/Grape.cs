using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grape : MonoBehaviour, IEnemy
{
    [SerializeField] private GameObject grapeProjectilePrefab;


    readonly private int ATTACK_HASH = Animator.StringToHash("Attack");
    private Animator myAnimator;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

    }
    public void Attack()
    {
        myAnimator.SetTrigger(ATTACK_HASH);

        if (transform.position.x - PlayerController.Instance.transform.position.x < 0)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }

    }

    public void SpawnProjectileEvent()
    {
        ObjectPool.Instance.GetObject(grapeProjectilePrefab, transform.position, Quaternion.identity);

        //Instantiate(grapeProjectilePrefab, transform.position, Quaternion.identity);
    }


}
