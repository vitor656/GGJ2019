using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monitor : MonoBehaviour
{
    
    private UIManager ui;

    void Start()
    {
        ui = FindObjectOfType<UIManager>();
    }

    private void OnMouseDown()
    {
        ui.ShowMonitor();

        FindObjectOfType<AudioManager>().Play(Sounds.COMPUTER);
        ui.StartCoroutine(ui.TecTec());
    }
}
