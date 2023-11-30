using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update

    public Camera cam;
    private int offset = 10;

    private GameObject thisObj;

    private Rigidbody2D rb;

    private Animator animator;

    public float glideDescentRate = 0f;
    public float normalGravityScale = 10f;

    //初试玩家位置，用来计算距离
    private float initialPositionX = 0f;

    //0: running 1:jumping 2: double jumping 3: slide
    private int playerState = 0;

    //显示分数
    public GameObject textScore;

    private TMP_Text textMeshPro;

    public GameObject textDistance;

    private TMP_Text textMeshProDistance;

    private int score = 0;

    public float jumpForce; // 跳跃力度  

    public GameObject restartButton;

    void Start()
    {
        //获取本物体
        thisObj = this.gameObject;
        //初始化重力加速度参数
        glideDescentRate = 0f;
        normalGravityScale = 10f;
        // 获取Rigidbody2D组件  
        rb = GetComponent<Rigidbody2D>();
        //跳跃高度
        jumpForce = 30.0f;
        //animator
        animator = GetComponent<Animator>();
        //Text 组件获取
        textMeshPro = textScore.GetComponent<TMP_Text>();
        textMeshProDistance = textDistance.GetComponent<TMP_Text>();
        //initial
        textMeshPro.text = "SCORE : 00000000";
        textMeshProDistance.text = "DISTANCE : 0000000000";

        //button
        restartButton.SetActive(false);

        Button button1 = restartButton.GetComponent<Button>();
        //add event
        button1.onClick.AddListener(RestartGame);

        initialPositionX = gameObject.transform.position.x;
    }

    private void RestartGame()
    {
        Debug.Log("Restart Game!");
        CameraController.gameSpeed = 10;
        Debug.Log(CameraController.gameSpeed);
        SceneManager.LoadScene("MainScene");
    }

    // 
    bool IsPlayerVisible()
    {
        Vector3 screenPoint = cam.WorldToViewportPoint(gameObject.transform.position);
        return screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
    }

    // Update is called once per frame
    void Update()
    {
        //更新小恐龙位置
        Vector3 playerPosition = thisObj.transform.position;
        Vector3 cameraPosition = cam.transform.position;

        //判断恐龙是否出界
        if (!IsPlayerVisible())
        {
            dead();
        }

        if (playerPosition.x < cameraPosition.x - offset ) 
        {
            playerPosition.x += CameraController.gameSpeed * 1.0f * 1.2f * Time.deltaTime;
            
        }
        else if (playerPosition.x > cameraPosition.x - offset + 3)
        {
            playerPosition.x += CameraController.gameSpeed * 1.0f * 0.9f * Time.deltaTime;
        }
        else
        {
            playerPosition.x += CameraController.gameSpeed * 1.0f * Time.deltaTime;
        }

        thisObj.transform.position = playerPosition;


        // 检测是否按下了空格键  
        if ((playerState<2) && Input.GetKeyDown(KeyCode.Space))
        {
            //Debug.Log(jumpForce);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            //Debug.Log(rb.velocity);
            playerState += 1;
        }

        // 当playerstate=2且按下跳跃时开始滑翔
        else if(playerState==2 && Input.GetKeyDown(KeyCode.Space))
        {
            playerState = 3;
            rb.gravityScale = glideDescentRate;
            Vector2 v = rb.velocity;
            v.y = -2.0f;
            rb.velocity = v;
        }
        else if(playerState==3 && Input.GetKeyUp(KeyCode.Space))
        {
            rb.gravityScale = normalGravityScale;
        }

        getDistance();
    }



    // 当角色落地时，重置跳跃状态  
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            rb.gravityScale = normalGravityScale;
            playerState = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("enemy"))
        {
            // 触发主角死亡逻辑，比如播放死亡动画、停止游戏等
            Debug.Log("Player is dead!");
            // 在这里添加其他主角死亡的逻辑
            dead();
        }

        if (other.CompareTag("coin"))
        {
            //获得金币
            getCoin();
        }
    }

    private void dead()
    {
        //播放死亡动画
        animator.Play("boyDead");
        CameraController.gameSpeed = 0;
        //速度置零，重力置零
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(0f,0f);
        Invoke("Hide",1.05f);

        //restart button
        ShowRestartButton();
    }

    private void getDistance()
    {
        textMeshProDistance.text = "DISTANCE : " + formatNum((int)(gameObject.transform.position.x - initialPositionX),10);
    }

    private void getCoin()
    {
        score += 1;
        textMeshPro.text = "SCORE : " + formatNum(score, 8);
    }
    private string formatNum(int num,int len)
    {
        int l = 0;
        string res = num.ToString();
        while (num > 0)
        {
            num /= 10;
            l++;
        }
        for(int i = 0; i < len - l; ++i)
        {
            res = "0" + res;
        }
        Debug.Log(res);
        return res;
    }

    void ShowRestartButton()
    {
        restartButton.SetActive(true);
    }

    void HideRestartButton()
    {
        //restartButton.SetActive(false);
    }

    void Hide()
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.enabled = false; // 禁用渲染器，使玩家不可见
        }
    }


}
