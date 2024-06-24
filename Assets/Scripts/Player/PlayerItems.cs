using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItems : MonoBehaviour
{
    public List<bool> ownedItemsByIndex = new List<bool>();
    public int selectedItemIndex = -1; // Starts without any ON
    public GameObject itemSelected = null;
    Inventory inventory;
    PlayerMotor playerMotor;
    RawImage UIcurItem;
    [SerializeField] GameObject curPlayerItem;
    [SerializeField] Sprite none;
    void Start()
    {
        UIcurItem = GameObject.Find("curItem").GetComponent<RawImage>();
        playerMotor = FindAnyObjectByType<PlayerMotor>();
        inventory = FindAnyObjectByType<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleOwnedItems();
        HandleItemSwitch();
        UpdateItemPos();
    }

    void HandleOwnedItems()
    {
        for (int i = 0; i > inventory.fullItems.Count; i++)
        {
            ownedItemsByIndex[i] = true;
        }
    }

    void SetItemActive(int index)
    {
        if (index > -1)
        {
            // Check if item is owned by player
            if (ownedItemsByIndex[index] == true)
            {
                curPlayerItem.GetComponent<SpriteRenderer>().sprite = inventory.fullItems[index];
                curPlayerItem.GetComponent<SpriteRenderer>().sortingOrder = 3;
                curPlayerItem.GetComponent<Transform>().localScale = new Vector3(0.3f, 0.3f, 0.3f);
                itemSelected = curPlayerItem;
                UIcurItem.texture = inventory.fullItems[index].texture;
            }
            else
            {
                Debug.Log("User does not own the item. Item index: " + index.ToString());
            }
        }
        else{
            UIcurItem.texture = none.texture;
            if(itemSelected != null)
                itemSelected.GetComponent<SpriteRenderer>().sprite = null;

        }

    }

    void HandleItemSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (selectedItemIndex != -1)
            {
                selectedItemIndex -= 1; // None
            }
            else
            {
                selectedItemIndex = inventory.fullItems.Count - 1;
            }
            SetItemActive(selectedItemIndex);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            if (selectedItemIndex != inventory.fullItems.Count - 1)
            {
                selectedItemIndex += 1;
            }
            else
            {
                selectedItemIndex = -1;
            }
            SetItemActive(selectedItemIndex);
        }
    }


    void UpdateItemPos()
    {
        // TODO: Get item type
        if (itemSelected != null)
            itemSelected.transform.position = playerMotor._playerObject.transform.position;
    }
}
