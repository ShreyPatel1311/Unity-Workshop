using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardMissile : MonoBehaviour, MissileMovement
{
    [SerializeField] private float missileSpeed;

    private static List<GameObject> selectedTargets;

    public List<GameObject> SelectedTargets { get => selectedTargets; set => selectedTargets = value; }

    public void Movement()
    {
        if(selectedTargets.Capacity > 0)
        {
            transform.position += transform.forward * missileSpeed * Time.deltaTime;
        }
        else
        {
            return;
        }
    }

    void Update()
    {
        Movement();
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }
}
