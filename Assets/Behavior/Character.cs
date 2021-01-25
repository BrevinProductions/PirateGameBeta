using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
  enum WeaponState : byte
  {
    WeaponsStowed,
    SwordDrawn,
    GunDrawn,
  }

    // Start is called before the first frame update
    Rigidbody rb;
    public GameObject boat;
    public GameObject sword;
    public GameObject gun;
    public GameObject activeWeapon;

    float weaponTimer;

    WeaponState ws = WeaponState.SwordDrawn;
    WeaponState wsPrior = WeaponState.SwordDrawn; //prior WeaponState

    private static WeaponState SwordDrawn { get; set; }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(weaponTimer >= 0)
        {
            weaponTimer -= Time.deltaTime;
        }
        if (weaponTimer < 0)
            weaponTimer = 0;

        if (!(ws == wsPrior))
        {
          switch(ws)
          {
            case WeaponState.WeaponsStowed:
              activeWeapon.SetActive(false);
              break;
            case WeaponState.SwordDrawn:
              activeWeapon = sword;
              activeWeapon.SetActive(true);
              break;
            case WeaponState.GunDrawn:
              activeWeapon = gun;
              activeWeapon.SetActive(true);
              break;
            default:
              break;
          }
        }

        wsPrior = ws;

        if(Input.GetKeyDown(KeyCode.Q) && weaponTimer <= 0f)
        {
          weaponTimer = 0.2f;

          if(ws == WeaponState.SwordDrawn)
          {
            ws = WeaponState.WeaponsStowed;
          }
          else if(ws == WeaponState.WeaponsStowed)
          {
            ws = WeaponState.SwordDrawn;
          }
        }

    }
}
