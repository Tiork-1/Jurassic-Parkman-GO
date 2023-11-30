using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class TilemapUpdate : MonoBehaviour
{

    public Tilemap tilemap;
    public TileBase tileBase;
    public Camera mainCamera;
    public Vector3Int tilePosition=Vector3Int.zero;

    private int preTile = 1;
    private int num = 0;
    private int maxTilex = 50;

    public GameObject prefabFlie;
    public GameObject prefabBrush;
    public GameObject prefabCoin;

    private float timer = 0f;
    private float lastTime = 0f;
    private float lastCoinTime = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        //先放置50块
        for (int i = 0; i < maxTilex+1; i++) {
            Vector3Int localPlace = (new Vector3Int(i, 0, (int)tilemap.transform.position.z));
            tilemap.SetTile(localPlace, tileBase);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //更新计时器
        timer += Time.deltaTime;

        //更新tile
        Vector3 posCamera = mainCamera.transform.position;
        float cameraWorldCoordinate = posCamera.x;
        UpdateTiles(cameraWorldCoordinate - 30, cameraWorldCoordinate+30);
    }

    private int nextTile(int preTile,int num,bool stayPlain=true)
    {
        //当stayPlain为真，地面不起伏变换
        if (stayPlain)
        {
            return 1;
        }

        float randomNum = Random.Range(0, 100)/100f;
        // 地面有0.2的概率变换高度
        if (preTile != 0)
        {
            if (randomNum < 0.8) return preTile;
            else if (randomNum >= 0.8 && randomNum < 0.9) return 3 - preTile;
            else return 0;
        }
        else 
        {
            if (num>= 8)
            {
                if (randomNum < 0.5) return 1;
                else return 2;
            }
            else
            {
                if (randomNum < 0.5) return 0;
                else if (randomNum >= 0.5 && randomNum < 0.8) return 1;
                else return 2;
            }
        }
    }

    public void UpdateTiles(float minWorldCoordinate,float maxWorldCoordinate)
    {
        BoundsInt bounds = tilemap.cellBounds;

        //新增Tile
        Vector3Int maxLocalPlace = (new Vector3Int(maxTilex+1, 0, (int)tilemap.transform.position.z));
        Vector3 maxWorldPlace = tilemap.CellToWorld(maxLocalPlace);
        if (maxWorldPlace.x < maxWorldCoordinate)
        {
            maxTilex += 4;
            int newTile = nextTile(preTile, num, false);
            //更新num
            if (newTile == preTile) num += 1;
            else num = 1;
            preTile = newTile;

            if (newTile == 1)
            {
                //增加一个
                for(int j = 1; j <= 4; ++j)
                {
                    Vector3Int addPlace = (new Vector3Int(maxTilex - j, 0, (int)tilemap.transform.position.z));
                    tilemap.SetTile(addPlace, tileBase);
                }

                //看是否要生成一个敌人
                if (timer > 5)
                {
                    if (timer - lastTime > 5)
                    {
                        Vector3Int localPosForEnemy = (new Vector3Int(maxTilex - 4, 0, (int)tilemap.transform.position.z));
                        Vector3 worldPosForEnemy = tilemap.CellToWorld(localPosForEnemy);
                        addEnemy(worldPosForEnemy);
                        lastTime = timer;
                    }
                    else
                    {
                        if (timer-lastCoinTime > 3)
                        {

                            Vector3Int localPosForCoin = (new Vector3Int(maxTilex - 4, 0, (int)tilemap.transform.position.z));
                            Vector3 worldPosForCoin = tilemap.CellToWorld(localPosForCoin);
                            addCoin(worldPosForCoin);
                            lastCoinTime = timer;
                        }
                    }
                }
                
                

            }
            else if (newTile == 2)
            {
                //增加一个
                for (int j = 1; j <= 4; ++j)
                {
                    Vector3Int addPlace = (new Vector3Int(maxTilex - j, 2, (int)tilemap.transform.position.z));
                    tilemap.SetTile(addPlace, tileBase);
                }

                //看是否要生成一个敌人
                if (timer > 5)
                {
                    if (timer - lastTime > 4)
                    {
                        Vector3Int localPosForEnemy = (new Vector3Int(maxTilex - 4, 2, (int)tilemap.transform.position.z));
                        Vector3 worldPosForEnemy = tilemap.CellToWorld(localPosForEnemy);
                        addEnemy(worldPosForEnemy);
                        lastTime = timer;
                    }
                }

            }
            
        }

        //擦除Tile
        for (int x = bounds.min.x; x < bounds.max.x; x++)
        {
            for (int y = bounds.min.y; y < bounds.max.y; y++)
            {
                Vector3Int localPlace = (new Vector3Int(x, y, (int)tilemap.transform.position.z));
                Vector3 place = tilemap.CellToWorld(localPlace);

                if (place.x < minWorldCoordinate)
                {

                    if (tilemap.GetTile(localPlace) != null)
                    {
                        tilemap.SetTile(localPlace, null);
                    }
                    
                }
            }
        }
    }

    private void addEnemy(Vector3 pos)
    {
        Debug.Log("add enemy!");
        //增加一个
        float prob = Random.Range(1, 100) / 100.0f;
        //生成鸟
        if (prob < 0.5)
        {
            float offset = Random.Range(1, 100) / 20.0f + 5;
            Instantiate(prefabFlie, pos + new Vector3(0,offset,0), Quaternion.identity);
        }
        //生成仙人掌
        else if (prob>=0.5 && prob < 0.8)
        {
            float offset1 = 03f;
            float offset2 = 03f;
            Instantiate(prefabBrush, pos + new Vector3(offset1,offset2,0), Quaternion.identity);
        }
        //不生成
        else
        {

        }
    }

    private void addCoin(Vector3 pos)
    {
        Debug.Log("add coin!");
        float prob = Random.Range(1, 100) / 100.0f;
        if(prob < 0.5)
        {
            float offset1 = 03f;
            float offset2 = 03f;
            Instantiate(prefabCoin, pos + new Vector3(offset1, offset2, 0), Quaternion.identity);
        }
        else if (prob >= 0.5 && prob < 0.8)
        {
            float offset1 = 03f;
            float offset2 = 03f;
            Instantiate(prefabCoin, pos + new Vector3(offset1, offset2, 0), Quaternion.identity);
            Instantiate(prefabCoin, pos + new Vector3(offset1+2f, offset2, 0), Quaternion.identity);
        }
        else
        {

        }
    }

}
