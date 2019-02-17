using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility : MonoBehaviour
{
    public static ButtonTask GetButton()
    {
        return GameObject.FindGameObjectWithTag(GetTag.Task).GetComponent<ButtonTask>();
    }
}

public struct GetTag
{
    public const string Task = "TaskManager";
    public const string Block = "Block";
    public const string Sword = "Sword";
    public const string Player = "Player";
}

public struct GetPath
{
    public const string Prefab = "Prefab";
    public const string MapPrefab = Prefab + "/MapPrefab";

    public const string GamePrefab = Prefab + "/GamePrefab";

    public const string Txt = "Txt";
    public const string StageTxt =  Txt + "/Stage";
    public const string MainStage =  StageTxt + "/MainStage";
}

#region Interface

public interface IGravity
{
    bool IsGround();
    float GravitySize();
    float Gravity();
    void GravityChange();
}

#endregion