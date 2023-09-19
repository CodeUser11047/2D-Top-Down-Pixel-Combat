
using System;
using System.Collections;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    [SerializeField] private float knockBackTime = .1f;
    public bool gettingKnockedBack { get; private set; }
    private Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void GetKnockBack(Transform damageSource, float knockBackThrust)
    {
        gettingKnockedBack = true;
        Vector2 difference = knockBackThrust * rb.mass * (transform.position - damageSource.position).normalized;
        rb.AddForce(difference, ForceMode2D.Impulse);
        StartCoroutine(KnockRoutine());
    }
    private IEnumerator KnockRoutine()
    {
        yield return new WaitForSeconds(knockBackTime);
        rb.velocity = Vector2.zero;
        gettingKnockedBack = false;
    }

    internal void GetKnockBack(PlayerController instance, float v)
    {
        throw new NotImplementedException();
    }
}
