using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shooter : MonoBehaviour, IEnemy
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private int burstCount;
    [SerializeField] private int projectillPerBurst;
    [SerializeField][Range(0, 359)] private float angleSpread;

    [SerializeField] private float startingDistance = .1f;
    [SerializeField] private float timeBetwenBursts;
    [SerializeField] private float restShootTime = 1f;
    [SerializeField] private bool stagger;
    [Tooltip("Stagger 必须与oscillate同步使用才有效果")]
    [SerializeField] private bool oscillate;
    [SerializeField] private bool BossShotter = false;
    private bool isShooting = false;

    private void OnValidate()
    {
        if (oscillate) { stagger = true; }
        if (!oscillate) { stagger = false; }
        if (projectillPerBurst < 1) { projectillPerBurst = 1; }
        if (burstCount < 1) { burstCount = 1; }
        if (timeBetwenBursts < 0.1f) { timeBetwenBursts = 0.1f; }
        if (restShootTime < 0.1f) { restShootTime = 0.1f; }
        if (startingDistance < 0.1f) { startingDistance = 0.1f; }
        if (angleSpread == 0) { projectillPerBurst = 1; }
        if (bulletSpeed <= 0) { bulletSpeed = 0.1f; }
    }

    public void Attack()
    {
        if (!isShooting)
        {
            StartCoroutine(ShootRoutine());
        }
    }

    private IEnumerator ShootRoutine()
    {
        isShooting = true;

        float timeBetwenProjectiles = 0f;

        TargetConeOfInfluence(out float startAngle, out float currentAngle, out float angleStep, out float endAngle);
        //震荡射击，在间隔时间内将所有子弹均匀射出
        if (stagger) { timeBetwenProjectiles = timeBetwenBursts / projectillPerBurst; }


        for (int i = 0; i < burstCount; i++)
        {
            //交错射击，在循环内反复切换射击首尾点
            if (!oscillate)
            {
                TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep, out endAngle);
            }

            if (oscillate && i % 2 != 1)
            {
                TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep, out endAngle);
            }
            else if (oscillate)
            {
                currentAngle = endAngle;
                endAngle = startAngle;
                startAngle = currentAngle;
                angleStep *= -1;
            }

            for (int j = 0; j < projectillPerBurst; j++)
            {
                Vector2 pos = FindBulletSpawnPos(currentAngle);

                GameObject newBullet = ObjectPool.Instance.GetObject(bulletPrefab, pos, Quaternion.identity);
                newBullet.transform.SetPositionAndRotation(pos, Quaternion.identity);

                //GameObject newBullet = Instantiate(bulletPrefab, pos, Quaternion.identity);
                newBullet.transform.right = newBullet.transform.position - transform.position;

                if (newBullet.TryGetComponent(out Projectill projectill))
                {
                    projectill.UpdateProjectileMoveSpeed(bulletSpeed);
                }

                currentAngle += angleStep;
                if (stagger) { yield return new WaitForSeconds(timeBetwenProjectiles); }
            }

            currentAngle = startAngle;

            if (!stagger) { yield return new WaitForSeconds(timeBetwenBursts); }

        }

        if (BossShotter)
        {
            BossModeRandomChange(GetComponent<EnemyHealth>().currentHealth, GetComponent<EnemyHealth>().startHealth);
        }
        yield return new WaitForSeconds(restShootTime);
        isShooting = false;
    }
    /// <summary>
    /// 射手Boss的随机化
    /// </summary>
    /// <param name="currentHP">当前血量</param>
    private void BossModeRandomChange(int currentHP, int maxHealth)
    {
        float hpPercent = currentHP / maxHealth;
        angleSpread = Random.Range(60, 359);
        burstCount = Mathf.CeilToInt(Random.Range(2, 4));
        projectillPerBurst = Mathf.CeilToInt(Random.Range(10f, 30f));
        timeBetwenBursts = Random.Range(0.1f + hpPercent, 0.5f + hpPercent);
        bulletSpeed = Random.Range(5f - (hpPercent * 2), 12f - (hpPercent * 2));
        if (0.5f < Random.Range(0, 1))
            stagger = false;
        else
            stagger = true;
    }
    private Vector2 FindBulletSpawnPos(float currentAngle)
    {
        float x = transform.position.x + startingDistance * Mathf.Cos(currentAngle * Mathf.Deg2Rad);
        float y = transform.position.y + startingDistance * Mathf.Sin(currentAngle * Mathf.Deg2Rad);
        Vector2 pos = new(x, y);

        return pos;
    }

    private void TargetConeOfInfluence(out float startAngle, out float currentAngle, out float angleStep, out float endAngle)
    {
        Vector2 targetDirection = PlayerController.Instance.transform.position - transform.position;

        float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        startAngle = targetAngle;
        endAngle = targetAngle;
        currentAngle = targetAngle;
        float halfAngleSpread = 0f;
        angleStep = 0;
        if (angleSpread != 0)
        {
            angleStep = angleSpread / (projectillPerBurst - 1);
            halfAngleSpread = angleSpread / 2f;
            startAngle = targetAngle - halfAngleSpread;
            endAngle = targetAngle + halfAngleSpread;
            currentAngle = startAngle;
        }
    }
}
