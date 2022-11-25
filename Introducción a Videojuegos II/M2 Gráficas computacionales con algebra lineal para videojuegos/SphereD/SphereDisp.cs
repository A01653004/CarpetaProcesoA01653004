using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Jonatan Hernández García A01653004
public class SphereDisp : MonoBehaviour
{
    public Renderer renderer;
    public Texture2D background;
    public int numberSpheres = 20;

    Vector3 Ia;
    Vector3 Id;
    Vector3 Is;
    Vector3 n;
    Vector3 LIGHT;
    Vector3 CAMERA;
    float ALPHA;

    Camera mainCam;
    Vector2 CameraResolution;

    float frusttumHeight;
    float frustumWidth;

    float pixelWidth;
    float pixelHeight;

    Texture2D texture;

    Vector3 topLeft;

    struct Sphere
    {
        public float kdr;
        public float kdg;
        public float kdb;

        public Vector3 kd;
        public Vector3 ka;
        public Vector3 ks;

        public Vector3 SC;
        public float SR;
    };

    List<Sphere> spheres;
    void Start()
    {
        spheres = new List<Sphere>();

        //Sets plane
        renderer.transform.localRotation = Quaternion.Euler(90, 0, 0);
        renderer.transform.localPosition = new Vector3(0, 5, -2);

        //Sets camera
        mainCam = Camera.main;
        mainCam.clearFlags = CameraClearFlags.SolidColor;
        mainCam.backgroundColor = new Color(0, 0, 0);
        mainCam.fieldOfView = 65;
        mainCam.nearClipPlane = 1;
        mainCam.farClipPlane = 20;
        CAMERA = new Vector3(0f, 4f, 5.5f);
        mainCam.transform.rotation = new Quaternion(0, 0, 0, 0);
        mainCam.transform.position = CAMERA;
        mainCam.transform.localScale = new Vector3(0, 0, 0);

        CameraResolution = new Vector2(640, 480);

        //Sets light variable
        LIGHT = new Vector3(0f, 7.5f, 3f);
        Ia = new Vector3(0.7f, 0.7f, 0.7f);
        Id = new Vector3(0.8f, 0.8f, 1f);
        Is = new Vector3(1f, 1f, 1f);

        // Create pointLight
        GameObject pointLight = new GameObject("ThePointLight");
        Light lightComp = pointLight.AddComponent<Light>();
        pointLight.transform.position = LIGHT;
        lightComp.type = LightType.Point;
        lightComp.color = new Color(Id.x, Id.y, Id.z);
        lightComp.intensity = 4;

        //Sets Frustrum size 
        frusttumHeight = 2.0f * mainCam.nearClipPlane * Mathf.Tan(mainCam.fieldOfView * 0.5f * Mathf.Deg2Rad);
        frustumWidth = frusttumHeight * mainCam.aspect;
        //Sets pixels size
        pixelWidth = frustumWidth / (float)CameraResolution.x;
        pixelHeight = frusttumHeight / (float)CameraResolution.y;
        //Get the pixel at the top left corner from the frustrum
        topLeft = FindTopLeftFrusrtumNear();
        //Creates and sorts the spheres depending on its z position
        setSpheres(numberSpheres);
        spheres.Sort((s1, s2) => s1.SC.z.CompareTo(s2.SC.z));
        //Creates the texture
        texture = new Texture2D(Mathf.RoundToInt(CameraResolution.x), Mathf.RoundToInt(CameraResolution.y), TextureFormat.ARGB32, false);

        for (int y = 0; y < CameraResolution.y; y++)
        {
            for (int x = 0; x < CameraResolution.x; x++)
            {
                Color bg = background.GetPixel(x, y);
                texture.SetPixel(x, y, bg);
            }
        }
        texture.Apply();

        for (int s = spheres.Count - 1; s >= 0; s--)
        {
            for (int y = 0; y < CameraResolution.y; y++)
            {
                for (int x = 0; x < CameraResolution.x; x++)
                {
                    Color color = GetPixel(new Vector3(x, y, 0f), spheres[s]);
                    if (color != Color.clear) texture.SetPixel(x, -y, color);
                }
            }
        }
        texture.Apply();

        texture.filterMode = FilterMode.Point;
        Renderer rend2 = renderer.GetComponent<Renderer>();
        Shader shader = Shader.Find("Unlit/Texture");
        rend2.material.shader = shader;
        rend2.material.mainTexture = texture;
        SaveRender(texture);
    }

    //Gets the coordinates of a specific pixel center 
    Vector3 Cast(Vector3 coords)
    {
        Vector3 center = topLeft;

        center += (pixelWidth / 2f) * mainCam.transform.right;
        center -= (pixelHeight / 2f) * mainCam.transform.up;
        center += (pixelWidth) * mainCam.transform.right * coords.x;
        center -= (pixelHeight) * mainCam.transform.up * coords.y;

        return center;
    }

    //Checks whenever there is a sphere in a specific pixel and gets its color
    Color GetPixel(Vector3 coords, Sphere sphere)
    {
        Vector3 center = Cast(coords);
        Vector3 u = (center - CAMERA);

        u = u.normalized;
        Vector3 oc = CAMERA - sphere.SC;
        float nabla = (Vector3.Dot(u, oc) * Vector3.Dot(u, oc)) - ((oc.magnitude * oc.magnitude) - (sphere.SR * sphere.SR));

        if (nabla < 0)
        {
            return Color.clear;
        }

        float dpos = -1 * Vector3.Dot(u, oc) + Mathf.Sqrt(nabla);
        float dneg = -1 * Vector3.Dot(u, oc) - Mathf.Sqrt(nabla);

        Vector3 color = new Vector3();

        if (Mathf.Abs(dpos) < Mathf.Abs(dneg))
        {
            color = Illumination(CAMERA + dpos * u, sphere);
        }
        else
        {
            color = Illumination(CAMERA + dneg * u, sphere);
        }

        return new Color(color.x, color.y, color.z);
    }

    //Gets a vector with the rgb attributes of a specific point
    Vector3 Illumination(Vector3 PoI2, Sphere sphere)
    {
        Vector3 A = new Vector3(sphere.ka.x * Ia.x, sphere.ka.y * Ia.y, sphere.ka.z * Ia.z);
        Vector3 D = new Vector3(sphere.kd.x * Id.x, sphere.kd.y * Id.y, sphere.kd.z * Id.z);
        Vector3 S = new Vector3(sphere.ks.x * Is.x, sphere.ks.y * Is.y, sphere.ks.z * Is.z);

        Vector3 l = LIGHT - PoI2;
        Vector3 v = CAMERA - PoI2;

        n = PoI2 - sphere.SC;

        float dotNuLu = Vector3.Dot(n.normalized, l.normalized);
        float dotNuL = Vector3.Dot(n.normalized, l);

        Vector3 lp = n.normalized * dotNuL;
        Vector3 lo = l - lp;
        Vector3 r = lp - lo;
        D *= dotNuLu;
        float w = Mathf.Pow(Vector3.Dot(v.normalized, r.normalized), ALPHA);
        if (w is float.NaN) w = 0;
        S *= w;
        return A + D + S;
    }

    //Gets the pixel at the top left corner of the camera frustrum
    Vector3 FindTopLeftFrusrtumNear()
    {
        Vector3 o = CAMERA;
        Vector3 p = o + mainCam.transform.forward * mainCam.nearClipPlane;
        p += mainCam.transform.up * frusttumHeight / 2.0f;
        p -= mainCam.transform.right * frustumWidth / 2.0f;

        return p;
    }
    void setSpheres(int numSpheres)
    {
        for (int i = 0; i < numSpheres; i++)
        {
            Sphere sphere = new Sphere();
            sphere.kdr = Random.Range(0.5f, 1f);
            sphere.kdg = Random.Range(0.5f, 1f);
            sphere.kdb = Random.Range(0.5f, 1f);

            sphere.kd = new Vector3(sphere.kdr, sphere.kdg, sphere.kdb);
            sphere.ka = new Vector3(sphere.kdr / 5.0f, sphere.kdg / 5.0f, sphere.kdb / 5.0f);
            sphere.ks = new Vector3(sphere.kdr / 3.0f, sphere.kdg / 3.0f, sphere.kdb / 3.0f);

            float alpha = Random.Range(500f, 600f);

            sphere.SR = Random.Range(0.1f, 0.35f);
            sphere.SC = new Vector3(Random.Range(-2.0f, 2.0f), Random.Range(2f, 6.0f), Random.Range(8.0f, 10.0f));

            // Create sphere
            GameObject sph = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sph.transform.position = sphere.SC;
            sph.transform.localScale = new Vector3(sphere.SR * 2f, sphere.SR * 2f, sphere.SR * 2f);
            
            Renderer rend = sph.GetComponent<Renderer>();
            rend.material.shader = Shader.Find("Specular");
            rend.material.SetColor("_Color", new Color(sphere.kd.x, sphere.kd.y, sphere.kd.z, alpha));
            rend.material.SetColor("_SpecColor", new Color(sphere.ks.x, sphere.ks.y, sphere.ks.z));

            spheres.Add(sphere);
        }
    }
    void SaveRender(Texture2D texture)
    {
        byte[] bytes = texture.EncodeToPNG();
        var dirPath = Application.dataPath + "/Resources/";
        System.IO.File.WriteAllBytes(dirPath + "renderImg" + ".png", bytes);
    }
}
