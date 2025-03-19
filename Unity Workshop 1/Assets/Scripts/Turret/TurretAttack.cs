using System.Collections;
using UnityEngine;

public class TurretAttack : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float distance;
    [SerializeField] private GameObject barrel;
    [SerializeField] private float fireRate;
    [SerializeField] private float lifetime;
    [SerializeField] private AudioClip clip;
    [SerializeField] private float turretRange;

    private GameObject bullet;
    private GameObject player;
    private AudioSource audioS;
    private Vector3 prevPosition;
    private Vector3 currPosition;
    private Vector3 nextPosition;

    public Vector3 NextPosition { get => nextPosition; set => nextPosition = value; }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        audioS = GetComponent<AudioSource>();
        StartCoroutine(ShotBullet());
        currPosition = player.transform.position;
        prevPosition = currPosition;
    }

    void Update()
    {
        float timeToReach = CalculateTimeToReach();
        if(timeToReach > 0 && bullet != null)
        {
            currPosition = player.transform.position;
            Vector3 dir = - currPosition + prevPosition;
            float netDisplacement = Vector3.Distance(currPosition, prevPosition);
            float playerSpeed = netDisplacement / Time.deltaTime;
            float d = playerSpeed * timeToReach;
            Vector3 additive = -dir.normalized * d;
            NextPosition = additive + currPosition;
            Debug.Log("Name : " + transform.name + "\nTime to reach : " + timeToReach + "\nd : " + d + "\nNext Pos : " + NextPosition + "\ncurrent Pos : " + currPosition + "\nPlayer Speed : " + playerSpeed + "\nNet Displacement : " + netDisplacement + "\ndelta t : " + Time.deltaTime + "Player Direction : " + dir);
            prevPosition = currPosition;
        }
    }

    // Update is called once per frame
    private IEnumerator ShotBullet()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < turretRange)
        {
            bullet = Instantiate(bulletPrefab, barrel.transform.position, Quaternion.identity);
            audioS.PlayOneShot(clip);
            bullet.transform.LookAt(NextPosition);
            Destroy(bullet, lifetime);
        }

        yield return new WaitForSecondsRealtime(1/fireRate);
        StartCoroutine(ShotBullet());
    }

    private float CalculateTimeToReach()
    {
        if(bullet != null)
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);
            float speed = bullet.GetComponent<BulletMovement>().BulletSpeed;

            return distance / speed;
        }
        else
        {
            return 0;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(NextPosition, 3);
        Gizmos.DrawWireSphere (currPosition, 3);
    }
}
