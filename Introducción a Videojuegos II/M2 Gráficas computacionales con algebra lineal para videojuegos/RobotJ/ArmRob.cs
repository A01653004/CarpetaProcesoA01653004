using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Walk;

//Jonatan Hernández García A01653004
public class ArmRob : MonoBehaviour
{
    string side;

    public void Init(string _side, ref List<GameObject> go_parts, ref List<Matrix4x4> m_locations, ref List<Matrix4x4> m_scales)
    {
        if (_side == "LEFT")
        {
            side = "LEFT";
            //LSHOULDER
            INIT_PART(ref go_parts, ref m_locations, ref m_scales, PrimitiveType.Cube, (int)PARTS.LSHOULDER, Color.red, "LSHOULDER", new Vector3(0.45f, 0.35f, 0.4f), new Vector3(0f, 0.17f, 0.78f));
            //LARM
            INIT_PART(ref go_parts, ref m_locations, ref m_scales, PrimitiveType.Cube, (int)PARTS.LARM, Color.white, "LARM", new Vector3(0.3f, 0.3f, 0.4f), new Vector3(0f, 0f, 0.4f));
            //LFOREARM
            INIT_PART(ref go_parts, ref m_locations, ref m_scales, PrimitiveType.Cube, (int)PARTS.LFOREARM, Color.red, "LFOREARM", new Vector3(0.4f, 0.4f, 0.7f), new Vector3(0f, 0f, 0.55f));
            //LSHOULDER
            INIT_PART(ref go_parts, ref m_locations, ref m_scales, PrimitiveType.Cube, (int)PARTS.LHAND, Color.blue, "LHAND", new Vector3(0.35f, 0.4f, 0.25f), new Vector3(0f, 0f, 0.5f));

        }
        else if (_side == "RIGHT")
        {
            side = "RIGHT";
            //RSHOULDER
            INIT_PART(ref go_parts, ref m_locations, ref m_scales, PrimitiveType.Cube, (int)PARTS.RSHOULDER, Color.red, "RSHOULDER", new Vector3(0.45f, 0.35f, 0.4f), new Vector3(0f, 0.17f, -0.78f));
            //RARM
            INIT_PART(ref go_parts, ref m_locations, ref m_scales, PrimitiveType.Cube, (int)PARTS.RARM, Color.white, "RARM", new Vector3(0.3f, 0.3f, 0.4f), new Vector3(0f, 0f, -0.4f));
            //RFOREARM
            INIT_PART(ref go_parts, ref m_locations, ref m_scales, PrimitiveType.Cube, (int)PARTS.RFOREARM, Color.red, "RFOREARM", new Vector3(0.4f, 0.4f, 0.7f), new Vector3(0f, 0f, -0.55f));
            //RSHOULDER
            INIT_PART(ref go_parts, ref m_locations, ref m_scales, PrimitiveType.Cube, (int)PARTS.RHAND, Color.blue, "RHAND", new Vector3(0.35f, 0.4f, 0.25f), new Vector3(0f, 0f, -0.5f));
        }
    }

    public void Draw(ref Matrix4x4 chestMatrix, ref List<GameObject> go_parts, List<Matrix4x4> m_locations, List<Matrix4x4> m_scales, BACK_FORTH rX, Vector3[] v3_originals, BACK_FORTH rV)
    {
        Matrix4x4 accumT = Matrix4x4.identity;
        if (side == "LEFT")
        {
            for (int i = (int)PARTS.LSHOULDER; i <= (int)PARTS.LHAND; i++)
            {
                Matrix4x4 m = accumT * m_locations[i] * m_scales[i];
                if (i == (int)PARTS.LSHOULDER)
                {
                    accumT = chestMatrix;
                    m = accumT * m_locations[i] * m_scales[i];
                    Matrix4x4 r = Transforms.RotateX(90);
                    accumT *= m_locations[i] * r;
                }
                else if (i == (int)PARTS.LARM)
                {
                    Matrix4x4 t1 = Transforms.Translate(0f, 0f, .2f);
                    Matrix4x4 t2 = Transforms.Translate(0f, 0f, .2f);
                    Matrix4x4 r = new Matrix4x4();
                    if (rX.dir < 0)
                    {
                        r = Transforms.RotateY(rV.val * 2f);
                    }
                    else r = Transforms.RotateY(0);

                    m = accumT * t1 * r * t2 * m_scales[i];
                    accumT *= t1 * r * t2;
                }
                else if (i == (int)PARTS.LFOREARM)
                {
                    Matrix4x4 t1 = Transforms.Translate(0f, 0f, .2f);
                    Matrix4x4 t2 = Transforms.Translate(0f, 0f, .35f);
                    Matrix4x4 r = Transforms.RotateY(rX.val *.5f);
                    m = accumT * t1 * r * t2 * m_scales[i];
                    accumT *= t1 * r * t2;
                }
                else accumT *= m_locations[i];
                go_parts[i].GetComponent<MeshFilter>().mesh.vertices = Transforms.Transform(m, v3_originals);
            }
        }
        else if (side == "RIGHT")
        {
            for (int i = (int)PARTS.RSHOULDER; i <= (int)PARTS.RHAND; i++)
            {
                Matrix4x4 m = accumT * m_locations[i] * m_scales[i];

                if (i == (int)PARTS.RSHOULDER)
                {
                    accumT = chestMatrix;
                    m = accumT * m_locations[i] * m_scales[i];
                    Matrix4x4 r = Transforms.RotateX(-90);
                    accumT *= m_locations[i] * r;
                }
                else if (i == (int)PARTS.RARM)
                {
                    Matrix4x4 t1 = Transforms.Translate(0f, 0f, -.2f);
                    Matrix4x4 t2 = Transforms.Translate(0f, 0f, -.2f);
                    Matrix4x4 r = new Matrix4x4();
                    if (rX.dir < 0)
                    {
                        r = Transforms.RotateY(rV.val * 2f);
                    }
                    else r = Transforms.RotateY(0);

                    m = accumT * t1 * r * t2 * m_scales[i];
                    accumT *= t1 * r * t2;
                }
                else if (i == (int)PARTS.RFOREARM)
                {
                    Matrix4x4 t1 = Transforms.Translate(0f, 0f, -.2f);
                    Matrix4x4 t2 = Transforms.Translate(0f, 0f, -.35f);
                    Matrix4x4 r = Transforms.RotateY(rX.val * .5f);
                    m = accumT * t1 * r * t2 * m_scales[i];
                    accumT *= t1 * r * t2;
                }
                else
                {
                    accumT *= m_locations[i];
                }
                go_parts[i].GetComponent<MeshFilter>().mesh.vertices = Transforms.Transform(m, v3_originals);
            }
        }
    }
}
