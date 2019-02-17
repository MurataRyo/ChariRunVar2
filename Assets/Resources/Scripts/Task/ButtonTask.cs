using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTask : MonoBehaviour
{
    public enum Value
    {
        Down,
        Up,
        Key,
    }

    public enum Name
    {
        Jump,
        Attack,
    }

    #region ボタン判定
    //プログラム的には汚いが使う時に見やすいので関数を複数用意

    //ボタンが入力されているか
    public bool ButtonIf(Name name)
    {
        KeyCode[] keyCodes = NameToKeyCode(name);
        foreach (KeyCode key in keyCodes)
        {
            if (Inif(Value.Key, key))
                return true;
        }

        return false;
    }

    //ボタンが入力されているか
    public bool ButtonDownIf(Name name)
    {
        KeyCode[] keyCodes = NameToKeyCode(name);
        foreach (KeyCode key in keyCodes)
        {
            if (Inif(Value.Down, key))
                return true;
        }

        return false;
    }

    //ボタンが入力されているか
    public bool ButtonUpIf(Name name)
    {
        KeyCode[] keyCodes = NameToKeyCode(name);
        foreach (KeyCode key in keyCodes)
        {
            if (Inif(Value.Up, key))
                return true;
        }

        return false;
    }

    #endregion

    public bool Inif(Value value, KeyCode key)
    {
        switch (value)
        {
            case Value.Down:
                return Input.GetKeyDown(key);

            case Value.Up:
                return Input.GetKeyUp(key);

            case Value.Key:
                return Input.GetKey(key);
        }
        return false;
    }

    public KeyCode[] NameToKeyCode(Name name)
    {
        List<KeyCode> keyCode = new List<KeyCode>();
        switch (name)
        {
            case Name.Jump:
                keyCode.Add(KeyCode.Space);
                break;

            case Name.Attack:
                keyCode.Add(KeyCode.Return);
                break;
        }

        return keyCode.ToArray();
    }
}