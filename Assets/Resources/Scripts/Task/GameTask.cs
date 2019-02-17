using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTask : MonoBehaviour
{
    MapTask mapTask;
    CameraTask cameraTask;
    TextAsset[] prefabText;   //現在のモードの出現マップ

    // Start is called before the first frame update
    void Start()
    {
        mapTask = GetComponent<MapTask>();
        cameraTask = GetComponent<CameraTask>();
        prefabText = Resources.LoadAll<TextAsset>(GetPath.MainStage);

        CreatePrefab(Vector2Int.zero, StartMapData());
        CreateMap(RandomMapData());
    }

    // Update is called once per frame
    void Update()
    {
        CreateMap(RandomMapData());
        MapDelete();
    }

    private void CreateMap(string mapData)
    {
        while (CameraXPos() + cameraTask.whideSize / 2 > MaxPrefab().parent.transform.position.x + MaxPrefab().range)
        {
            //最後に生成した一番右上の場所
            Vector2Int createPos = new Vector2Int(MaxPrefab().range + (int)MaxPrefab().parent.transform.position.x, -MaxPrefab().rightTop + (int)MaxPrefab().parent.transform.position.y);
            CreatePrefab(createPos, mapData);
        }
    }

    private void MapDelete()
    {
        if(CameraXPos() - cameraTask.whideSize / 2 > MinPrefab().parent.transform.position.x + MaxPrefab().range)
        {
            Destroy(MinPrefab().parent.gameObject);
            mapTask.prefabMaps.RemoveAt(0);
        }
    }

    private string PathToString(string path)
    {
        string data = Resources.Load<TextAsset>(path).text;
        return data;
    }

    //最初の生成
    private string StartMapData()
    {
        return PathToString(GetPath.StageTxt + "/StartStage");
    }

    //特殊な条件で変更する場合はここで変更する
    private string MainMapData()
    {
        return RandomMapData();
    }

    private string RandomMapData()
    {
        return prefabText[Random.Range(0, prefabText.Length)].text;
    }

    private void CreatePrefab(Vector2Int vec2,string mapdata)
    {
        mapTask.prefabMaps.Add(mapTask.LoadMap(mapdata, vec2));
    }

    //PrefabMapのショートカット
    private PrefabMap MinPrefab()
    {
        return mapTask.prefabMaps[0];
    }

    private PrefabMap MaxPrefab()
    {
        return mapTask.prefabMaps[mapTask.prefabMaps.Count - 1];
    }

    //カメラのxPos
    private float CameraXPos()
    {
        return cameraTask.cameraObject.transform.position.x;
    }
}
