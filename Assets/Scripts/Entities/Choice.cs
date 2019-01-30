using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Choice : MonoBehaviour
{
    public void MouseClick()
    {
        GameManager.manager.selectedChoice = name;
        GameObject.Find("ResponseChoiceBox").gameObject.SetActive(false);
    }
}
