using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Examinable))]
[RequireComponent(typeof(Dragable))]
public class Doc : MonoBehaviour
{   

    private Transform table;

    void Start()
    {
        table = GameObject.Find("Table").transform;
    }

    void Update()
    {
        if(GameManager.manager.isCurrentState(GameState.GUEST_SHOWS_DOC))
        {
            Vector2 desPos = table.position;
            desPos.x -= 3;

            transform.position = new Vector3(
                Mathf.Lerp(transform.position.x, desPos.x, 0.2f),
                Mathf.Lerp(transform.position.y, desPos.y, 0.2f),
                transform.position.z
            );
            
            if(Vector3.Distance(desPos, transform.position) < .2f)
            {
                GameManager.manager.SwitchState(GameState.GUEST_INTERACTION);
            }
        }
    }
}
