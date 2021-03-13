using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class CameraScript : MonoBehaviour
{

    public float panSpeed = 60f;
    public float rotationSpeed = 20f;
    public float mouseScrollSpeed = 50f;
    public float mouseSensitivity = 1f;

    public GameMasterScript gameMasterScript;
    public GameObject volumeObject;
    DepthOfField depthOfField;

    // Start is called before the first frame update
    void Start()
    {
        //gameMasterScript = GameObject.Find("GameMaster").GetComponent<GameMasterScript>();

        volumeObject = GameObject.FindGameObjectWithTag("PostProsessing");
        VolumeProfile profile = volumeObject.GetComponent<Volume>().profile;
        profile.TryGet(out depthOfField);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = transform.position;
        Vector3 rotation = transform.rotation.eulerAngles;


        if(Input.GetKey("w"))
        {
            Vector3 forward = new Vector3(transform.forward.x, 0f, transform.forward.z);
            Vector3.Normalize(forward);
            position += forward * panSpeed * Time.unscaledDeltaTime;
        }

        if (Input.GetKey("s"))
        {
            Vector3 backward = new Vector3(-transform.forward.x , 0f, -transform.forward.z);
            Vector3.Normalize(backward);
            position += backward * panSpeed * Time.unscaledDeltaTime;
        }


        if (Input.GetKey("a"))
        {
            Vector3 left = new Vector3(-transform.right.x, 0f, -transform.right.z);
            Vector3.Normalize(left);
            position += left * panSpeed * Time.unscaledDeltaTime;
        }

        if (Input.GetKey("d"))
        {
            Vector3 right = new Vector3(transform.right.x, 0f, transform.right.z);
            Vector3.Normalize(right);
            position += right * panSpeed * Time.unscaledDeltaTime;
        }


        if (Input.GetKey("q"))
        {
            position.y -= panSpeed * Time.unscaledDeltaTime;
        }

        if (Input.GetKey("e"))
        {
            position.y += panSpeed * Time.unscaledDeltaTime;
        }

        if (Input.GetMouseButton(2))
        {
            float pitch = -Input.GetAxis("Mouse Y") * mouseSensitivity;
            float yaw = Input.GetAxis("Mouse X") * mouseSensitivity;

            transform.eulerAngles = transform.eulerAngles + new Vector3(pitch, yaw, 0f);
            Cursor.lockState = CursorLockMode.Locked;
        }

        else
            Cursor.lockState = CursorLockMode.None;


        position.y = position.y + -Input.GetAxis("Mouse ScrollWheel") * mouseScrollSpeed;

        transform.position = position;

        if (!gameMasterScript.upgrading || gameMasterScript.precheckingUpgrade)
        {
            FocusCamera();
        }
        else
        {
            depthOfField.focusDistance.value = 0;
            depthOfField.aperture.value = 1f;
        }
        
    }

    void FocusCamera()
    {

        Ray raycast = new Ray(transform.position, transform.forward);
        Physics.Raycast(raycast, out RaycastHit hitPoint, 100f);
        float rayLength = Vector3.Distance(raycast.origin, hitPoint.point);
        //Debug.Log(rayLength);

        float depthOfFieldTarget = rayLength;
        float apertureTarget = 4.25f - rayLength * 0.065f;
        depthOfField.focusDistance.value = Mathf.Lerp(depthOfField.focusDistance.value, depthOfFieldTarget, 4f * Time.unscaledDeltaTime);
        depthOfField.aperture.value = Mathf.Lerp(depthOfField.aperture.value, apertureTarget, 4f * Time.unscaledDeltaTime);

    }
}
