using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicLaser : MonoBehaviour
{
    [SerializeField] private float laserGrowTime = 2f;

    private bool isGrowing = true;
    private float laserRange;
    private SpriteRenderer spriteRenderer;
    private CapsuleCollider2D myCollider;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        myCollider = GetComponent<CapsuleCollider2D>();
    }

    private void Start()
    {
        LaserFaceMouse();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Indestructible>() && !other.isTrigger)
            isGrowing = false;
    }

    public void UpdateLaserRange(float laserRange)
    {
        this.laserRange = laserRange;
        StartCoroutine(IncreaseLaserLengthRoutine());
    }

    private IEnumerator IncreaseLaserLengthRoutine()
    {
        float timePass = 0f;
        while (spriteRenderer.size.x < laserRange && isGrowing)
        {
            timePass += Time.deltaTime;
            float linearT = timePass / laserGrowTime;
            //sprite
            spriteRenderer.size = new Vector2(Mathf.Lerp(1f, laserRange, linearT), 1f);

            //collider
            myCollider.size = new Vector2(Mathf.Lerp(1f, laserRange, linearT), myCollider.size.y);
            myCollider.offset = new Vector2(Mathf.Lerp(1f, laserRange, linearT) / 2, myCollider.offset.y);

            yield return null;
        }

        StartCoroutine(GetComponent<SpriteFade>().SlowFadeRoutine());

    }

    private void LaserFaceMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 dir = transform.position - mousePos;

        transform.right = -dir;
    }

}
