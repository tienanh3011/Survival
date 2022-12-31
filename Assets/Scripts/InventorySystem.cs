using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{

    public static InventorySystem Instance { get; set; }

    public GameObject inventoryScreenUI;
    public List<GameObject> slotList=new List<GameObject>();
    public List<string> itemList =new List<string>();
    private GameObject itemToAdd;
    private GameObject whatslotToEquip;
    public bool isOpen;
    


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    void Start()
    {
        isOpen = false;
        
        PopulateSlotList();
    }
    private void PopulateSlotList()
    {
        foreach(Transform child in inventoryScreenUI.transform)
        {
            if(child.CompareTag("Slot"))
            {
                slotList.Add(child.gameObject);
            }
        }
    }


    void Update()
    {

        if (Input.GetKeyDown(KeyCode.I) && !isOpen)
        {

            Debug.Log("i is pressed");
            inventoryScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            isOpen = true;

        }
        else if (Input.GetKeyDown(KeyCode.I) && isOpen)
        {
            inventoryScreenUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            isOpen = false;
        }
    }
    public void AddToInventory(string itemName)
    {
            whatslotToEquip = FindNextEmptySlot();
            itemToAdd=Instantiate(Resources.Load<GameObject>(itemName),whatslotToEquip.transform.position,whatslotToEquip.transform.rotation);
            itemToAdd.transform.SetParent(whatslotToEquip.transform);
            itemList.Add(itemName);
        
    }
    private GameObject FindNextEmptySlot()
    {
        foreach(GameObject slot in slotList)
        {
            if(slot.transform.childCount==0)
            {
                return slot;
            }
        }
        return new GameObject();
    }
    public bool CheckifFull()
    {
        int couter = 0;
        foreach(GameObject slot in slotList)
        {
            if(slot.transform.childCount>0)
            {
                couter += 1;
            }
        }
        if(couter==21)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}