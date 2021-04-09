﻿using System;
using System.Collections;
using System.Data;
using System.Runtime.CompilerServices;
using UnityEngine;


//VR - This class is a damage system for any target/enemy
public class Health : MonoBehaviour, ISaveable
{
    [Header("Health Settings")]
    [SerializeField]
    private float m_HP = 50.0f;
    private float m_MaxHealth = 100f;

    [Space]
    [Header("Debug Variables")]
    [SerializeField]
    private bool m_IsLoadingShield = false;

    public event Action OnTakeDamage;
    public event Action<float> OnHeal;
    public event Action OnDeath;

    public bool bCanBeDamaged = true;
    public bool isDead = false;
    bool isMarkerCreated;

    public HealthBarUI healthBar;

    //Compass Markers
    public Compass m_compass;
    public CompassMarkers m_marker;

    EnemyHitEffects hitEffects;


    void Start()
    {
        healthBar = FindObjectOfType<HealthBarUI>();

        if (healthBar != null)
            healthBar.SetMaxHealth(m_MaxHealth);

        isMarkerCreated = false;
    }

    void PlayerSpawned(GameObject playerReference)
    {
        try
        {
            m_compass = FindObjectOfType<Compass>();
            m_marker = GetComponent<CompassMarkers>();
            if (gameObject.tag != "Player" && m_marker != null && isMarkerCreated == false)
            {
                m_compass.AddMarker(m_marker);
                isMarkerCreated = true;
            }
        }
        catch
        {

        }

    }

    void Awake()
    {
        LoadDataOnSceneEnter();
        EventBroker.OnPlayerSpawned += PlayerSpawned;
        hitEffects = GetComponentInChildren<EnemyHitEffects>();
        //if (isDead) GetComponent<MeshRenderer>().enabled = false;
        //else GetComponent<MeshRenderer>().enabled = true;
    }

    public void TakeDamage(float damage)
    {
        if (bCanBeDamaged)
        {
            m_HP -= damage;
            
            //Clamp values -LCC
            m_HP = Mathf.Clamp(m_HP, 0, m_MaxHealth);
            
            if (healthBar != null && gameObject.tag == "Player")
                healthBar.LoseHealth(m_HP, damage, m_MaxHealth);
            if (hitEffects != null)
            {
                hitEffects.Hit();
            }

            if (m_HP <= 0.0f)
            {
                if (hitEffects != null)
                {
                    hitEffects.Hit();
                }
                if (gameObject.tag == "Player")
                {
                    PlayerDeath();
                }
                else
                {
                    Die();
                }
            }


            CallOnTakeDamage();
        }
    }

    public float GetMaxHealth()
    {
        return m_MaxHealth;
    }

    void Die()
    {
        isDead = true;

        if (hitEffects != null)
        {
            hitEffects.Death();
        }
        if (gameObject.tag != "Roamer")
        {
            if (m_marker != null)
            {
                m_compass.RemoveMarker(m_marker);
            }

            //Temporary Spawning Stuff - Anthony
            Spawner spawner = GetComponent<Spawner>();
            if (spawner != null)
            {
                spawner.Spawn();
            }


            //Disabled Deactivation on Death to make Death Event more malleable - LCC
        
            //Destroy(gameObject);
            //if(gameObject.tag != "Player")
            //gameObject.SetActive(false);
            //for (int i = 0; i < transform.childCount; ++i)
            //{
            //    transform.GetChild(i).gameObject.SetActive(false);
            //}
            transform.DetachChildren();
            CallOnDeath();
        }
    }

    void PlayerDeath()
    {
        if (gameObject.tag == "Player")
        {
            EventBroker.CallOnPlayerDeath();

        }

    }

    public void CallOnTakeDamage()
    {
        OnTakeDamage?.Invoke();
    }

    public void CallOnDeath()
    {
        OnDeath?.Invoke();
    }

    public void CallOnHeal(float healthToHeal)
    {
        OnHeal?.Invoke(healthToHeal);
    }

    public bool IsAtFullHealth()
    {
        if (m_HP < m_MaxHealth)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void Heal(float healthToHeal)
    {
        if (m_HP < m_MaxHealth)
        {
            CallOnHeal(healthToHeal);
            m_HP += healthToHeal;

            if (healthBar != null)
                healthBar.GainHealth(m_HP, healthToHeal, m_MaxHealth);

            m_HP = Mathf.Clamp(m_HP, 0, m_MaxHealth);
            Debug.Log("Healed" + healthToHeal + "amount");
        }
    }

    public void LoadDataOnSceneEnter()
    {
        //isDead = SaveSystem.LoadBool(gameObject.name, "isDead", gameObject.scene.name);

        //if (SaveSystem.LoadFloat(gameObject.name, "posX", gameObject.scene.name) != 0)
        //{
        //    transform.position = new Vector3(SaveSystem.LoadFloat(gameObject.name, "posX", gameObject.scene.name), SaveSystem.LoadFloat(gameObject.name, "posY", gameObject.scene.name), SaveSystem.LoadFloat(gameObject.name, "posZ", gameObject.scene.name));
        //}

        //transform.rotation = new Quaternion(
        //    SaveSystem.LoadFloat(gameObject.name, "rotX", gameObject.scene.name),
        //    SaveSystem.LoadFloat(gameObject.name, "rotY", gameObject.scene.name),
        //    SaveSystem.LoadFloat(gameObject.name, "rotZ", gameObject.scene.name),
        //    SaveSystem.LoadFloat(gameObject.name, "rotW", gameObject.scene.name));
    }

}
