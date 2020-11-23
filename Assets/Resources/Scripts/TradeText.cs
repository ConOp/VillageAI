using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeText : MonoBehaviour
{
    // Start is called before the first frame update
    float destroytime = 1.3f;
    void Start()
    {
        Destroy(gameObject, destroytime);
    }
}
