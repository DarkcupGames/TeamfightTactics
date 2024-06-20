using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;

public class AIOpponent : MonoBehaviour
{
    [SerializeField] private GameObject minion;
    public GameObject[,] gridMinionsArray;
    private void Start()
    {
        gridMinionsArray = new GameObject[Map.hexMapSizeX, Map.hexMapSizeZ / 2];
        AddRandomChampion();   
    }
    public void OnMapReady()
    {
        gridMinionsArray = new GameObject[Map.hexMapSizeX, Map.hexMapSizeZ / 2];
        AddRandomChampion();
    }
    public void AddRandomChampion()
    {
        //get an empty slot

        GetEmptySlot(out int indexX, out int indexZ);

        //dont add champion if there is no empty slot
        if (indexX == -1 || indexZ == -1)
            return;
        gridMinionsArray[indexX, indexZ] = minion;
    }
    private void GetEmptySlot(out int emptyIndexX, out int emptyIndexZ)
    {
        emptyIndexX = -1;
        emptyIndexZ = -1;

        for (int x = 0; x < Map.hexMapSizeX; x++)
        {
            for (int z = 0; z < Map.hexMapSizeZ / 2; z++)
            {
                if (gridMinionsArray[x, z] == null)
                {
                    emptyIndexX = x;
                    emptyIndexZ = z;
                    goto end;
                }
            }
        }

 end:
        Debug.Log("Empty slot not found");
    }
}
