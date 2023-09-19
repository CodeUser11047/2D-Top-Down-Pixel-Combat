using System.Collections;
using UnityEngine;
public enum EnemyState
{
    Roming,
    Attacking,
    Boss
}
public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float roamChangeDirFloat = 2f;
    [SerializeField] private float attackRange = 0f;
    [SerializeField] private MonoBehaviour enemyType;
    [SerializeField] private bool stopMoveInAttacking = false;



    private Vector2 roamPos;
    private float timeRoaming = 0f;
    public EnemyState state;
    private EnemyPathfinding enemyPathfinding;

    private void Awake()
    {
        enemyPathfinding = GetComponent<EnemyPathfinding>();
        if (state == EnemyState.Boss) { state = EnemyState.Boss; }
        else
            state = EnemyState.Roming;

    }
    private void Start()
    {
        roamPos = GetRomingPosition();
    }

    private void Update()
    {
        MovementStateControl();
    }

    private void MovementStateControl()
    {
        switch (state)
        {
            default:
            case EnemyState.Roming:
                Roming();
                break;

            case EnemyState.Attacking:
                Attacking();
                break;
            case EnemyState.Boss:
                Roming();
                Attacking();
                break;
        }
    }

    private void Roming()
    {
        timeRoaming += Time.deltaTime;

        enemyPathfinding.MoveTo(roamPos);

        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) < attackRange)
        {
            if (state == EnemyState.Boss) { state = EnemyState.Boss; }
            else
                state = EnemyState.Attacking;
        }

        if (timeRoaming > roamChangeDirFloat)
        {
            roamPos = GetRomingPosition();
        }
    }

    private void Attacking()
    {
        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) > attackRange)
        {
            if (state == EnemyState.Boss) { state = EnemyState.Boss; }
            else
                state = EnemyState.Roming;
        }

        (enemyType as IEnemy)?.Attack();

        if (stopMoveInAttacking)
        {
            enemyPathfinding.StopMoving();
        }
        else
        {
            enemyPathfinding.MoveTo(roamPos);
        }
    }

    // private IEnumerator AttackCooldownRoutine()
    // {
    //     yield return new WaitForSeconds(attackCooldown);

    //     canAttack = true;
    // }

    private Vector2 GetRomingPosition()
    {
        timeRoaming = 0f;
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
    }
}
