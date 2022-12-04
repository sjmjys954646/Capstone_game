using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.AI;


public class MapManager : MonoBehaviour
{
    public GameObject Tile;
    public GameObject Map;
    public GameObject Player;
    public GameObject playerManager;
    public GameObject mobGenerator;
    public GameObject Timer;
    bool mapMakeFin = false;

    public List<int> mapBreakInterval = new List<int>() { 20, 60, 210 };
    public List<int> mapBreakPosTime = new List<int>() { 3,5,9 };
    //초반부 - 중반부 - 후반부가 바뀌는 지점 시간일듯
    public List<int> whenSectionChange = new List<int>();
    public int curSection = 0;
    public bool mapBreak = false;
    public bool mapBreakStart = false;
    public int breakNum = 0;
    bool mapBreakFin = false;

    //테스트용 bake + 유저 이동 + 몹generate 생성 bool
    public bool RealMode;

    //장애물 prefab들
    public List<GameObject> obstacle = new List<GameObject>();

    //각 mapTile을 GameObject화 하기 전 int mapTile
    public int[,] mapTileInt;

    //각 mapTile에 대한 관리
    public GameObject[,] mapTile;

    //네모형 타일 예시
    public int[,] tileNumArr = new int[,]{ { 1, 2 }, { 2, 1 }, { 2, 2 }, { 3, 3 } };
    //장애물을 세울 dx
    public List<int[,]> tileTypeDX = new List<int[,]>();// 현재 6개

    public int row = 80;
    public int column = 80;

    // Start is called before the first frame update
    void Start()
    {
        mapTile = new GameObject[row, column];
        mapTileInt = new int[row, column];

        StartCoroutine(mapDelayCharacterMove());

        if(RealMode)
        {
            NavMeshBuilder.ClearAllNavMeshes();
            NavMeshBuilder.BuildNavMesh();
        }
    }

    private void Update()
    {
        if(mapBreak && !mapBreakStart)
        {
            mapBreakStart = true;
            //timeScale 돌려놓고
            //mapBreak 풀고
            StartCoroutine(mapBreakCoroutine());
        }
    }

    void findWhichToBreakandRemake()
    {
        int[,] mapTileIntTmp = mapTileInt;
        for(int i = 0; i < row ;i++)
        {
            mapTileIntTmp[i, 0 + breakNum] = 1;
            mapTileIntTmp[i, 0 + breakNum + 1] = 1;
            mapTileIntTmp[i, row - breakNum - 1] = 1;
            mapTileIntTmp[i, row - breakNum - 2] = 1;
        }

        for (int i = 0; i < column; i++)
        {
            mapTileIntTmp[0 + breakNum, i] = 1;
            mapTileIntTmp[0 + breakNum + 1, i] = 1;
            mapTileIntTmp[column - breakNum - 1, i] = 1;
            mapTileIntTmp[column - breakNum - 1 - 1, i] = 1;
        }

        for(int i = 0;i<row ;i++)
        {
            for(int j = 0;j<column ;j++)
            {
                if (mapTileIntTmp[i, j] == 1)
                    continue;

                if(mapTile[i, j].GetComponent<TileFrame>().playerOnTime >= mapBreakPosTime[curSection])
                {
                    mapTileIntTmp[i, j] = 1;
                }
            }
        }

        //맵 재구성
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                if (mapTileIntTmp[i, j] == 1 && mapTile[i, j].GetComponent<TileFrame>().index != 1)
                {
                    GameObject removeTile = mapTile[i, j];
                    Destroy(removeTile);
                    GameObject oneTile;
                    oneTile = Instantiate(obstacle[1], new Vector3((float)(i * 2 + 1), 0, (float)(j * 2 + 1)), Quaternion.identity);
                    oneTile.GetComponent<TileFrame>().player = Player.GetComponent<Player>();
                    oneTile.GetComponent<TileFrame>().tileRow = i;
                    oneTile.GetComponent<TileFrame>().tileColumn = j;
                    mapTile[i, j] = oneTile;
                    oneTile.transform.parent = Map.transform;
                }
            }
        }
        mapBreakFin = true;
    }


    //intmapTile 을 기반으로 mapTile을 만든다.
    void makeMap()
    {
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                GameObject oneTile;
                if (mapTileInt[i, j] == 4)
                    oneTile = Instantiate(obstacle[mapTileInt[i, j]], new Vector3((float)(i*2 + 1), 1f, (float)(j*2 + 1)), Quaternion.identity);
                else
                    oneTile = Instantiate(obstacle[mapTileInt[i, j]], new Vector3((float)(i*2 + 1), 0, (float)(j*2 + 1)), Quaternion.identity);
                oneTile.GetComponent<TileFrame>().player = Player.GetComponent<Player>();
                oneTile.GetComponent<TileFrame>().tileRow = i;
                oneTile.GetComponent<TileFrame>().tileColumn = j;
                mapTile[i, j] = oneTile;
                oneTile.transform.parent = Map.transform;
                
            }
        }
    }

    //장애물 생성
    void makeObstacle()
    {
        //전체 구역을 4*4로 나눠서 왼쪽 위 기준 3*3 중 해당부분의 한 지점을 선택
        //이 지점을 기준으로 장애물을 설치 할 수 있는가 판단
        //불가능 할시 가능한곳 까지 설치
        for(int i = 0;i<row/4 ;i++)
        {
            for(int j = 0;j <column/4 ;j++)
            {
                int randX = 4 * i + Random.Range(0, 2);
                int randZ = 4 * j + Random.Range(0, 2);
                int randObstacle = Random.Range(1, obstacle.Count);
                int randTile = Random.Range(0, tileTypeDX.Count );
                fillMapTileInt(randX, randZ, randObstacle, randTile); 
            }
        }
    }

    void fillMapTileInt(int randX, int randZ, int randObstacle, int randTile)
    {
        int[,] selectedTile = tileTypeDX[randTile];
        for(int i = 0; i < 4;i++)
        {
            for(int j = 0;j < 4 ;j++)
            {
                if (randX + i < 0 || randX + i >= row || randZ + j < 0 || randZ + j >= column)
                    continue;
                if (selectedTile[i, j] == 0)
                    continue;
                if (mapTileInt[randX + i, randZ + j] != 0)
                    continue;
                mapTileInt[randX + i, randZ + j] = randObstacle;
            }
        }
    }

    void makeTileType()
    {
        //네모타일들 1,2 2,1 2,2 3,3
        for(int i = 0;i<4 ;i++)
        {
            tileTypeDX.Add(putTyle(tileNumArr[i,0] , tileNumArr[i,1]));
        }
        // ㄱ자 ㄴ 자 시도
        int[,] p = putTyle(1, 2);
        p[1, 1] = 1;
        tileTypeDX.Add(p);
        p = putTyle(2, 1);
        p[1, 1] = 1;
        tileTypeDX.Add(p);
    }

    int[,] putTyle(int x, int z)
    {
        int[,] d = new int[4, 4];
        for(int i = 0;i<x ;i++)
        {
            for(int j = 0;j<z ;j++)
            {
                d[i,j]++;
            }
        }

        return d;
    }

    void makeMapFull()
    {
        // tileTypeDX를 채워준다.
        makeTileType();
        //장애물 생성
        makeObstacle();
        makeMap();
        mapMakeFin = true;
    }

    IEnumerator mapDelayCharacterMove()
    {
        makeMapFull();
        yield return new WaitUntil(() => mapMakeFin);
        if(RealMode)
        {
            playerManager.GetComponent<PlayerManager>().SetPlayerPosition();
            mobGenerator.GetComponent<MobGenerater>().generateReady = true;
        }
    }

    IEnumerator mapBreakCoroutine()
    {
        findWhichToBreakandRemake();
        yield return new WaitUntil(() => mapBreakFin);
        mapBreak = false;
        mapBreakFin = false;
        Time.timeScale = 1f;
        breakNum += 2;
        Timer.GetComponent<Timer>().intervalSec = 0;
        mapBreakStart = false;
    }
}
