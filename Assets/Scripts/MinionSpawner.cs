using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MinionSpawner : MonoBehaviour
{
    [Inject] private Minion.Factory minionFactory;
    private void Start()
    {
        minionFactory.Create();
    }
}
