using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{
    public GameObject craftingScreenUI;
    public GameObject toolScreenUI;
    public List<string>inventoryItemList=new List<string>();
    Button toolsBTN;
    Button craftAxeBTN;
    Text AxeReq1, AxeReq2;
    public bool isOpen;
    public Blueprint AxeBLP = new Blueprint("Axe", 2, "Stone", 3, "Stick", 3);

    public static CraftingSystem Instance { get;  set; }
    private void Awake()
    {
        if(Instance!=null&&Instance!=this)
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
        toolsBTN=craftingScreenUI.transform.Find("ToolsButton").GetComponent<Button>();
        toolsBTN.onClick.AddListener(delegate { OpenToolsCategory(); });
        AxeReq1 = toolScreenUI.transform.Find("Axe").transform.Find("req1").GetComponent<Text>();
        AxeReq2 = toolScreenUI.transform.Find("Axe").transform.Find("req2").GetComponent<Text>();
        craftAxeBTN = toolScreenUI.transform.Find("Axe").transform.Find("ButtonAxe").GetComponent<Button>();
        craftAxeBTN.onClick.AddListener(delegate { CraftAnyItem(AxeBLP); });
    }
    void OpenToolsCategory()
    {
        craftingScreenUI.SetActive(false);
        toolScreenUI.SetActive(true);
    }
    void CraftAnyItem(Blueprint blueprintToCraft)
    {
        InventorySystem.Instance.AddToInventory(blueprintToCraft.itemName);
        if (blueprintToCraft.numofRequirements == 1)
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1amount);

        }
        else if (blueprintToCraft.numofRequirements == 2)
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1amount);
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req2, blueprintToCraft.Req2amount);
        }
        StartCoroutine(calculate());
        
    }
    public IEnumerator calculate()
    {
        yield return 0;
        
        InventorySystem.Instance.ReCalculeList();
        RefreshNeededItems();
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.T) && !isOpen)
        {
            craftingScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            isOpen = true;

        }
        else if (Input.GetKeyDown(KeyCode.T) && isOpen)
        {
            craftingScreenUI.SetActive(false);
            toolScreenUI.SetActive(false);
            if (!InventorySystem.Instance.isOpen)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            isOpen = false;
        }
    }
    public void RefreshNeededItems()
    {
        int stone_count = 0;
        int stick_count = 0;
        inventoryItemList = InventorySystem.Instance.itemList;
        foreach (string itemName in inventoryItemList)
        {
            switch (itemName)
            {
                case "Stone":
                    stone_count += 1;
                    break;
                case "Stick":
                    stick_count += 1;
                    break;
            }
        }

        //-----AXE----//
    AxeReq1.text = "3 Stone[" + stone_count + "]";
    AxeReq2.text = "3 Stick[" + stick_count + "]";
        if (stone_count >= 3 && stick_count >= 3)
        {
            craftAxeBTN.gameObject.SetActive(true);

        }
        else
        {
            craftAxeBTN.gameObject.SetActive(false);
        }
    }

}
