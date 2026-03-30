using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stripes : MonoBehaviour
{
    public float speedX;
    public float speedY;

    private Material mat;
    private Vector2 offset;

    void Start()
    {
        mat = GetComponent<SpriteRenderer>().material;
    }

    void Update()
    {
        offset.x += speedX * Time.deltaTime;
        offset.y += speedY * Time.deltaTime;

        mat.mainTextureOffset = offset;
    }
}
