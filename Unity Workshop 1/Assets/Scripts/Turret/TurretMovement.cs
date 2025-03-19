using UnityEngine;

public class TurretMovement : MonoBehaviour
{
    [SerializeField] private Transform barrelRotator;
    [SerializeField] private Transform turretRotator;

    private TurretAttack ta;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        ta = GetComponent<TurretAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        turretRotator.LookAt(new Vector3(ta.NextPosition.x, 0, ta.NextPosition.z), transform.forward);
        barrelRotator.LookAt(new Vector3(ta.NextPosition.x, ta.NextPosition.y, ta.NextPosition.z));
    }
}
