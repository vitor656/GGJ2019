using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{

    public List<Item> itensOnTable;

    void Start()
    {   
        itensOnTable = new List<Item>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.GetComponent<Item>())
        {
            itensOnTable.Add(collider.GetComponent<Item>());
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.GetComponent<Item>())
        {
            if(CheckItemOnList(collider.GetComponent<Item>()))
            {
                itensOnTable.Remove(collider.GetComponent<Item>());
            }
        }
        
    }

    bool CheckItemOnList(Item itemToCheck)
    {
        return itensOnTable.Contains(itemToCheck);
    }   

    public bool IsItemOnTable(Nacionalidade nac)
    {
        foreach(Item item in itensOnTable)
        {
            if(item.nacionalidade == nac)
            {
                return true;
            }
        }

        return false;
    }
}
