using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
public class PlayerHealth : MonoBehaviour
{
    static public PlayerHealth Instance;
    float health = 100;

    static public event Action<float> OnPlayerTakesHit;

    public int playerLayer { get; private set; }

    private void Awake()
    {
        Instance = this;
        //rb = GetComponent<Rigidbody>();
        //if (rb == null) Debug.LogWarning("Couldn't find Rigidbody!");
        playerLayer = LayerMask.NameToLayer("Player");
    }

    public void DamagePlayer(float damage)
    {
        health -= damage;
        if (health <= 0f)
        {
            Debug.Log("PLAYER DEAD!");
            SceneStateManager.Instance.ChangeState(SceneState.Failed);
        }
        // Trigger event
        OnPlayerTakesHit(damage);
    }


    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.layer == ammoLayer)
    //    {
    //        AmmoGuidance ammo = collision.gameObject.GetComponent<AmmoGuidance>();
    //        if (ammo && ammo.owner == Faction.Enemy)
    //        {
    //            Debug.Log("PLAYER HIT!");
    //            health -= ammo.ammoDamage;
    //            if (health <= 0f)
    //            {
    //                Debug.Log("PLAYER DEAD!");
    //            }
    //            GameObject.Destroy(ammo.gameObject);
    //        }
    //        Debug.Log("Ignoring hit by " + collision.gameObject.name);
    //    }
    //    else Debug.Log("HIT "+collision.gameObject.name);
    //}


}
