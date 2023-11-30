using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlushCrash : MonoBehaviour
{
    // Start is called before the first frame update
    private float destoryTime = 30f;
    void Start()
    {
        Destroy(gameObject, destoryTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
