﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tool : MonoBehaviour, ISaveable
{
    public bool bIsActive = false;
    public bool bIsObtained = false;


    public virtual void Start()
    {
        //bIsActive = false;
        //bIsObtained = false;
    }
    public virtual void Awake()
    {
        LoadDataOnSceneEnter();
    }

    public abstract void Update();
    public abstract void UseTool();

    public virtual void Activate()
    {
        EventBroker.CallOnWeaponSwap();
        bIsActive = true;
    }

    public virtual void Deactivate()
    {
        EventBroker.CallOnWeaponSwap();
        bIsActive = false;
    }

    public void ObtainEquipment()
    {
        bIsObtained = true;
        SaveSystem.Save(gameObject.name, "bIsObtained", "Equipment", bIsObtained, SaveSystem.SaveType.EQUIPMENT);
    }

    public void LoadDataOnSceneEnter()
    {
        bIsObtained = SaveSystem.LoadBool(gameObject.name, "bIsObtained", "Equipment");
    }

}
