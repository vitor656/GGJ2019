using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragable : MonoBehaviour
{
    private Vector3 screenPoint;
    private Vector3 offset;

    public bool canDrag = true;

    void OnMouseDown()
    {
        if(canDrag)
        {
            screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
            offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(
                new Vector3(
                    Input.mousePosition.x, 
                    Input.mousePosition.y, 
                    screenPoint.z
                )
            );

            if(tag == "Key")
            {
                FindObjectOfType<AudioManager>().Play(Sounds.KEYS);
            }
        }
    }
    
    void OnMouseDrag()
    {
        if(canDrag)
        {
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
            transform.position = curPosition;

            GameManager.manager.currentDraggedObject = gameObject;
        }

    }

    void OnMouseUp()
    {
        GameManager.manager.currentDraggedObject = null;
    }

    public IEnumerator WaitToCanDrag()
    {
        canDrag = false;
        yield return new WaitForSeconds(1);
        canDrag = true;
    }
}
