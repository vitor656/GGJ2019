using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextGuestBtn : MonoBehaviour
{
    public void CallGuest()
    {
        GameManager.manager.SwitchState(GameState.GUEST_ARRIVED);
    }
}
