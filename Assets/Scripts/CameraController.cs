using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    public static float gameSpeed = 10f;

    public float difficulty = 1.0f;
    GameObject thisObj = null;

    private float timer = 0f;
    
    void Start()
    {
        thisObj = this.gameObject;
        gameSpeed = 10;
        Debug.Log("Start game!");
        timer = 0f;
    }

    public static void resetSpeed()
    {
        CameraController.gameSpeed = 10;
    }

    // Update is called once per frame
    void Update()
    {

        //更新speed,每隔十秒，速度加快diffyculty * x%
        //更新timer
        timer += Time.deltaTime;
        if (timer > 10)
        {
            gameSpeed = gameSpeed * (1.0f + 0.2f * difficulty);
            timer = 0f;
        }
        //Debug.Log(CameraController.gameSpeed);
        //移动camera
        Vector3 cameraPosition = thisObj.transform.position;
        cameraPosition.x += gameSpeed * 1.0f * Time.deltaTime;
        thisObj.transform.position = cameraPosition;   
    }
}
