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
        offset.x += speedX * Time.unscaledDeltaTime;
        offset.y += speedY * Time.unscaledDeltaTime;

        mat.mainTextureOffset = offset;
    }
}
