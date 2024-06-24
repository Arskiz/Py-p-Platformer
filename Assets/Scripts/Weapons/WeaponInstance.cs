using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponInstance : MonoBehaviour
{
    // List of all the weapons available. All weapons set accordingly in the inspector.
    [Header("Essential Details:")]
    public Sprite weaponSprite;
    public string weaponName;
    public int weaponID;
    public WeaponTypes.Types WeaponType;
    public float offsetX;
    public float offsetY;
    public float offsetAimingX;
    public float offsetAimingY;

    [Header("Weapon Stats:")]
    public int maxAmmo;
    public int maxReserve;
    public int currentAmmo;
    public int currentReserve;
    public float pickupRange;
    public float fireRate;
    public float emptyClipTime;
    public float recoil;
    public float recoilVelocity;
    public AudioClip emptyClipSound;
    public AudioClip reloadSound;
    public AudioClip shootOneSound;
    public AudioClip shootConstantlySound;

    void Awake(){
        weaponSprite = transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
    }
}
