using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    //references
    public Functions staminaAmt;
    public ManagerStamina manager;

    //UI import
    public Image staminaBar;

    // Update is called once per frame
    void Update()
    {
        staminaBar.fillAmount = staminaAmt.currStamina / manager.maxStamina;
    }
}
