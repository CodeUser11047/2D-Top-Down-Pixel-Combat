using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Pickup : MonoBehaviour
{
    private enum PickUpType
    {
        Gold,
        Health,
        Stamina
    }

    [SerializeField] private float pickupDistance = 5f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float speedIncreasRate = .5f;
    [SerializeField] private AnimationCurve animCure;
    [SerializeField] private float heightY = 1.5f;
    [SerializeField] private float popDuration = 1f;
    [SerializeField] private PickUpType pickUpType;
    private Vector3 moveDir;
    private Rigidbody2D rb;
    private float timePassed = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {

        StartCoroutine(AnimCureSpawnRoutine());
        // float angel = UnityEngine.Random.Range(-30f, 30f);
        // rb.velocity = Quaternion.AngleAxis(angel, Vector3.forward) * Vector3.up * 7f;
    }

    private void Update()
    {
        timePassed += Time.deltaTime;

        Vector3 playerPos = PlayerController.Instance.transform.position;

        if (Vector3.Distance(playerPos, transform.position) < pickupDistance)
        {
            moveDir = (playerPos - transform.position).normalized;
            moveSpeed += speedIncreasRate;
        }
        else if (Vector3.Distance(playerPos, transform.position) >= pickupDistance)
        {
            moveDir = Vector3.zero;
            moveSpeed = 0f;
        }
    }
    private void FixedUpdate()
    {
        if (timePassed < .5f) { return; }

        rb.gravityScale = 0f;
        rb.velocity = moveSpeed * Time.fixedDeltaTime * moveDir;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            DetectPickUpType();
            ObjectPool.Instance.PushObject(gameObject);
            //Destroy(gameObject);
        }
    }

    private IEnumerator AnimCureSpawnRoutine()
    {
        Vector2 startPos;
        Vector2 endPos;

        startPos = transform.position;
        endPos = startPos + new Vector2(UnityEngine.Random.Range(-3, 3), UnityEngine.Random.Range(-1, 1));
        float timePassed = 0f;

        while (timePassed < popDuration)
        {
            timePassed += Time.deltaTime;
            float linerT = timePassed / popDuration;
            float heightT = animCure.Evaluate(linerT);
            float height = Mathf.Lerp(0f, heightY, heightT) * 2f;

            transform.position = Vector2.Lerp(startPos, endPos, linerT) + new Vector2(0f, height);

            yield return null;
        }
    }

    private void DetectPickUpType()
    {
        switch (pickUpType)
        {
            case PickUpType.Gold:
                EconomyManagement.Instance.UpdateCurrentGold();
                break;

            case PickUpType.Health:
                PlayerHealth.Instance.GetHealth();
                break;

            case PickUpType.Stamina:
                Stamina.Instance.RefreshStamia();
                break;
        }
    }
}
