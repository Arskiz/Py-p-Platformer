using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VarHelper
{
    public static int GetIntFromBool(bool b){
        switch(b)
        {
            case true:
                return 1;
            case false:
                return 0;
        }
    }

    public static bool GetBoolFromInt(int i){
        switch(i){
            case 1:
                return true;
            case 0:
                return false;
        }
        return false;
    }

    public static bool CheckKeybindHold(KeyCode key)
    {
        if (Input.GetKey(key))
            return true;
        else
            return false;
    }

    public static bool CheckKeybindPressed(KeyCode key)
    {
        if (Input.GetKeyDown(key))
            return true;
        else
            return false;
    }
}
