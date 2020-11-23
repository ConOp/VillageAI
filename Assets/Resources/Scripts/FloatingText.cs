using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    // Start is called before the first frame update
    float destroytime = 1f;
    Vector2 offset = new Vector2(0, 1);
    void Start()
    {
        Destroy(gameObject, destroytime);
        transform.localPosition = new Vector2(transform.localPosition.x + offset.x,transform.localPosition.y+offset.y);
    }
}
