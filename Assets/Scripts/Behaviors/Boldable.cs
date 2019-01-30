using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Boldable : MonoBehaviour
{

    public void PointerEnter()
    {
        GetComponent<TextMeshProUGUI>().fontSize = 24;
    }

    public void PointerExit()
    {
        GetComponent<TextMeshProUGUI>().fontSize = 22;
    }
}
