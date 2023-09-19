using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [Range(-1, 1)]
    [SerializeField] private float parallaxOffest = -0.15f;
    private Camera cam;
    private Vector2 startPos;
    private Vector2 travel => (Vector2)cam.transform.position - startPos;

    private void Awake()
    {
        cam = Camera.main;
    }
    private void Start()
    {
        startPos = transform.position;
    }
    private void FixedUpdate()
    {
        transform.position=startPos+travel*parallaxOffest;
    }
}