using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapTask : MonoBehaviour
{
    public enum MapId
    {
        None,
        Block,
        SkyBlock,
        SkyBlockLeft,
        SkyBlockRight,
    }

    public static GameObject[] mapPrefab;
    public List<PrefabMap> prefabMaps = new List<PrefabMap>();

    private void Start()
    {

    }

    //文字列からマップの生成
    public PrefabMap LoadMap(string mapData, Vector2Int createPos)
    {
        int[][] intData = LoadMap(MapData(mapData));

        GameObject parent = new GameObject();
        PrefabMap prefabMap = new PrefabMap(intData, parent);
        parent.transform.position = (Vector2)createPos + new Vector2Int(0, prefabMap.leftTop);
        
        CreateMap(intData , parent);

        return prefabMap;
    }

    public string[][] MapData(string mapData)
    {
        //横の列に変換
        string[] mapData2 = mapData.Split(char.Parse(","));

        //1マスごとへ変換
        string[][] mapData3;
        mapData3 = new string[mapData2.Length][];
        for (int i = 0; i < mapData2.Length; i++)
        {
            mapData3[i] = mapData2[i].Split(char.Parse("."));
        }

        return mapData3;
    }

    //マスの文字列をマップデータ(int)へ変換
    public int[][] LoadMap(string[][] mapData)
    {
        //戻り値の宣言
        int[][] rInt = new int[mapData.Length][];

        //文字列からintへ変換
        for (int i = 0; i < mapData.Length; i++)
        {
            rInt[i] = new int[mapData[i].Length];
            for (int j = 0; j < mapData[i].Length; j++)
            {
                rInt[i][j] = int.Parse(mapData[i][j]);
            }
        }

        return rInt;
    }

    //マップの生成
    public void CreateMap(int[][] mapData, GameObject parent)
    {
        for (int i = 0; i < mapData.Length; i++)
        {
            for (int j = 0; j < mapData[i].Length; j++)
            {
                //Noneは何もないのでとばす
                if (mapData[i][j] == (int)MapId.None)
                    continue;

                CreateBlock(mapData[i][j], new Vector2Int(j, -i), parent);
            }
        }
    }

    //ブロックの生成
    public GameObject CreateBlock(int mapId, Vector2Int createPos,GameObject parent)
    {
        if (mapPrefab == null)
            Loadobject();

        GameObject go = Instantiate(mapPrefab[mapId]);
        go.transform.parent = parent.transform;
        go.transform.localPosition = (Vector2)createPos;

        return go;
    }

    //オブジェクトロード※初回のみ
    public void Loadobject()
    {
        int value = Enum.GetValues(typeof(MapId)).Length;
        mapPrefab = new GameObject[value];

        for (int i = 0; i < value; i++)
        {
            mapPrefab[i] = Resources.Load<GameObject>(GetPath.MapPrefab + "/" + Enum.GetName(typeof(MapId), i));
        }
    }
}

public class PrefabMap
{
    public GameObject parent;
    public int leftTop;
    public int rightTop;
    public int range;

    public PrefabMap(int[][] mapData, GameObject parent)
    {
        this.parent = parent;
        range = mapData[0].Length;
        leftTop = TopNum(mapData, 0);
        rightTop = TopNum(mapData, mapData[0].Length - 1);
    }


    private int TopNum(int[][] mapData, int num)
    {
        //列のにブロックがあるか調べる
        for (int i = 0; i < mapData.Length; i++)
        {
            if (mapData[i][num] == (int)MapTask.MapId.Block)
            {
                return i;
            }
                
        }

        Debug.Log("エラー正しくないです" + num);
        //何もなければ一番上
        return 0;
    }
}
