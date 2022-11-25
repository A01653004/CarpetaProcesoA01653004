using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Walk;

//Jonatan Hernández García A01653004
public class LegRob : MonoBehaviour
{
    string side;
    public void Init(string _side, ref List<GameObject> go_parts, ref List<Matrix4x4> m_locations, ref List<Matrix4x4> m_scales)
    {
        if (_side == "LEFT")
        {
            side = "LEFT";
            //LTHIGH
            INIT_PART(ref go_parts, ref m_locations, ref m_scales, PrimitiveType.Cube, (int)PARTS.LTHIGH, Color.white, "LTHIGH", new Vector3(0.4f, 0.8f, 0.4f), new Vector3(0f, -0.575f, 0.32f));
            //LLEG
            INIT_PART(ref go_parts, ref m_locations, ref m_scales, PrimitiveType.Cube, (int)PARTS.LLEG, Color.blue, "LLEG", new Vector3(0.5f, 1f, 0.5f), new Vector3(0f, -0.9f, 0.01f));
            //LFOOT
            INIT_PART(ref go_parts, ref m_locations, ref m_scales, PrimitiveType.Cube, (int)PARTS.LFOOT, Color.blue, "LFOOT", new Vector3(0.7f, 0.3f, 0.5f), new Vector3(-0.103f, -0.625f, 0));

        }
        else if (_side == "RIGHT")
        {
            side = "RIGHT";
            //RTHIGH
            INIT_PART(ref go_parts, ref m_locations, ref m_scales, PrimitiveType.Cube, (int)PARTS.RTHIGH, Color.white, "RTHIGH", new Vector3(0.4f, 0.8f, 0.4f), new Vector3(0f, -0.575f, -0.32f));
            //RLEG
            INIT_PART(ref go_parts, ref m_locations, ref m_scales, PrimitiveType.Cube, (int)PARTS.RLEG, Color.blue, "RLEG", new Vector3(0.5f, 1f, 0.5f), new Vector3(0f, -0.9f, -0.01f));
            //RFOOT
            INIT_PART(ref go_parts, ref m_locations, ref m_scales, PrimitiveType.Cube, (int)PARTS.RFOOT, Color.blue, "RFOOT", new Vector3(0.7f, 0.3f, 0.5f), new Vector3(-0.103f, -0.625f, 0));
        }
    }
    public void Draw(ref Matrix4x4 heapMatrix, ref List<GameObject> go_parts, List<Matrix4x4> m_locations, List<Matrix4x4> m_scales, BACK_FORTH rX, Vector3[] v3_originals, BACK_FORTH rk)
    {
        Matrix4x4 accumT = Matrix4x4.identity;
        float movementLegL;
        float movementLegR;

        if (side == "LEFT")
        {
            for (int i = (int)PARTS.LTHIGH; i <= (int)PARTS.LFOOT; i++)
            {
                Matrix4x4 m = accumT * m_locations[i] * m_scales[i];
                if (i == (int)PARTS.LTHIGH)
                {
                    accumT = heapMatrix;
                    Matrix4x4 t1 = Transforms.Translate(0f, -0.175f, 0.32f);
                    Matrix4x4 t2 = Transforms.Translate(0f, -0.4f, 0.0f);
                    Matrix4x4 r = Transforms.RotateZ(-rX.val * 4f);
                    m = accumT * t1 * r * t2 * m_scales[i];
                    accumT *= t1 * r * t2;

                }
                else if (i == (int)PARTS.LLEG)
                {
                    Matrix4x4 t1 = Transforms.Translate(0f, -0.4f, 0f);
                    Matrix4x4 t2 = Transforms.Translate(0f, -0.5f, 0f);
                    Matrix4x4 r = new Matrix4x4();
                    if (rX.dir > 0)
                    {
                        r = Transforms.RotateZ(rk.val * 5f);
                    }
                    else r = Transforms.RotateZ(0);
                    m = accumT * t1 * r * t2 * m_scales[i];
                    accumT *= t1 * r * t2;
                }
                else 
                {
                    Matrix4x4 t1 = Transforms.Translate(0f, -0.5f, 0f);
                    Matrix4x4 t2 = Transforms.Translate(-0.103f, -0.15f, 0f);
                    movementLegL = Mathf.Max(-rX.val * 4f, 0);
                    Matrix4x4 r = Transforms.RotateZ(rX.val * 2f);
                    m = accumT * t1 * r * t2 * m_scales[i];
                    accumT *= t1 * r * t2;
                } 

                go_parts[i].GetComponent<MeshFilter>().mesh.vertices = Transforms.Transform(m, v3_originals);
            }
        }
        else if (side == "RIGHT")
        {
            for (int i = (int)PARTS.RTHIGH; i <= (int)PARTS.RFOOT; i++)
            {
                
                Matrix4x4 m = accumT * m_locations[i] * m_scales[i];
                if (i == (int)PARTS.RTHIGH)
                {
                    accumT = heapMatrix;
                    Matrix4x4 t1 = Transforms.Translate(0f, -0.175f, -0.32f);
                    Matrix4x4 t2 = Transforms.Translate(0f, -0.4f, 0.0f);
                    Matrix4x4 r = Transforms.RotateZ(rX.val * 4f);
                    m = accumT * t1 * r * t2 * m_scales[i];
                    accumT *= t1 * r * t2;

                }
                else if (i == (int)PARTS.RLEG)
                {
                    Matrix4x4 t1 = Transforms.Translate(0f, -0.4f, 0f);
                    Matrix4x4 t2 = Transforms.Translate(0f, -0.5f, 0f);
                    Matrix4x4 r = new Matrix4x4();
                    if (rX.dir < 0) {
                        r = Transforms.RotateZ(-rk.val * 5f);
                    } 
                    else r = Transforms.RotateZ(0);

                    m = accumT * t1 * r * t2 * m_scales[i];
                    accumT *= t1 * r * t2;
                }
                else
                {
                    Matrix4x4 t1 = Transforms.Translate(0f, -0.5f, 0f);
                    Matrix4x4 t2 = Transforms.Translate(-0.103f, -0.15f, 0f);
                    Matrix4x4 r = Transforms.RotateZ(-rX.val * 2f);
                    m = accumT * t1 * r * t2 * m_scales[i];
                    accumT *= t1 * r * t2;
                }
                go_parts[i].GetComponent<MeshFilter>().mesh.vertices = Transforms.Transform(m, v3_originals);
            }
        }
    }
}