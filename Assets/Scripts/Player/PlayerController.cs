using System.Collections;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    public bool FacingLeft { get { return facingLeft; } set { facingLeft = value; } }

    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float dashSpeed = 4f;
    [SerializeField] private TrailRenderer myTrailRenderer;
    [SerializeField] private float dashCD = .25f;
    [SerializeField] private Transform weaponCollider;

    private PlayerControls playerControls;
    private KnockBack knockBack;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRenderer;
    private bool facingLeft = false;
    private bool isDashing = false;

    protected override void Awake()
    {
        base.Awake();

        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        knockBack = GetComponent<KnockBack>();
    }

    private void OnEnable()
    {
        playerControls?.Enable();
    }
    private void OnDisable()
    {
        playerControls?.Disable();
    }

    private void Start()
    {
        playerControls.Combot.Dash.performed += _ => Dash();

        ActiveInventory.Instance.EquipStartingWeapon();
    }
    private void Update()
    {
        PlayerInput();
    }

    private void FixedUpdate()
    {
        Move();

        AdjustPlayerFacingDirection();
    }

    public Transform GetWeaponCollider()
    {
        return weaponCollider;
    }
    private void PlayerInput()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();

        myAnimator.SetFloat("Speed", Mathf.Abs(movement.x) + Mathf.Abs(movement.y));
    }

    private void Move()
    {
        if (knockBack.gettingKnockedBack || PlayerHealth.Instance.IsDead||DialogManager.Instance.istalking)
        {
            return;
        }
        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }

    private void AdjustPlayerFacingDirection()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        if (mousePos.x > playerScreenPoint.x)
        {
            mySpriteRenderer.flipX = false;
            FacingLeft = false;
        }
        else
        {
            mySpriteRenderer.flipX = true;
            FacingLeft = true;
        }
    }
    //冲刺
    private void Dash()
    {
        if (!isDashing && Stamina.Instance.CurrentStamina > 0)
        {
            Stamina.Instance.UseStamia();

            isDashing = true;
            moveSpeed *= dashSpeed;
            myTrailRenderer.emitting = true;
            StartCoroutine(EndDashRoutine());
        }
    }
    private IEnumerator EndDashRoutine()
    {
        float dashTime = .2f;
        yield return new WaitForSeconds(dashTime);
        moveSpeed /= dashSpeed;
        yield return new WaitForSeconds(.05f);
        myTrailRenderer.emitting = false;
        yield return new WaitForSeconds(dashCD);
        isDashing = false;
    }
}