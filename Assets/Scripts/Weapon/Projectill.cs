using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectill : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 22f;
    [SerializeField] private GameObject projectileVFX;
    [SerializeField] private bool isEnemyProjectile = false;
    [SerializeField] private float projectillRange = 10f;


    private Vector3 startPos;
    private void OnEnable()
    {
        startPos = transform.position;
        if (GetComponent<TrailRenderer>())
        {
            GetComponent<TrailRenderer>().emitting = false;

            StartCoroutine(TrailWaitActiveRoutine());
        }

    }

    void Update()
    {
        MoveProjectill();
        DetectFireDistance();
    }

    //外部设置属性
    public void UpdateProjectileRange(float projectillRange)
    {
        this.projectillRange = projectillRange;
    }

    public void UpdateProjectileMoveSpeed(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyHealth enemy = other.gameObject.GetComponent<EnemyHealth>();
        Indestructible indestructible = other.gameObject.GetComponent<Indestructible>();
        PlayerHealth player = other.gameObject.GetComponent<PlayerHealth>();

        switch (isEnemyProjectile)
        {
            case true:
                if (!other.isTrigger && (player || indestructible))
                {
                    if (player)
                    {
                        player.TakeDamge(1, other.gameObject.transform);
                    }
                    ObjectPool.Instance.GetObject(projectileVFX, transform, transform.rotation);
                    //Instantiate(projectileVFX, transform.position, transform.rotation);
                    ObjectPool.Instance.PushObject(gameObject);
                    //Destroy(gameObject);
                }
                break;
            case false:
                if (!other.isTrigger && (enemy || indestructible))
                {
                    ObjectPool.Instance.GetObject(projectileVFX, transform, transform.rotation);
                    //Instantiate(projectileVFX, transform.position, transform.rotation);
                    ObjectPool.Instance.PushObject(gameObject);
                    //Destroy(gameObject);
                }
                break;
        }
    }

    private void DetectFireDistance()
    {
        if (Vector3.Distance(startPos, transform.position) > projectillRange)
        {
            ObjectPool.Instance.PushObject(gameObject);
            //Destroy(gameObject);
        }

    }
    private void MoveProjectill()
    {
        transform.Translate(moveSpeed * Time.deltaTime * Vector3.right);
    }

    private IEnumerator TrailWaitActiveRoutine()
    {
        yield return new WaitForSeconds(0.05f);
        GetComponent<TrailRenderer>().emitting = true;
    }
}
