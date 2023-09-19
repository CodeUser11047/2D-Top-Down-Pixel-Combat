using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapeProjectile : MonoBehaviour
{
    [SerializeField] private float duration = 1f;
    [SerializeField] private AnimationCurve animCure;
    [SerializeField] private float heightY = 3f;
    [SerializeField] private GameObject grapeProjectileShadow;
    [SerializeField] private GameObject grapeLandSplatterPrefab;
    [SerializeField] Vector3 shadowAdjust = new(0f, -0.3f, 0f);

    // private void Start()
    // {
    //     Vector3 playerPos = PlayerController.Instance.transform.position;

    //     StopAllCoroutines();
    //     StartCoroutine(ProjectileCureRoutine(transform.position, playerPos));
    // }

    private void OnEnable()
    {
        Vector3 playerPos = PlayerController.Instance.transform.position;

        StopAllCoroutines();
        StartCoroutine(ProjectileCureRoutine(transform.position, playerPos));
    }

    private IEnumerator ProjectileCureRoutine(Vector3 startPos, Vector3 endPos)
    {
        float timePassed = 0f;

        GameObject grapeShadow = ObjectPool.Instance.GetObject(grapeProjectileShadow, transform.position + shadowAdjust, Quaternion.identity);


        //GameObject grapeShadow = Instantiate(grapeProjectileShadow, transform.position + shadowAdjust, Quaternion.identity);
        while (timePassed < duration)
        {
            timePassed += Time.deltaTime;
            float linerT = timePassed / duration;
            float heightT = animCure.Evaluate(linerT);
            float height = Mathf.Lerp(0f, heightY, heightT) * 2f;

            transform.position = Vector2.Lerp(startPos, endPos, linerT) + new Vector2(0f, height);

            grapeShadow.transform.position = Vector2.Lerp(startPos, endPos, linerT);

            yield return null;
        }
        ObjectPool.Instance.GetObject(grapeLandSplatterPrefab, transform.position, Quaternion.identity);

        //Instantiate(grapeLandSplatterPrefab, transform.position, Quaternion.identity);
        ObjectPool.Instance.PushObject(grapeShadow);
        //Destroy(grapeShadow);
        ObjectPool.Instance.PushObject(gameObject);
        //Destroy(gameObject);
    }

}
