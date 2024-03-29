using UnityEngine;

public class TurretMovement : MonoBehaviour
{
    [SerializeField] private Transform barrelRotator;
    [SerializeField] private Transform turretRotator;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        turretRotator.LookAt(new Vector3(player.transform.position.x, 0, player.transform.position.z), transform.forward);
        barrelRotator.LookAt(new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z));
    }
}
