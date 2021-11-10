using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using PathCreation.Examples;

public class SatelliteEntity : MonoBehaviour
{
    PathFollower pathFollower;

    void Start()
    {
        pathFollower = transform.parent.parent.GetComponent<PathFollower>(); // Replace this with constructor?
        if (pathFollower == null) Debug.LogError("Unable to find PathFollower -script in the parent object");
        else pathFollower.AddSatellite(this);
    }
    
    public void EngageSatellite()
    {
        
    }

    private void OnDestroy()
    {
        pathFollower.RemoveSatellite(this);
    }

    public SatelliteEntity(PathFollower parentPathFollower)
    {
        pathFollower = parentPathFollower;
    }

    void DestroySatellite()
    {
        GameObject.Destroy(this.gameObject);
    }
}
