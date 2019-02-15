using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTask : MonoBehaviour
{
    MapTask mapTask;
    CameraTask cameraTask;
    TextAsset[] prefabText;   //現在のモードの出現マップ

    private void Awake()
    {
        mapTask = GetComponent<MapTask>();
        cameraTask = GetComponent<CameraTask>();
    }

    // Start is called before the first frame update
    void Start()
    {
        CreatePrefab(Vector2Int.zero);
        CreateMap();
    }

    // Update is called once per frame
    void Update()
    {
        CreateMap();
        MapDelete();
    }

    private void CreateMap()
    {
        while (CameraXPos() + cameraTask.whideSize / 2 > MaxPrefab().parent.transform.position.x + MaxPrefab().range)
        {
            //最後に生成した一番右上の場所
            Vector2Int createPos = new Vector2Int(MaxPrefab().range + (int)MaxPrefab().parent.transform.position.x, -MaxPrefab().rightTop + (int)MaxPrefab().parent.transform.position.y);
            CreatePrefab(createPos);
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

    private void CreatePrefab(Vector2Int vec2)
    {
        prefabText = Resources.LoadAll<TextAsset>("Txt/Stage");
        mapTask.prefabMaps.Add(mapTask.LoadMap(prefabText[Random.Range(0,prefabText.Length)].text, vec2));
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
