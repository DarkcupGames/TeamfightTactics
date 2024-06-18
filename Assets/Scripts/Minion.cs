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

