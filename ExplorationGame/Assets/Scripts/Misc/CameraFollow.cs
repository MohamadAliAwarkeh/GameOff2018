using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour {

    //Public Variables
    [Header("Player Variables")]
    public List<Transform> targets;

    [Header("Camera Variables")]
    public Vector3 offset;
    public float smoothTime;
    public float minZoom;
    public float maxZoom;
    public float zoomLimiter;

    [Header("Script References")]
    public PlayersManager playerManager;

    //Private Variables
    private Vector3 velocity;
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        //To make sure we do not get an error
        if (targets.Count == 0)
        {
            return;
        }

        Move();
        //Zoom();
    }

    void Move()
    {
        //Setting the center point of the camera 
        Vector3 centerPoint = GetCenterPoint();

        //Adding a little offset to the camera
        Vector3 newPosition = centerPoint + offset;

        //This finally moves the camera to follow the center position
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }

    void Zoom()
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / zoomLimiter);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
    }

    Vector3 GetCenterPoint()
    {
        //If there is only one player in the scene, then we set the camera to 
        //follow the one player
        if (targets.Count == 1)
        {
            return targets[0].position;
        }

        //Otherwise, if there is more than one player in the scene, then we
        //set the camera to find the bound between all players and get the center point
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }

        return bounds.center;
    }

    float GetGreatestDistance()
    {
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }

        return bounds.size.x;
    }
}
