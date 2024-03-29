using System.Collections;
using UnityEngine;

public class TurretAttack : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float distance;
    [SerializeField] private GameObject barrel;
    [SerializeField] private float fireRate;
    [SerializeField] private float lifetime;

    private GameObject bullet;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        StartCoroutine(ShotBullet());
    }

    // Update is called once per frame
    private IEnumerator ShotBullet()
    {
        bullet = Instantiate(bulletPrefab, barrel.transform.position, Quaternion.identity);
        bullet.transform.LookAt(player.transform.position);
        Destroy(bullet, lifetime);

        yield return new WaitForSecondsRealtime(1/fireRate);
        StartCoroutine(ShotBullet());
    }
}
