using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public int id;
    public bool available;
    private SpriteRenderer sprite;
    private UIManager ui;

    void Awake(){
        sprite = GetComponent<SpriteRenderer>();
    }  

    void Start()
    {
        //mostra as chaves disponíveis
    }

    // Update is called once per frame
    void Update()
    {
        //verifica quantos quartos estão disponíveis
    //if() //chaveiro tem chave
   //     Instantiate(Key key, Vector3 position, Quaternion rotation);
   // else
        //quartos ocupados, oferecer outra opção?
    }
}
