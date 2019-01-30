using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToOrigin : MonoBehaviour
{
    private Vector3 originPosition;
    
    void Start()
    {
        originPosition = transform.position;
    }

    void OnMouseUp()
    {
        transform.position = originPosition;
    }

    public void ForceBack()
    {
        transform.position = originPosition;

        if(GetComponent<Dragable>())
        {
            GetComponent<Dragable>().StartCoroutine(GetComponent<Dragable>().WaitToCanDrag());
        }
    }
    
}
