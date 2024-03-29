using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class MissleAttack : MonoBehaviour
{
    [SerializeField] private Transform wings;
    [SerializeField] private GameObject missilePrefab;
    private LineOfSight los;

    private void Start()
    {
        los = GetComponent<LineOfSight>();
    }

    private void Update()
    {
        GameObject nearest = FindNearestTurret(los.visibleEnemy);
        //if (Input.GetAxisRaw("Fire") != 0f)
        //{
        //    Instantiate(missilePrefab, nearest.transform.position, Quaternion.identity);
        //}
    }

    private GameObject FindNearestTurret(List<GameObject> visibleEnemy)
    {
        GameObject nearest = visibleEnemy[0];
        float dis = Vector3.Distance(this.transform.position, nearest.transform.position);

        foreach (GameObject t in visibleEnemy)
        {
            if (t != nearest && Vector3.Distance(t.transform.position, this.transform.position) < dis)
            {
                dis = Vector3.Distance(t.transform.position, this.transform.position);
                nearest = t;
            }
        }
        return nearest;
    }
}
