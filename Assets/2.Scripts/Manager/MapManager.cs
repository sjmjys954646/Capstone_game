using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class MapManager : MonoBehaviour
{
    public GameObject gameManager;
    public GameObject materialManager;
    public GameObject Tile;
    public GameObject Map;
    public GameObject Player;
    public GameObject playerManager;
    public GameObject mobGenerator;
    public GameObject Timer;
    bool mapMakeFin = false;

    public List<int> mapBreakInterval = new List<int>() { 20, 60, 210 };
    public List<int> mapBreakPosTime = new List<int>() { 3,5,9 };
    //�ʹݺ� - �߹ݺ� - �Ĺݺΰ� �ٲ�� ���� �ð��ϵ�
    public List<int> whenSectionChange = new List<int>();
    public int curSection = 0;
    public bool mapBreak = false;
    public bool mapBreakStart = false;
    public int breakNum = 0;
    bool mapBreakFin = false;
    public int[,] mapTileIntTmp;
    public bool findTobreak = false;

    //�׽�Ʈ�� bake + ���� �̵� + ��generate ���� bool
    public bool RealMode;

    //��ֹ� prefab��
    public List<GameObject> obstacle = new List<GameObject>();

    //�� mapTile�� GameObjectȭ �ϱ� �� int mapTile
    public int[,] mapTileInt;

    //�� mapTile�� ���� ����
    public GameObject[,] mapTile;

    //�׸��� Ÿ�� ����
    public int[,] tileNumArr = new int[,]{ { 1, 2 }, { 2, 1 }, { 2, 2 }, { 3, 3 } };
    //��ֹ��� ���� dx
    public List<int[,]> tileTypeDX = new List<int[,]>();// ���� 6��

    public GameObject thornDamagedUI;

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
            #if UNITY_EDITOR
            UnityEditor.AI.NavMeshBuilder.ClearAllNavMeshes();
            UnityEditor.AI.NavMeshBuilder.BuildNavMesh();
            #endif
        }
    }

    private void Update()
    {
        if(mapBreak && !mapBreakStart)
        {
            mapBreakStart = true;
            //timeScale ��������
            //mapBreak Ǯ��
            StartCoroutine(mapBreakCoroutine());
        }
    }

    public void findWhichTobreak()
    {
        mapTileIntTmp = mapTileInt;
        for (int i = 0; i < row; i++)
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

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                if (mapTileIntTmp[i, j] == 1)
                    continue;

                if (mapTile[i, j].GetComponent<TileFrame>().playerOnTime >= mapBreakPosTime[curSection])
                {
                    mapTileIntTmp[i, j] = 1;
                }
            }
        }
        findTobreak = true;
    }

    void findWhichToBreakandRemake()
    {
        //�� �籸��
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
                    oneTile.GetComponent<TileFrame>().materialManager = materialManager;
                    oneTile.GetComponent<TileFrame>().tileRow = i;
                    oneTile.GetComponent<TileFrame>().tileColumn = j;

                    //���ô� ����UI�� ���� �߰�
                    if (oneTile.GetComponent<TileFrame>().index == 3)
                    {
                        oneTile.GetComponent<Thorn>().thornDamagedUI = thornDamagedUI;
                    }


                    mapTile[i, j] = oneTile;
                    oneTile.transform.parent = Map.transform;
                }
            }
        }
        mapBreakFin = true;
    }


    //intmapTile �� ������� mapTile�� �����.
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
                oneTile.GetComponent<TileFrame>().materialManager = materialManager;
                oneTile.GetComponent<TileFrame>().tileRow = i;
                oneTile.GetComponent<TileFrame>().tileColumn = j;
                //���ô� ����UI�� ���� �߰�
                if (oneTile.GetComponent<TileFrame>().index == 3)
                {
                    oneTile.GetComponent<Thorn>().thornDamagedUI = thornDamagedUI;
                    oneTile.GetComponent<Thorn>().gameManager = gameManager;
                }
                mapTile[i, j] = oneTile;
                oneTile.transform.parent = Map.transform;              
            }
        }
    }

    //��ֹ� ����
    void makeObstacle()
    {
        //��ü ������ 4*4�� ������ ���� �� ���� 3*3 �� �ش�κ��� �� ������ ����
        //�� ������ �������� ��ֹ��� ��ġ �� �� �ִ°� �Ǵ�
        //�Ұ��� �ҽ� �����Ѱ� ���� ��ġ
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
        //�׸�Ÿ�ϵ� 1,2 2,1 2,2 3,3
        for(int i = 0;i<4 ;i++)
        {
            tileTypeDX.Add(putTyle(tileNumArr[i,0] , tileNumArr[i,1]));
        }
        // ���� �� �� �õ�
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
        // tileTypeDX�� ä���ش�.
        makeTileType();
        //��ֹ� ����
        makeObstacle();
        makeMap();
        mapMakeFin = true;
    }

    IEnumerator mapDelayCharacterMove()
    {
        makeMapFull();
        yield return new WaitUntil(() => mapMakeFin);
        playerManager.GetComponent<PlayerManager>().SetPlayerPosition();
        if (RealMode)
        {
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
        Timer.GetComponent<Timer>().findMapBreakFin = true;
        mapBreakStart = false;
    }
}
