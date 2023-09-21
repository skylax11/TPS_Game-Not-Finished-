using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public Image Health;
    public Image Stamina;
    public TextMeshProUGUI Ammo;
    public TextMeshProUGUI Magazine;

    // Singleton

    public static UI_Manager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            return;
        }
    }

    public void takeDamage(int damage)
    {
        // Will be handled
    }
    public void decreaseStamina()
    {
        Stamina.fillAmount -= Time.deltaTime*0.24f;
    }
    public void increaseStamina()
    {
        Stamina.fillAmount += Time.deltaTime * 0.08f;
    }
    public void reflectAmmo()
    {
        if (Inventory.instance.current_item.weapon_data != null)
        {
            Ammo.text = Inventory.instance.current_item.weapon_data._currentAmmo.ToString();
            Magazine.text = Inventory.instance.current_item.weapon_data._magazineAmmo.ToString();
        }
        else
        {
            Ammo.text = "0";
            Magazine.text = "0";
        }
    }
}
