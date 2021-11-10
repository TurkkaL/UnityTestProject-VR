using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Faction
{
    Player,
    Enemy
}

public class AmmoGuidance : MonoBehaviour
{
    public Faction owner = Faction.Enemy;
    public float ammoDamage = 25f;
    public float ammoSpeed = 10f;
    public float lifeTime = 4f;

    Vector3 targetDirection = Vector3.zero;

    private void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0f)
        {
            GameObject.Destroy(this.gameObject);
        }
    }

    public void Shoot()
    {
        if (owner == Faction.Enemy)
        {
            if (ammoSpeed == 0f) return;

            targetDirection = 
                PlayerInstance.Instance.playerController.CenterEyeAnchor.position
                + (PlayerInstance.Instance.GetTargetOffset() / ammoSpeed *4f)
                - transform.position;

            this.GetComponentInChildren<Rigidbody>().AddForce(targetDirection, ForceMode.VelocityChange);
        }
        else Debug.LogWarning("TODO: only enemies use AmmoGuidance so far!");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 0) return;
        if (collision.gameObject.layer == PlayerHealth.Instance.playerLayer)
        {
            Debug.Log("Player hit! [" + collision.gameObject.name + "]");
            PlayerHealth.Instance.DamagePlayer(ammoDamage);
            GameObject.Destroy(this.gameObject);
        }
        else Debug.Log("HIT " + collision.gameObject.name);
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawSphere(PlayerInstance.Instance./*playerController.CenterEyeAnchor*/transform.position /*- transform.position*/
    //            + (PlayerInstance.Instance.GetTargetOffset() /ammoSpeed * 4f)
    //        , 0.25f);
    //}
}
