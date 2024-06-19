using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIOponent : MonoBehaviour
{
    [SerializeField] private GameObject minion;
    public GameObject[,] gridMinionsArray;
    public void OnMapReady()
    {
        gridMinionsArray = new GameObject[Map.hexMapSizeX, Map.hexMapSizeZ / 2];
        AddRandomChampion();
    }
    public void AddRandomChampion()
    {
        //get an empty slot
      
        GetEmptySlot(out int indexX, out int  indexZ);

        //dont add champion if there is no empty slot
        if (indexX == -1 || indexZ == -1)
            return;

        Minion minion = this.minion.GetComponent<Minion>();

        //instantiate champion prefab
        GameObject minionPrefab = Instantiate(minion.gameObject);

        //add champion to array
        gridMinionsArray[indexX, indexZ] = minionPrefab;

        //get champion controller
        //ChampionController championController = minionPrefab.GetComponent<ChampionController>();

        //setup chapioncontroller
       // minion.Init(minion, ChampionController.TEAMID_AI);

        //set grid position
        minion.SetGridPosition(Map.GRIDTYPE_HEXA_MAP, indexX, indexZ + 4);

        //set position and rotation
        //minion.SetWorldPosition();
        //minion.SetWorldRotation();

        //check for champion upgrade
        List<ChampionController> championList_lvl_1 = new List<ChampionController>();
        List<ChampionController> championList_lvl_2 = new List<ChampionController>();

        for (int x = 0; x < Map.hexMapSizeX; x++)
        {
            for (int z = 0; z < Map.hexMapSizeZ / 2; z++)
            {
                //there is a champion
                if (gridMinionsArray[x, z] != null)
                {
                    //get character
                    ChampionController cc = gridMinionsArray[x, z].GetComponent<ChampionController>();

                    //check if is the same type of champion that we are buying
                    if (cc.champion == minion)
                    {
                        if (cc.lvl == 1)
                            championList_lvl_1.Add(cc);
                        else if (cc.lvl == 2)
                            championList_lvl_2.Add(cc);
                    }
                }

            }
        }

        //if we have 3 we upgrade a champion and delete rest
        if (championList_lvl_1.Count == 3)
        {
            //upgrade
            championList_lvl_1[2].UpgradeLevel();

            //destroy gameobjects
            Destroy(championList_lvl_1[0].gameObject);
            Destroy(championList_lvl_1[1].gameObject);

            //we upgrade to lvl 3
            if (championList_lvl_2.Count == 2)
            {
                //upgrade
                championList_lvl_1[2].UpgradeLevel();

                //destroy gameobjects
                Destroy(championList_lvl_2[0].gameObject);
                Destroy(championList_lvl_2[1].gameObject);
            }
        }


       // CalculateBonuses();
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
                    break;
                }
            }
        }
    }
}
