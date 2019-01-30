using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Examinable : MonoBehaviour
{

    public GameObject imageDetails;
    private GameObject imageReference;

    private void OnMouseOver()
    {
        if(imageReference == null){
            imageReference = Instantiate(
                imageDetails, 
                new Vector3(transform.position.x + 1, transform.position.y + 1, transform.position.z), 
                Quaternion.identity
            );

            imageReference.transform.parent = gameObject.transform;
        }

        GameManager.manager.firstCardChecked = true;
    
    }

    private void OnMouseExit()
    {
        if(imageReference != null)
            Destroy(imageReference);
    }
}
