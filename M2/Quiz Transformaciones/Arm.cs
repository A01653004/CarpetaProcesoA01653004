using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Luis Gerardo Reyes Lozano
// Jonatan Hernández García

public class Arm : MonoBehaviour
{
    GameObject cube;
    GameObject cube2;
    GameObject cube3;
    Vector3[] verticesC;
    float grados;
    float actual;
    float extra;
    private void Start()
    {
        actual = 0;
        grados = 45;
        extra = .25f;
        cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        verticesC = cube.GetComponent<MeshFilter>().mesh.vertices;
        cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
    }

    private void Update()
    {
        Matrix4x4 tr1 = Transforms.RotateZ(actual)*Transforms.Translate(.5f,0,0);
        Matrix4x4 tr2 = Transforms.RotateZ(actual)*Transforms.Translate(.5f,0,0)* Transforms.Translate(.5f, 0, 0)* Transforms.RotateZ(actual)* Transforms.Translate(.5f, 0, 0);
        Matrix4x4 tr3 = Transforms.RotateZ(actual)*Transforms.Translate(.5f,0,0)* Transforms.Translate(.5f, 0, 0)* Transforms.RotateZ(actual)* Transforms.Translate(.5f, 0, 0) *Transforms.Translate(.5f, 0, 0)* Transforms.RotateZ(actual) * Transforms.Translate(.5f, 0, 0);
        Matrix4x4 ts1 = Transforms.Scale(1, .5f, .5f);
        cube.GetComponent<MeshFilter>().mesh.vertices = Transforms.Transform(tr1 * ts1, verticesC);
        cube.GetComponent<MeshFilter>().mesh.RecalculateNormals();
        cube2.GetComponent<MeshFilter>().mesh.vertices = Transforms.Transform(tr2 * ts1, verticesC);
        cube2.GetComponent<MeshFilter>().mesh.RecalculateNormals();
        cube3.GetComponent<MeshFilter>().mesh.vertices = Transforms.Transform(tr3 * ts1, verticesC);
        cube3.GetComponent<MeshFilter>().mesh.RecalculateNormals();
        if (actual > grados)
        {
            grados = -45;
            extra = -.25f;
        }
        else
        {
            grados = +45;
            extra = +.25f;
        }
        actual += extra;
    }
}
