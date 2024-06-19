using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Zenject;

public class Minion : MonoBehaviour
{
    public const byte TEAMID_PLAYER = 0;
    public const byte TEAMID_AI = 1;

    [SerializeField] private GameObject target;
    [SerializeField] private Settings settings;

    private NavMeshAgent navMeshAgent;
    private AIOponent aIOpponent;
    private Gameplay gameplay;
    private Map map;

    private GameState gameState;
    public MinionState minionState;

    private Vector3 gridTargetPosition;
    private int gridType = 0;
    public int gridPositionX = 0;
    public int gridPositionZ = 0;
    public int teamID = 0;


    private void Start()
    {
        gameState = GameState.GAME_STATE_WAITING;
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.destination = target.transform.position;
        navMeshAgent.isStopped = false;

    }

    public void SetGridPosition(int _gridType, int _gridPositionX, int _gridPositionZ)
    {
        gridType = _gridType;
        gridPositionX = _gridPositionX;
        gridPositionZ = _gridPositionZ;


        //set new target when chaning grid position
        gridTargetPosition = GetWorldPosition();
    }
    public Vector3 GetWorldPosition()
    {
        //get world position
        Vector3 worldPosition = Vector3.zero;

        if (gridType == Map.GRIDTYPE_OWN_INVENTORY)
        {
            worldPosition = map.ownInventoryGridPositions[gridPositionX];
        }
        else if (gridType == Map.GRIDTYPE_HEXA_MAP)
        {
            worldPosition = map.mapGridPositions[gridPositionX, gridPositionZ];

        }

        return worldPosition;
    }
    public void OnCombatStart()
    {
        //IsDragged = false;

        this.transform.position = gridTargetPosition;


        //in combat grid
        if (gridType == Map.GRIDTYPE_HEXA_MAP)
        {
            //isInCombat = true;
            gameState = GameState.GAME_STATE_COMBAT;

            navMeshAgent.enabled = true;

            TryAttackNewTarget();

        }

    }
    private void TryAttackNewTarget()
    {
        //find closest enemy
        target = FindTarget();

        //if target found
        if (target != null)
        {
            //set pathfinder target
            navMeshAgent.destination = target.transform.position;


            navMeshAgent.isStopped = false;
        }
    }
    private GameObject FindTarget()
    {
        GameObject closestEnemy = null;
        float bestDistance = 1000;

        //find enemy
        if (teamID == TEAMID_PLAYER)
        {

            for (int x = 0; x < Map.hexMapSizeX; x++)
            {
                for (int z = 0; z < Map.hexMapSizeZ / 2; z++)
                {
                    if (aIOpponent.gridMinionsArray[x, z] != null)
                    {
                        ChampionController championController = aIOpponent.gridMinionsArray[x, z].GetComponent<ChampionController>();

                        if (championController.isDead == false)
                        {
                            //calculate distance
                            float distance = Vector3.Distance(this.transform.position, aIOpponent.gridMinionsArray[x, z].transform.position);

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
        }
        else if (teamID == TEAMID_AI)
        {

            for (int x = 0; x < Map.hexMapSizeX; x++)
            {
                for (int z = 0; z < Map.hexMapSizeZ / 2; z++)
                {
                    if (gameplay.gridChampionsArray[x, z] != null)
                    {
                        ChampionController championController = gameplay.gridChampionsArray[x, z].GetComponent<ChampionController>();

                        if (championController.isDead == false)
                        {
                            //calculate distance
                            float distance = Vector3.Distance(this.transform.position, gameplay.gridChampionsArray[x, z].transform.position);

                            //if new this champion is closer then best distance
                            if (distance < bestDistance)
                            {
                                bestDistance = distance;
                                closestEnemy = gameplay.gridChampionsArray[x, z];
                            }
                        }
                    }
                }
            }

        }


        return closestEnemy;
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
public enum GameState
{
    GAME_STATE_WAITING,
    GAME_STATE_COMBAT,
    GAME_STATE_SHOP,
    GAME_STATE_PLACEMENT,
    GAME_STATE_END
}
public enum MinionState
{
    MINION_STATE_IDLE,
    MINION_STATE_MOVING,
    MINION_STATE_ATTACKING,
    MINION_STATE_DEAD
}
