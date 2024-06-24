using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Gameplay : MonoInstaller
{
    [SerializeField] private GameState gameState;
    [SerializeField] public GameObject minion;
    public GameObject[,] gridMinionsArray;
    public GameState GameState => gameState;
    private void Start()
    {
        gameState = GameState.GAME_STATE_WAITING;
        gridMinionsArray = new GameObject[Map.hexMapSizeX, Map.hexMapSizeZ / 2];
        gridMinionsArray[0, 0] = minion;
    }
}

public enum TeamID
{
    TEAMID_PLAYER,
    TEAMID_AI
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
public enum GridType
{
    GRIDTYPE_OWN_INVENTORY,
    GRIDTYPE_OPPONENT_INVENTORY,
    GRIDTYPE_HEXA_MAP
}