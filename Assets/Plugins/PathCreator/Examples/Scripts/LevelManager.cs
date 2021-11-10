using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using PathCreation.Examples;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    //public GameObject followerPrefab;
    public List<FollowerEntity> followerEntities = new List<FollowerEntity>();

    [HideInInspector] public PathFollower followerPlayer;

    void Awake()
    {
        Instance = this;
    }


    bool AllFollowersInitialized = false;
    bool foundUninitializedEntities = false;
    private void Update()
    {
        // Instantiate followerEntities
        if (AllFollowersInitialized == false)
        {
            foundUninitializedEntities = false;
            for (int i = 0; i<followerEntities.Count; i++)
            {
                if (followerEntities[i].instantiated == false)
                {
                    foundUninitializedEntities = true;
                    followerEntities[i].spawnTimer -= Time.deltaTime;
                    if (followerEntities[i].spawnTimer <= 0f)
                    {
                        InstantiateFollower(followerEntities[i], i);
                    }
                    foundUninitializedEntities = true;
                }
            }
            if (foundUninitializedEntities == false) AllFollowersInitialized = true;
        }
        // Check for entity proximities
        foreach (var entity in followerEntities)
        {
            if (entity.instantiated && entity.pathFollower.metPlayer == false)
            {
                // Start moving aligned with the player
                if (entity.pathFollower.distanceTravelled - followerPlayer.distanceTravelled <= entity.pathFollower.stayDistance)
                {
                    entity.pathFollower.MeetPlayer();
                    entity.pathFollower.Reverse = followerPlayer.Reverse;
                    entity.pathFollower.speed = followerPlayer.speed;
                }
            }
        }
    }

    void InstantiateFollower(FollowerEntity followerEntity, int NthEntity = 0)
    {
        followerEntity.instantiated = true;
        GameObject follower = Instantiate(followerEntity.followerEntityPrefab, this.transform);
        followerEntity.pathFollower = follower.GetComponent<PathFollower>();
        followerEntity.pathFollower.Initialize(
            NthEntity,
            false,
            followerEntity.Reverse,
            followerEntity.startPercentage);
    }

    [Serializable]
    public class FollowerEntity
    {
        [HideInInspector] public bool instantiated = false;
        public float spawnTimer;
        [Range(0f, 0.996f)]
        public float startPercentage;
        public float speed;
        public bool Reverse = true;
        public GameObject followerEntityPrefab;
        public int waveNumbers;
        [HideInInspector] public PathFollower pathFollower;
    }
}
