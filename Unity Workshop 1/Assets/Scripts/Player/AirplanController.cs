using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AirplanController : MonoBehaviour
{
    private AudioSource audioS;
    private Rigidbody rb;
    private MissleAttack ma;
    private float pitch;
    private float roll;
    private float yaw;
    private float thrust;
    private int count;

    [Header("Airplane Properties")]
    [SerializeField] private float thrustAcc = 0.5f;
    [SerializeField] private float maxThrust = 400;
    [SerializeField] private float lift = 135f;
    [SerializeField] private float responsiveness = 10f;

    [Header("target")]
    [SerializeField] private TextMeshProUGUI thrustTMP;
    [SerializeField] private TextMeshProUGUI airSpeedTMP;
    [SerializeField] private TextMeshProUGUI altitudeTMP;

    [Header("Audio")]
    [SerializeField] private GameObject thrustObject;
    [SerializeField] private AudioClip jetEngineStart;
    [SerializeField] private AudioClip jetInAir;

    private float responseModifier { get
        {
            return (Rb.mass / 10f) * responsiveness;
        } 
    }

    public Rigidbody Rb { get => rb; set => rb = value; }

    // Start is called before the first frame update
    void Start()
    {
        audioS = thrustObject.GetComponent<AudioSource>();
        Rb = GetComponent<Rigidbody>();
        ma = GetComponent<MissleAttack>();
        StartCoroutine(Shoot());
    }

    // Update is called once per frame
    void Update()
    {
        pitch = Input.GetAxisRaw("Vertical");
        roll = Input.GetAxisRaw("Horizontal");
        yaw = Input.GetAxisRaw("Yaw");
        if (Input.GetKey(KeyCode.Space)) thrust += thrustAcc;
        if (Input.GetKey(KeyCode.LeftShift)) thrust -= thrustAcc;
        thrust = Mathf.Clamp(thrust, 0f, 100f);

        UpdateHUD();
        //AdjustThrustSoundFX(thrust);
    }

    private void FixedUpdate()
    {
        Rb.AddForce(transform.forward * maxThrust * thrust);
        Rb.AddTorque(transform.up * yaw * responseModifier);
        Rb.AddTorque(-transform.forward * roll * responseModifier);
        Rb.AddTorque(-transform.right * pitch * responseModifier);

        Rb.AddForce(Vector3.up * lift * Rb.linearVelocity.magnitude);
    }

    private void AdjustThrustSoundFX(float thrust)
    {
        if(thrust < 50 && Rb.linearVelocity.magnitude * 3.6f < 125f && transform.position.y < 15f)
        {
            audioS.PlayOneShot(jetEngineStart);
        }
        else
        {
            if(thrust > 50f)
            {
                audioS.PlayOneShot(jetInAir);
            }
            audioS.volume = thrust / 100f;
        }
    }

    private void UpdateHUD()
    {
        thrustTMP.text = "Thrust : " + thrust.ToString("F0") + "%\n";
        airSpeedTMP.text = "Air Speed : " + (Rb.linearVelocity.magnitude * 3.6f).ToString("F0") + "km\\hr";
        altitudeTMP.text = "Altitude : " + transform.position.y.ToString("F0") + "m";
    }

    private IEnumerator Shoot()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            ma.Shoot();
        }

        yield return new WaitForSeconds(1/ma.FireRate);
        StartCoroutine(Shoot());
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(count++);   
    }
}
