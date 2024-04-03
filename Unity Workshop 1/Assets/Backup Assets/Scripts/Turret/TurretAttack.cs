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

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        audioS = GetComponent<AudioSource>();
        StartCoroutine(ShotBullet());
    }

    // Update is called once per frame
    private IEnumerator ShotBullet()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < turretRange)
        {
            bullet = Instantiate(bulletPrefab, barrel.transform.position, Quaternion.identity);
            audioS.PlayOneShot(clip);
            bullet.transform.LookAt(player.transform.position);
            Destroy(bullet, lifetime);
        }

        yield return new WaitForSecondsRealtime(1/fireRate);
        StartCoroutine(ShotBullet());
    }
}
