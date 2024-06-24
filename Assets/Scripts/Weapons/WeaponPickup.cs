using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class WeaponPickup : MonoBehaviour
{
    public int pickUpWeaponID;
    public Sprite weaponSprite;
    GameObject wpn;
    Inventory inv;
    Weapon weaponScript;
    WeaponList wpList;

    bool isInstaPickUpable;

    void Start()
    {
        wpList = FindAnyObjectByType<WeaponList>();
        Debug.Log($"WeaponID: {pickUpWeaponID}, Name: {wpList.weaponNames[pickUpWeaponID]}");
        wpn = Resources.Load<GameObject>("Weapons/Prefabs/" + wpList.weaponNames[pickUpWeaponID]);
        inv = FindAnyObjectByType<Inventory>();
        weaponScript = FindAnyObjectByType<Weapon>();
        this.gameObject.AddComponent<SpriteRenderer>().sprite = weaponSprite;
        this.GetComponent<SpriteRenderer>().spriteSortPoint = SpriteSortPoint.Pivot;
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0f);
        
    }

    public void PickUp()
    {

        int upcomingWeaponIndex;
        if (inv.weaponsInventory != null)
        {
            upcomingWeaponIndex = inv.weaponsInventory.Count();
        }
        else
        {
            upcomingWeaponIndex = 0;
        }
        if (wpn != null)
        {
            GameObject weapon = Instantiate(wpn, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
            inv.weaponsInventory.Add(weapon);
            weaponScript.weapons.Add(weapon.GetComponent<WeaponInstance>());
            weaponScript.selectedWeapon = upcomingWeaponIndex;
        }

    }

    void CheckPlayerHit()
    {
        // Default radius in case of fail when retrieving the weapon-specified one
        float raycastRadius = 1.5f;

        // Aseta raycastin suunta ja pituus
        Vector2 raycastOrigin = transform.position;
        if (weaponScript.weapons != null && weaponScript.weapons.Count() > 0)
            raycastRadius = weaponScript.weapons[weaponScript.selectedWeapon].pickupRange;
        //LayerMask playerLayer = LayerMask.GetMask("Player"); 

        // Do raycast circle around the object to detect nearby collidings
        RaycastHit2D[] hitColliders = Physics2D.CircleCastAll(raycastOrigin, raycastRadius, Vector2.left);

        // If hits player...
        for (int i = 0; i < hitColliders.Count(); i++)
        {
            if (hitColliders[i].collider != null)
            {
                if (hitColliders[i].collider.name == "Player")
                {
                    if(isInstaPickUpable)
                        PickUp();
                    else
                    {
                        if(Input.GetKeyDown(KeyCode.F)){
                            PickUp();
                        }
                    }
                }
                    
            }
        }


        Debug.DrawRay(raycastOrigin, Vector2.left * raycastRadius, Color.red); // Piirrä raycastin debug-näkyvyyden vuoksi
    }

    void FixedUpdate()
    {
        CheckPlayerHit();
    }
}
