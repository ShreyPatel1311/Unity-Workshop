using UnityEngine;

public class PlaneCamera : MonoBehaviour
{
    [SerializeField] private Transform player;

    [SerializeField] private Vector2 lookLimit;
    [SerializeField] private float sensX;
    [SerializeField] private float sensY;

    [SerializeField] private float clampMinZ;
    [SerializeField] private float clampMaxZ;
    [SerializeField] private float clampMinY;
    [SerializeField] private float clampMaxY;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (transform.rotation.z >= clampMinZ && transform.rotation.z <= clampMaxZ)
            transform.RotateAround(player.position, Vector3.right, Input.GetAxisRaw("Mouse Y") * sensY * Time.deltaTime);
        if (transform.rotation.y >= clampMinY && transform.rotation.y <= clampMaxY)
            transform.RotateAround(player.position, Vector3.up, Input.GetAxisRaw("Mouse X") * sensX * Time.deltaTime);
    }
}
