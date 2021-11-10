using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WeaponAI : MonoBehaviour
{

    public GameObject ammoPrefab;
    public bool Engaged { get; private set; } = false;
    public float rateOfFire { get; private set; } = 2f;
    [SerializeField] private float recharge;


    void Update()
    {
        Tick(Time.deltaTime);
    }
    public void Engage(float chargeDelay)
    {
        recharge = chargeDelay * rateOfFire;
        Engaged = true;
    }

    void Tick(float deltaTime)
    {
        if (Engaged)
        {
            if (recharge > 0f)
            {
                recharge -= Time.deltaTime;
            }
            else
            {
                Attack();
                recharge = rateOfFire;
            }
        }
    }

    void Attack()
    {
        AmmoGuidance ammo = Instantiate(ammoPrefab, transform.position, new Quaternion(0, 0, 0, 0) ).GetComponent<AmmoGuidance>();
        if (ammo) ammo.Shoot();
    }
}
