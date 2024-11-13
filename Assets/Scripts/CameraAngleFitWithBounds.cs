using UnityEngine;

public class CameraAngleFitWithBounds : MonoBehaviour
{
    Transform object1; // The closer object (near bottom-left)
    Transform object2; // The farther object (near top-right)
    [SerializeField]
    [Range(1.01f, 10f)]
    float padding = 1.2f; // Logarithmic factor fior distantiating from screen edges
    [SerializeField]
    [Range(1.01f, 5f)]
    float objectsDistanceRatio = 2f; // Number by which object1 is closer to camera than object2;
    [SerializeField]
    [Range(1, 50)]
    int adjustCycles = 5; // How precise is calculation;
    private Camera cam;

    public void Init()
    {
        cam = Camera.main;
        object1 = Manager.projectileSpawner.transform;
        object2 = Manager.target;
        for (int i = 0; i < adjustCycles; i++)
            PositionAndRotateCameraToFit();
    }
    public void PositionAndRotateCameraToFit()
    {
        if (object1 == null || object2 == null || cam == null) return;

        //find desired screen points in world
        float paddingX = (Screen.width - Screen.width / padding)/2f;
        float paddingY = (Screen.height - Screen.height / padding) / 2f;
        Vector3 bottomLeft = cam.ScreenToWorldPoint(new Vector3(paddingX, paddingY, cam.nearClipPlane));
        Vector3 topRight = cam.ScreenToWorldPoint(new Vector3(Screen.width- paddingX, Screen.height- paddingY, cam.nearClipPlane));
    
        //find desired points for world objects to fit screen without moving camera
        Vector3 lookAtPoint = (bottomLeft + topRight) / 2f;
        Vector3 tmpPosAUnscaled = cam.transform.position+ (bottomLeft- cam.transform.position).normalized;
        Vector3 tmpPosBUnscaled = cam.transform.position+ (topRight- cam.transform.position).normalized* objectsDistanceRatio;
        float distTmpAB = Vector3.Distance(tmpPosAUnscaled, tmpPosBUnscaled);
        float actualDistanceAB = (Vector3.Distance(object1.position, object2.position));
        float tmpScaleFactor = actualDistanceAB / distTmpAB;
        Vector3 tmpPosAScaled = cam.transform.position+(bottomLeft - cam.transform.position).normalized * tmpScaleFactor;
        Vector3 tmpPosBScaled = cam.transform.position+(topRight - cam.transform.position).normalized * tmpScaleFactor * objectsDistanceRatio;

        //find linear and angular shifts foor camera and screen to align desired points with actual positions of objects 
        Vector3 shift = object1.transform.position - tmpPosAScaled;
        Vector3 camPos = cam.transform.position;
        tmpPosBScaled += shift;
        camPos += shift;
        lookAtPoint += shift;
        float shiftAngle = Vector3.Angle((tmpPosBScaled - object1.position), (object2.position - object1.position));
        Vector3 shiftAxis = Vector3.Cross((tmpPosBScaled - object1.position), (object2.position - object1.position)).normalized;
        Quaternion shiftRotation = Quaternion.AngleAxis(shiftAngle, shiftAxis);
        Vector3 camPosRelative = camPos - object1.position;
        Vector3 lookAtRelative = lookAtPoint - object1.position;

        //apply rotations
        camPosRelative = shiftRotation * camPosRelative;
        lookAtRelative = shiftRotation * lookAtRelative;
        camPos = object1.position + camPosRelative;
        lookAtPoint = object1.position + lookAtRelative;

        //set calculated pos and rotation to camera
        cam.transform.position = camPos;
        cam.transform.LookAt(lookAtPoint);
    }
}