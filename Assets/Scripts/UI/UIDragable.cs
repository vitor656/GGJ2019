using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDragable : MonoBehaviour
{
    
    private GameObject keysOBj;
    private GameObject canvas;

    private float offsetX;
    private float offsetY;

    float originX;
    float originY;

    void Start()
    {
        originX = GetComponent<RectTransform>().anchoredPosition.x;
        originY = GetComponent<RectTransform>().anchoredPosition.y;

        keysOBj = GameObject.Find("Keys");
        canvas = GameObject.Find("Canvas");
    }

    public void BeginDrag()
    {
        offsetX = transform.position.x - Input.mousePosition.x;
        offsetY = transform.position.y - Input.mousePosition.y;
    }

    public void OnDrag()
    {
        transform.position = new Vector3(offsetX + Input.mousePosition.x, offsetY + Input.mousePosition.y);
        GameManager.manager.currentDraggedObject = gameObject;

        transform.SetParent(canvas.transform);
    }

    public void OnEndDrag()
    {
        GameManager.manager.currentDraggedObject = null;

        transform.SetParent(keysOBj.transform, false);
    }

}
