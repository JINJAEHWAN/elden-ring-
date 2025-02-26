using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UIElements;

public class SpringArm : MonoBehaviour
{
    [SerializeField] float rotSpeed = 1.0f;
    [SerializeField] LayerMask crashMask;
    public Transform myCam;
    float camDist = 0.0f;
    float rotX, rotY, targetRotX, targetRotY;
    float targetDist;
    private void Start()
    {
        targetDist = camDist = Mathf.Abs(myCam.transform.localPosition.x);
        rotX = transform.localRotation.eulerAngles.x;
        if (rotX > 180.0f) rotX -= 360.0f;
        targetRotX = rotX;

        targetRotY = rotY = transform.parent.localRotation.eulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButton(1))
        {
            float temp = Input.GetAxis("Mouse Y") * rotSpeed;
            targetRotY = Mathf.Clamp(targetRotY + temp, -80.0f, 80.0f);

            temp = Input.GetAxis("Mouse X") * rotSpeed;
            targetRotX += temp;

        }

        rotX = Mathf.Lerp(rotX, targetRotX, Time.deltaTime * rotSpeed);
        rotY = Mathf.Lerp(rotY, targetRotY, Time.deltaTime * rotSpeed);


        transform.localRotation = Quaternion.Euler(-targetRotY, 0, 0);
        transform.parent.localRotation = Quaternion.Euler(0, targetRotX, 0);

        float tmpScroll = Input.GetAxis("Mouse ScrollWheel");
        //if (!Mathf.Approximately(tmpScroll, 0))
        {
            camDist = Mathf.Lerp(camDist, targetDist, Time.deltaTime);
            myCam.localPosition = new Vector3(0, 0, -camDist);
        }
        float offset = 0.5f;
        if (Physics.Raycast(transform.position, -transform.forward, out RaycastHit hit, camDist + offset, crashMask))
        {
            myCam.position = hit.point + transform.forward * offset;
            camDist = hit.distance - offset;
        }
    }
}
