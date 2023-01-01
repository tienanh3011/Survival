using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public bool playerInRange;
    public string ItemName;
    public string GetItemName()
    {
        return ItemName;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0)&&playerInRange&&SelectionManager.Instance.onTarget&&SelectionManager.Instance.selectedObject==gameObject)
        {
            if (!InventorySystem.Instance.CheckifFull())
            {
                InventorySystem.Instance.AddToInventory(ItemName);
                Debug.Log("item added to inventory");
                Destroy(gameObject);
            }
            else 
            {
                Debug.Log("inventory is full");
            }

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
