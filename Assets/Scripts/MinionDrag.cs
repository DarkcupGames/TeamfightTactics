using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MinionDrag : MonoBehaviour
{
    [Inject] private Map map;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private GameObject target;
    [SerializeField] private TriggerInfo currentTriggerInfo;
    private Plane plane;
    private void Start()
    {
        plane = new Plane(Vector3.up, Vector3.zero);
    }
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            IndicatorDrag();
        }
        if (Input.GetMouseButtonUp(0))
        {
            currentTriggerInfo = null;
            map.resetIndicators();
        }
    }
    private void StartDrag()
    {


    }
    private void IndicatorDrag()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, layerMask, QueryTriggerInteraction.Collide))
        {
            TriggerInfo newTriggerInfo = hit.collider.gameObject.GetComponent<TriggerInfo>();
            if (newTriggerInfo != null)
            {
                if(!newTriggerInfo.Equals(currentTriggerInfo))
                {
                    currentTriggerInfo = newTriggerInfo;
                    map.resetIndicators();
                    return;
                }
                GameObject indicator = map.GetIndicatorFromTriggerInfo(currentTriggerInfo);
                indicator.GetComponent<MeshRenderer>().material.color = map.indicatorActiveColor;
            }
        }

    }
    private void GetObjectInWorld()
    {
   
    }
}
