using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public GameObject Tile;
    public GameObject Map;
    public GameObject Player;

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

    public int row = 80;
    public int column = 80;
    // Start is called before the first frame update
    void Start()
    {
        mapTile = new GameObject[row, column];
        mapTileInt = new int[row, column];
        // tileTypeDX�� ä���ش�.
        makeTileType();
        //��ֹ� ����
        makeObstacle();
        makeMap();

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
                    oneTile = Instantiate(obstacle[mapTileInt[i, j]], new Vector3((float)(i + 0.5), 0.5f, (float)(j + 0.5)), Quaternion.identity);
                else
                    oneTile = Instantiate(obstacle[mapTileInt[i, j]], new Vector3((float)(i + 0.5), 0, (float)(j + 0.5)), Quaternion.identity);
                oneTile.GetComponent<TileFrame>().player = Player.GetComponent<Player>();
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
}
