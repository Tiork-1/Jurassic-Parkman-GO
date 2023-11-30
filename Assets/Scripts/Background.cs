using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera mainCamera; // 将你的相机拖拽到这里
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        Vector3 newPos = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, transform.position.z);
        transform.position = newPos;
    }


}
