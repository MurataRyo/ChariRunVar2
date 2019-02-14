using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility : MonoBehaviour
{
    
}

public struct GetTag
{
    public const string Block = "Block";
}

public struct Button
{
    public static bool JumpButton()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }
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