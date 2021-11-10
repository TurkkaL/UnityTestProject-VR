using System.Collections.Generic;
using UnityEngine;
using System;


public enum SatelliteEvent
{
    OnDestroyed
}

namespace PathCreation.Examples
{
    // Moves along a path at constant speed.
    // Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
    public class PathFollower : MonoBehaviour
    {
        public bool isPlayer;
        public bool metPlayer { get; private set; } = false;
        public bool Reverse = false;

        public float speed = 3;
        public float stayDistance = 10;
        [Range(0f, .996f)]
        public float startPercentage = 0f;
        public float distanceTravelled { get; private set; }
        public EndOfPathInstruction endOfPathInstruction { get; private set; }

        public GameObject satelliteRoot;
        public int satellites = 0;
        private List<SatelliteEntity> satelliteEntities = new List<SatelliteEntity>();

        [HideInInspector] public PathCreator pathCreator;

        bool Initialized = false;
        void Start()
        {
            pathCreator = LevelWaypointGenerator.Instance.pathCreator;

            if (pathCreator != null)
            {
                // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
                pathCreator.pathUpdated += OnPathChanged;
            }
            // init player
            if (Initialized == false) Initialize(0, true, Reverse, startPercentage);
        }

        void Update()
        {
            if (pathCreator != null)
            {
                distanceTravelled += Reverse ? -(speed * Time.deltaTime) : (speed * Time.deltaTime);
                transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
                transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
            }
        }

        public void Initialize(int NthEntity, bool setPlayer, bool SetReverse = false, float? setStartPercentage = null)
        {
            if (Initialized) return;
            Debug.Log("Initializing " + this.gameObject.name);
            isPlayer = setPlayer;
            if (isPlayer) LevelManager.Instance.followerPlayer = this;
            startPercentage = setStartPercentage == null ? 0f : (float)setStartPercentage;
            Reverse = SetReverse;
            distanceTravelled = startPercentage * LevelWaypointGenerator.Instance.pathLength;

            //TODO: populate satellites from here
            if (NthEntity > 0) satelliteRoot.transform.Rotate(0, 0, NthEntity * 45);
            Initialized = true;
        }
        public void MeetPlayer()
        {
            metPlayer = true;
            foreach (var satellite in satelliteEntities)
            {
                satellite.EngageSatellite();

                // TODO: Engage weapons only on Enemy satellites.
                WeaponAI[] weapons = GetComponentsInChildren<WeaponAI>();

                float increment = 1f / (float)weapons.Length;

                for (int i = 0; i < weapons.Length; i++)
                {
                    //Debug.Log("weapons (" + weapons.Length + ") reload increment: " + increment + " Delay: " + ((i + 1) * increment));
                    weapons[i].Engage((i + 1) * increment);
                }
            }
        }

        public void AddSatellite(SatelliteEntity satellite)
        {
            if (satelliteEntities.Contains(satellite))
            {
                Debug.LogWarning("Trying to add existing satellite: "+satellite.gameObject.name+" ("+satelliteEntities.Count+")");
                return;
            }
            satelliteEntities.Add(satellite);
            satellites++;
        }
        public void RemoveSatellite(SatelliteEntity satellite)
        {
            if (satelliteEntities.Contains(satellite))
            {
                satelliteEntities.Remove(satellite);
                satellites--;
                if (satellites == 0) Debug.Log("NO MORE SATELLITES");
                return;
            }
            Debug.LogWarning("Trying to remove non-existing satellite");
        }

        // If the path changes during the game, update the distance travelled so that the follower's position on the new path
        // is as close as possible to its position on the old path
        void OnPathChanged()
        {
            distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        }

    }
}