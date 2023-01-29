using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{
    public GameObject craftingScreenUI;
    public GameObject toolScreenUI, survivalScreenUI, refineScreenUI, constructionScreenUI;
    public List<string>inventoryItemList=new List<string>();
    Button toolsBTN, survivalBTN, refineBTN, constructionBTN;
    Button craftAxeBTN, craftPlankBTN, craftFoundationBTN;
    Text AxeReq1, AxeReq2, PlankReq1,FoundationReq1;
    public bool isOpen;
    private Blueprint axeBLP = new Blueprint("Axe",1,2, "Stone", 3, "Stick", 3);
    private Blueprint PlankBLP = new Blueprint("Plank",2,1, "Log", 1,"",0);
    private Blueprint FoundationBLP = new Blueprint("Foundation",1,1, "Plank", 4,"",0);

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

        survivalBTN = craftingScreenUI.transform.Find("SurvivalButton").GetComponent<Button>();
        survivalBTN.onClick.AddListener(delegate { OpenSurvivalCategory(); });

        refineBTN = craftingScreenUI.transform.Find("RefineButton").GetComponent<Button>();
        refineBTN.onClick.AddListener(delegate { OpenRefineCategory(); });

        constructionBTN = craftingScreenUI.transform.Find("ConstructionButton").GetComponent<Button>();
        constructionBTN.onClick.AddListener(delegate { OpenConstructionCategory(); });

        //Axe

        AxeReq1 = toolScreenUI.transform.Find("Axe").transform.Find("req1").GetComponent<Text>();
        AxeReq2 = toolScreenUI.transform.Find("Axe").transform.Find("req2").GetComponent<Text>();
        craftAxeBTN = toolScreenUI.transform.Find("Axe").transform.Find("ButtonAxe").GetComponent<Button>();
        craftAxeBTN.onClick.AddListener(delegate { CraftAnyItem(axeBLP); });
        //Plank

        PlankReq1 = refineScreenUI.transform.Find("Plank").transform.Find("req1").GetComponent<Text>();
        
        craftPlankBTN = refineScreenUI.transform.Find("Plank").transform.Find("Button").GetComponent<Button>();
        craftPlankBTN.onClick.AddListener(delegate { CraftAnyItem(PlankBLP); });
        //Foundation

        FoundationReq1 = constructionScreenUI.transform.Find("Foundation").transform.Find("req1").GetComponent<Text>();

        craftFoundationBTN = constructionScreenUI.transform.Find("Foundation").transform.Find("Button").GetComponent<Button>();
        craftFoundationBTN.onClick.AddListener(delegate { CraftAnyItem(FoundationBLP); });
    }
    void OpenToolsCategory()
    {
        craftingScreenUI.SetActive(false);
        refineScreenUI.SetActive(false);
        survivalScreenUI.SetActive(false);
        constructionScreenUI.SetActive(false);
        toolScreenUI.SetActive(true);
    }
    void OpenSurvivalCategory()
    {
        craftingScreenUI.SetActive(false);
        toolScreenUI.SetActive(false);
        refineScreenUI.SetActive(false);
        constructionScreenUI.SetActive(false);
        survivalScreenUI.SetActive(true);
    }
    void OpenRefineCategory()
    {
        craftingScreenUI.SetActive(false);
        toolScreenUI.SetActive(false);
        survivalScreenUI.SetActive(false);
        constructionScreenUI.SetActive(false);
        refineScreenUI.SetActive(true);
    }
    void OpenConstructionCategory()
    {
        craftingScreenUI.SetActive(false);
        toolScreenUI.SetActive(false);
        survivalScreenUI.SetActive(false);
        refineScreenUI.SetActive(false);
        constructionScreenUI.SetActive(true);
    }
    void CraftAnyItem(Blueprint blueprintToCraft)
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.craftingSound);
        StartCoroutine(craftedDelayForSound(blueprintToCraft));
              



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
   IEnumerator craftedDelayForSound(Blueprint blueprintToCraft)
   {
        yield return new WaitForSeconds(1f);
        for (var i = 1; i < blueprintToCraft.numberofItemsToProduce; i++)
        {
            InventorySystem.Instance.AddToInventory(blueprintToCraft.itemName);
        }
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.C) && !isOpen && !ConstructionManager.Instance.inConstructionMode)
        {
            craftingScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SelectionManager.Instance.DisableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;
            isOpen = true;

        }
        else if (Input.GetKeyDown(KeyCode.C) && isOpen)
        {
            craftingScreenUI.SetActive(false);
            toolScreenUI.SetActive(false);
            refineScreenUI.SetActive(false);
            survivalScreenUI.SetActive(false);
            constructionScreenUI.SetActive(false);
            if (!InventorySystem.Instance.isOpen)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                SelectionManager.Instance.EnableSelection();
                SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
            }
            isOpen = false;
        }
    }
    public void RefreshNeededItems()
    {
        int stone_count = 0;
        int stick_count = 0;
        int log_count = 0;
        int plank_count = 0;
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
                case "Log":
                    log_count += 1;
                    break;
                case "Plank":
                    plank_count += 1;
                    break;
            }
        }

        //-----AXE----//
    AxeReq1.text = "3 Stone[" + stone_count + "]";
    AxeReq2.text = "3 Stick[" + stick_count + "]";
        if (stone_count >= 3 && stick_count >= 3&&InventorySystem.Instance.CheckSlotAvailable(1))
        {
            craftAxeBTN.gameObject.SetActive(true);

        }
        else
        {
            craftAxeBTN.gameObject.SetActive(false);
        }
        //-----Plank----//
        PlankReq1.text = "1 Log[" + log_count + "]";
       
        if (log_count >= 1 &&InventorySystem.Instance.CheckSlotAvailable(2))
        {
            craftPlankBTN.gameObject.SetActive(true);

        }
        else
        {
            craftPlankBTN.gameObject.SetActive(false);
        } 
        //-----Plank----//
        FoundationReq1.text = "4 Plank[" + plank_count + "]";
       
        if (plank_count >= 4 &&InventorySystem.Instance.CheckSlotAvailable(2))
        {
            craftFoundationBTN.gameObject.SetActive(true);

        }
        else
        {
            craftFoundationBTN.gameObject.SetActive(false);
        }
    }

}
