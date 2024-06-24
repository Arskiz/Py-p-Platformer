
using System.Collections.Generic;
using UnityEngine;

public class WeaponList : MonoBehaviour
{
    public List<string> weaponNames = new List<string>();
    
    public void Awake()
    {
        // ID 0 = AK-47
        weaponNames.Add("AK-47");
    }
    
    public enum weaponID{
        AK47
    }
}
