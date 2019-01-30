using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIMonitor : MonoBehaviour
{
    void Update()
    {
        if(!EventSystem.current.IsPointerOverGameObject())
        {
            FindObjectOfType<UIManager>().HideMonitor();
        }
    }
}
