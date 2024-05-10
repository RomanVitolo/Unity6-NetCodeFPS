using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime.Utilities
{
    public class SpawnPoint : MonoBehaviour
    {
        private static List<SpawnPoint> spawnPoints = new List<SpawnPoint>();

        private void OnEnable()
        {
            spawnPoints.Add(this);
        }

        private void OnDisable()
        {
            spawnPoints.Remove(this);
        }

        public static Vector3 GetRandomSpawnPosition()
        {
            return spawnPoints.Count == 0 ? Vector3.zero : spawnPoints[Random.Range(0, spawnPoints.Count)].transform.position;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, 1);
        }
    }
}