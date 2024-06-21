using System;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class Minion : MonoBehaviour
{
    [SerializeField] private TeamID teamID;
    [SerializeField] private MinionState minionState;
    [SerializeField] private GridType gridType;
    [SerializeField] private GameObject target;
    [SerializeField] private Settings settings;

    private NavMeshAgent navMeshAgent;
    [Inject] private AIOpponent aIOpponent;
    [Inject] private Gameplay gameplay;
    private Map map;


    private Vector3 gridTargetPosition;
    public int gridPositionX = 0;
    public int gridPositionZ = 0;

    [Inject]
    public void Construct(Settings settings)
    {
        this.settings = settings;
        navMeshAgent = gameObject.AddComponent<NavMeshAgent>();
    }

    private void Start()
    {
        navMeshAgent.isStopped = true;

    }
    private void Update()
    {
        if (gameplay.GameState == GameState.GAME_STATE_COMBAT)
        {
            if (target == null)
            {
                TryAttackNewTarget();
            }
            if (target != null)
            {
                transform.LookAt(target.transform, Vector3.up);
                if (minionState != MinionState.MINION_STATE_ATTACKING)
                {
                    float distace = Vector3.Distance(transform.position, target.transform.position);
                    if (distace <= settings.attackRange)
                    {
                        Debug.Log("Attack");
                        minionState = MinionState.MINION_STATE_ATTACKING;
                        navMeshAgent.isStopped = true;
                    }
                    else
                    {
                        navMeshAgent.destination = target.transform.position;
                    }
                }
            }
        }
    }

    //public void SetGridPosition(int _gridType, int _gridPositionX, int _gridPositionZ)
    //{
    //    gridType = _gridType;
    //    gridPositionX = _gridPositionX;
    //    gridPositionZ = _gridPositionZ;


    //    //set new target when chaning grid position
    //    gridTargetPosition = GetWorldPosition();
    //}
    //public Vector3 GetWorldPosition()
    //{
    //    //get world position
    //    Vector3 worldPosition = Vector3.zero;

    //    if (gridType == Map.GRIDTYPE_OWN_INVENTORY)
    //    {
    //        worldPosition = map.ownInventoryGridPositions[gridPositionX];
    //    }
    //    else if (gridType == Map.GRIDTYPE_HEXA_MAP)
    //    {
    //        worldPosition = map.mapGridPositions[gridPositionX, gridPositionZ];

    //    }

    //    return worldPosition;
    //}
    //public void OnCombatStart()
    //{
    //    //IsDragged = false;

    //    this.transform.position = gridTargetPosition;


    //    //in combat grid
    //    if (gridType == Map.GRIDTYPE_HEXA_MAP)
    //    {
    //        //isInCombat = true;
    //        gameState = GameState.GAME_STATE_COMBAT;

    //        navMeshAgent.enabled = true;

    //        TryAttackNewTarget();

    //    }

    //}
    private void TryAttackNewTarget()
    {
        //find closest enemy
        target = FindTarget();

        //if target found
        if (target != null)
        {
            //set pathfinder target
            navMeshAgent.destination = target.transform.position;
            minionState = MinionState.MINION_STATE_MOVING;

            navMeshAgent.isStopped = false;
        }
    }
    private GameObject FindTarget()
    {
        GameObject closestEnemy = null;
        float bestDistance = 1000;

        if (teamID == TeamID.TEAMID_PLAYER)
        {
            FindEnemy(ref closestEnemy, ref bestDistance);
        }
        else if (teamID == TeamID.TEAMID_AI)
        {
            FindPlayer(ref closestEnemy, ref bestDistance);
        }

        return closestEnemy;
    }

    private void FindPlayer(ref GameObject closestEnemy, ref float bestDistance)
    {
        for (int x = 0; x < Map.hexMapSizeX; x++)
        {
            for (int z = 0; z < Map.hexMapSizeZ / 2; z++)
            {
                if (gameplay.gridMinionsArray[x, z] != null)
                {
                    //calculate distance
                    Vector3 playerPos = gameplay.gridMinionsArray[x, z].transform.position;
                    float distance = Vector3.Distance(transform.position, playerPos);

                    //if new this champion is closer then best distance
                    if (distance < bestDistance)
                    {
                        bestDistance = distance;
                        closestEnemy = gameplay.gridMinionsArray[x, z];
                    }
                }

            }
        }
    }

    private void FindEnemy(ref GameObject closestEnemy, ref float bestDistance)
    {
        for (int x = 0; x < Map.hexMapSizeX; x++)
        {
            for (int z = 0; z < Map.hexMapSizeZ / 2; z++)
            {
                if (aIOpponent.gridMinionsArray[x, z] != null)
                {
                    //calculate distance
                    Vector3 enemyPos = aIOpponent.gridMinionsArray[x, z].transform.position;
                    float distance = Vector3.Distance(transform.position, enemyPos);

                    //if new this champion is closer then best distance
                    if (distance < bestDistance)
                    {
                        bestDistance = distance;
                        closestEnemy = aIOpponent.gridMinionsArray[x, z];
                    }
                }
            }
        }
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
    public class Factory : PlaceholderFactory<Settings, Minion>
    {
    }
}
