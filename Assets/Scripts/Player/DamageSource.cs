using UnityEngine;

public class DamageSource : MonoBehaviour
{
    private int damageAmount = 1;

    private void Start()
    {
        MonoBehaviour currentActiveWeapon = ActiveWeapon.Instance.CurrentActiveWeapon;
        damageAmount = (currentActiveWeapon as IWeapon).GetWeaponInfo().weaponDamage;

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
        enemyHealth?.TakeDamge(damageAmount);
    }
}
