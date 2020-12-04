﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public WeaponType _pickUpWeapon;
    int weaponNum;
    bool isUsed = false;

    void Awake()
    {
        LoadDataOnSceneEnter();
        SaveSystem.SaveEvent += SaveDataOnSceneChange;

        switch (_pickUpWeapon)
        {
            case WeaponType.BaseWeapon:
                weaponNum = 0;
                break;
            case WeaponType.GrenadeWeapon:
                weaponNum = 1;
                break;
            case WeaponType.CreatureWeapon:
                weaponNum = 2;
                break;
        }

        if (isUsed) GetComponent<MeshRenderer>().enabled = false;
        else GetComponent<MeshRenderer>().enabled = true;
    }

    void OnDisable()
    {
        SaveSystem.SaveEvent -= SaveDataOnSceneChange;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //GameObject g = GameObject.FindGameObjectWithTag("WeaponBelt");
            //Belt b = g.GetComponent<Belt>();
            EventBroker.CallOnPickupWeapon(weaponNum);
            //b.EquipToolAtIndex(weaponNum);
            isUsed = true;
            GetComponent<MeshRenderer>().enabled = false;
            //Destroy(gameObject);
        }
    }

    public void SaveDataOnSceneChange()
    {
        SaveSystem.Save(gameObject.name, "isEnabled", gameObject.scene.name, isUsed);
        //Debug.Log(isUsed);
    }

    public void LoadDataOnSceneEnter()
    {
        isUsed = SaveSystem.LoadBool(gameObject.name, "isEnabled", gameObject.scene.name);
    }
}
