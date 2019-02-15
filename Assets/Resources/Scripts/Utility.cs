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
}

public struct GetPath
{
    public const string Prefab = "Prefab";
    public const string MapPrefab = Prefab + "/MapPrefab";
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