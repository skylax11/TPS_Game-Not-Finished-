using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] public Image health;
    [SerializeField] public Image stamina;
    [SerializeField] public TextMeshProUGUI ammo;
    [SerializeField] public TextMeshProUGUI magazine;

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
        stamina.fillAmount -= Time.deltaTime*0.24f;
    }
    public void increaseStamina()
    {
        stamina.fillAmount += Time.deltaTime * 0.08f;
    }
    public void reflectAmmo()
    {
        if (Inventory.instance.current_item.weapon_data != null)
        {
            ammo.text = Inventory.instance.current_item.weapon_data._currentAmmo.ToString();
            magazine.text = Inventory.instance.current_item.weapon_data._magazineAmmo.ToString();
        }
        else
        {
            ammo.text = "0";
            magazine.text = "0";
        }
    }
}
