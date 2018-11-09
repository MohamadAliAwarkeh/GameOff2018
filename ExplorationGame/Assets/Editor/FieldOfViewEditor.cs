using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(FieldOfView))]
public class FieldOfViewEditor : Editor
{

    void OnSceneGUI()
    {
        //Getting a reference to the field of view script and get the object
        //that this is a custom editor of
        FieldOfView fov = (FieldOfView)target;

        //Drawing the view raidus
        Handles.color = Color.white;

        //We get the center which is the object attached to the FoW
        //We get the normal which is the direction around which angle it is going to rotate
        //We get the from which is where the angle will start
        //We get the angle which tells it how many degrees it must rotate
        //We get the radius which is simply the radius of the object
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.viewRadius);

        //Visualising and setting the viewAngle for the player, this creates two lines,
        //one that is negative and one positive, to represent the angle shown
        Vector3 viewAngleA = fov.DirFromAngle(-fov.viewAngle / 2, false);
        Vector3 viewAngleB = fov.DirFromAngle(fov.viewAngle / 2, false);

        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleA * fov.viewRadius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleB * fov.viewRadius);

        //Visualising when you see the target with a draw line
        //and setting it a new colour
        Handles.color = Color.red;
        foreach (Transform visibleTarget in fov.visibleTargets)
        {
            Handles.DrawLine(fov.transform.position, visibleTarget.position);
        }
    }
}
