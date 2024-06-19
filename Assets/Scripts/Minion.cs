using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Zenject;

public class Minion : MonoBehaviour
{
    [SerializeField] private GameObject target;
    private NavMeshAgent navMeshAgent;
    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.destination = target.transform.position;
        navMeshAgent.isStopped = false;
    }
    //private GameObject FindTarget()
    //{
    //    GameObject closestEnemy = null;
    //    float bestDistance = 1000;

    //    //find enemy
    //    if (teamID == TEAMID_PLAYER)
    //    {

    //        for (int x = 0; x < Map.hexMapSizeX; x++)
    //        {
    //            for (int z = 0; z < Map.hexMapSizeZ / 2; z++)
    //            {
    //                if (aIopponent.gridChampionsArray[x, z] != null)
    //                {
    //                    ChampionController championController = aIopponent.gridChampionsArray[x, z].GetComponent<ChampionController>();

    //                    if (championController.isDead == false)
    //                    {
    //                        //calculate distance
    //                        float distance = Vector3.Distance(this.transform.position, aIopponent.gridChampionsArray[x, z].transform.position);

    //                        //if new this champion is closer then best distance
    //                        if (distance < bestDistance)
    //                        {
    //                            bestDistance = distance;
    //                            closestEnemy = aIopponent.gridChampionsArray[x, z];
    //                        }
    //                    }


    //                }
    //            }
    //        }
    //    }
    //    else if (teamID == TEAMID_AI)
    //    {

    //        for (int x = 0; x < Map.hexMapSizeX; x++)
    //        {
    //            for (int z = 0; z < Map.hexMapSizeZ / 2; z++)
    //            {
    //                if (gamePlayController.gridChampionsArray[x, z] != null)
    //                {
    //                    ChampionController championController = gamePlayController.gridChampionsArray[x, z].GetComponent<ChampionController>();

    //                    if (championController.isDead == false)
    //                    {
    //                        //calculate distance
    //                        float distance = Vector3.Distance(this.transform.position, gamePlayController.gridChampionsArray[x, z].transform.position);

    //                        //if new this champion is closer then best distance
    //                        if (distance < bestDistance)
    //                        {
    //                            bestDistance = distance;
    //                            closestEnemy = gamePlayController.gridChampionsArray[x, z];
    //                        }
    //                    }
    //                }
    //            }
    //        }

    //    }


    //    return closestEnemy;
    //}

    [Serializable]
    public class Settings
    {
        public string minionName;
        public int mana;
        public int health;
        public int damagePerSecond;
        public int attackDamage;
        public float attackSpeed;
        public int attackRange;
        public int attackAreaOfEffect;
        public float skillCastTime;
    }
}

