using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Jonatan Hernández García
public class Cam : MonoBehaviour
{
    Camera cam;
    void Start()
    {
        cam = Camera.main;
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = new Color(120f / 255f, 97f / 255f, 47f / 255f);
        cam.transform.localPosition = new Vector3(-3.56f, 2.6f, -1.87f);
        cam.transform.localRotation = Quaternion.Euler(34.86f, 38.8f, 0f);
    }
}
