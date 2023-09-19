
using System.Collections;
using UnityEngine;

public class Sword : MonoBehaviour, IWeapon
{
    [SerializeField] private GameObject slashAnimationPrefab;
    [SerializeField] private Transform slashAnimationSpwanPoint;
    [SerializeField] private WeaponInfo weaponInfo;

    private Transform weaponCollider;
    private Animator myAnimator;

    private GameObject slashAnim;


    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        weaponCollider = PlayerController.Instance.GetWeaponCollider();
        weaponCollider.gameObject.SetActive(false);

        slashAnimationSpwanPoint = GameObject.Find("SlashAnimationSpwanPoint").transform;
    }

    private void OnDisable()
    {
        weaponCollider.gameObject.SetActive(false);
    }


    private void Update()
    {

        MouseFollowWithOffest();
    }

    public void Attack()
    {
        //isAttacking = true;
        myAnimator.SetTrigger("Attack");
        weaponCollider.gameObject.SetActive(true);


        slashAnim = Instantiate(slashAnimationPrefab, slashAnimationSpwanPoint.position, Quaternion.identity);
        slashAnim.transform.parent = this.transform.parent;
    }


    public void DoneAttackAnimEvent()
    {
        weaponCollider.gameObject.SetActive(false);
    }
    public void SwingUpFlipAnimationEvent()
    {
        slashAnim.transform.rotation = Quaternion.Euler(-180, 0, 0);
        if (PlayerController.Instance.FacingLeft)
        {
            slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    public void SwingDownFlipAnimationEvent()
    {
        slashAnim.transform.rotation = Quaternion.Euler(0, 0, 0);
        if (PlayerController.Instance.FacingLeft)
        {
            slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    private void MouseFollowWithOffest()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(PlayerController.Instance.transform.position);
        float angle = Mathf.Atan2(mousePos.y - PlayerController.Instance.transform.position.y, mousePos.x - PlayerController.Instance.transform.position.x) * Mathf.Rad2Deg;
        if (mousePos.x < playerScreenPoint.x)
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 180, angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }
}
