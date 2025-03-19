using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MissleAttack : MonoBehaviour
{
    [SerializeField] private Transform wings;
    [SerializeField] private Transform LookAt;
    [SerializeField] private LineOfSight los;

    [Header("HUD")]
    [SerializeField] private GameObject target;
    [SerializeField] private Sprite possibleTargetSprite;
    [SerializeField] private GameObject possibleTargetParent;

    [Header("Missile")]
    [SerializeField] private GameObject missilePrefab;

    [Header("Bullet")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletLifeTime = 5f;
    [SerializeField] private float fireRate = 5f;

    private RectTransform targetTransform;
    private GameObject[] allTargetList;
    private GameObject[] allTargetListUI;

    private Hashtable nearestEnemy;

    public float FireRate { get => fireRate; }

    private void UpdateHashTable()
    {
        if(allTargetList.Length >= nearestEnemy.Count)
        {
            foreach (GameObject target in allTargetList) 
            {
                nearestEnemy[target] = Vector3.Distance(transform.position, target.transform.position);
            }
        }
        else
        {
            foreach (GameObject target in allTargetList)
            {
                nearestEnemy.Add(target, Vector3.Distance(transform.position, target.transform.position));
            }
        }
    }

    private void Start()
    {
        if(los == null)
        {
            los = Camera.main.GetComponent<LineOfSight>();
        }
        targetTransform = target.GetComponent<RectTransform>();
        allTargetList = GameObject.FindGameObjectsWithTag("Turret");
        allTargetListUI = new GameObject[allTargetList.Length];
        for (int i=0; i<allTargetList.Length; i++)
        {
            allTargetListUI[i] = Instantiate(target, possibleTargetParent.transform);
            allTargetListUI[i].GetComponent<Image>().sprite = possibleTargetSprite;
        }
    }

    private void Update()
    {
        GameObject nearest = FindNearestTurret(los.visibleEnemy);
        if (nearest != null)
        {
            Debug.Log(nearest.name);
            Vector3 pos = Camera.main.WorldToScreenPoint(nearest.transform.position);
            targetTransform.position = pos;
        }

        if (Input.GetAxisRaw("Fire1") != 0f)
        {
            Instantiate(missilePrefab, nearest.transform.position, Quaternion.identity);
        }
    }

    public void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, wings.position, Quaternion.identity);
        bullet.transform.LookAt(LookAt);
        Destroy(bullet, bulletLifeTime);
    }

    private int FindEnemy(GameObject t)
    {
        for (int i = 0; i < allTargetList.Length; i++)
        {
            if (allTargetList[i] == t)
            {
                return i;
            }
        }
        return -1;
    }

    private GameObject FindNearestTurret(List<GameObject> visibleEnemy)
    {
        GameObject nearest = visibleEnemy[0];

        float dis = Vector3.Distance(this.transform.position, nearest.transform.position);

        foreach (GameObject t in visibleEnemy)
        {
            UIOnGameObject(FindEnemy(t));
            if (t != nearest && Vector3.Distance(t.transform.position, this.transform.position) < dis)
            {
                dis = Vector3.Distance(t.transform.position, this.transform.position);
                nearest = t;
            }
        }
        return nearest;
    }

    private void UIOnGameObject(int index)
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(allTargetList[index].transform.position);
        allTargetListUI[index].GetComponent<RectTransform>().position = pos;
    }
}