using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class PlayerInstance : MonoBehaviour
{
    static public PlayerInstance Instance { get; private set; }
    private Vector3 _targetOffset;
    
    public BNGPlayerController playerController { get; private set; }

    void Awake()
    {
        Instance = this;
        playerController = GetComponentInChildren<BNGPlayerController>();
        if (playerController == null) Debug.LogWarning("Couldn't find BNGPlayerController");
    }

    public Vector3 GetTargetOffset()
    {
        return playerController.transform.parent.forward
            * LevelManager.Instance.followerPlayer.speed;
    }
    
}
