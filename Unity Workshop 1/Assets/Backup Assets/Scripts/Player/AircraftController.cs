using UnityEngine;

public class AircraftController : MonoBehaviour
{
    [SerializeField] private float thrustSpeed;
    [SerializeField] private float thrustAcc;
    [SerializeField] private float rollSpeed;
    [SerializeField] private float rollAcc;
    [SerializeField] private float pitchValue;
    [SerializeField] private float yawValue;

    private Vector2 screenCenter, lookInput, mouseDistance;
    private float rollInput;
    [SerializeField]private float activeThrustValue;

    // Start is called before the first frame update
    void Start()
    {
        screenCenter.x = Screen.width/2;
        screenCenter.y = Screen.height/2;
    }

    // Update is called once per frame
    void Update()
    {
        float roll = Input.GetAxisRaw("Horizontal");
        float pitch = Input.GetAxisRaw("Vertical");
        float yaw = Input.GetAxisRaw("Yaw");

        lookInput.x = Input.mousePosition.x;
        lookInput.y = Input.mousePosition.y;

        mouseDistance = GetActiveMouseDistanceValue(lookInput, screenCenter);
        mouseDistance = mouseDistance.normalized;
        float yawActual = mouseDistance.x;
        float pitchActual = mouseDistance.y;

        rollInput = Mathf.Lerp(rollInput, -roll * rollSpeed, rollAcc * Time.deltaTime);

        if(Mathf.Abs(mouseDistance.x) < 0.1f)
        {
            yawActual = yaw;
        }
        if(Mathf.Abs(mouseDistance.y) < 0.1f)
        {
            pitchActual = pitch;
        }
        else
        {
            activeThrustValue = SpeedChanger(pitch, activeThrustValue, thrustSpeed, thrustAcc);
        }

        transform.Rotate(pitchActual * pitchValue * Time.deltaTime, yawActual * yawValue * Time.deltaTime, rollInput * Time.deltaTime, Space.Self);
        transform.position += (transform.forward * Mathf.Abs(activeThrustValue) * Time.deltaTime);
    }

    private float SpeedChanger(float pitch, float activeThrustValue, float thrustSpeed, float thrustAcc) 
    {
        if (pitch > 0f && activeThrustValue <= thrustSpeed)
        {
            activeThrustValue += thrustAcc * Time.deltaTime;
        }
        else if (pitch >= 0f && activeThrustValue >= thrustSpeed)
        {
            activeThrustValue = thrustSpeed;
        }
        else if (pitch < 0f && activeThrustValue > 0f)
        {
            activeThrustValue -= thrustAcc * Time.deltaTime;
        }
        return activeThrustValue;
    }

    private Vector2 GetActiveMouseDistanceValue(Vector2 lookInput, Vector2 screenCenter)
    {
        Vector2 mouseD = Vector2.zero;
        if(Mathf.Abs((lookInput.x - screenCenter.x)/screenCenter.x) < 0.1f)
        {
            mouseD.x = 0f;
        }
        else
        {S
            mouseD.x = (lookInput.x - screenCenter.x)/screenCenter.x;
        }
        if(Mathf.Abs((lookInput.y - screenCenter.y) / screenCenter.y) < 0.1f)
        {
            mouseD.y = 0f;
        }
        else
        {
            mouseD.y = (-lookInput.y + screenCenter.y) / screenCenter.y;
        }
        return mouseD;
    }
}
