using System.Reflection;
using UnityEngine;

[ExecuteInEditMode]
public class MagLineRenderer : MonoBehaviour
{
    [SerializeField, HideInInspector] Mesh lineMesh;
    [SerializeField, HideInInspector] Material lineMat;
    [SerializeField, HideInInspector] MeshRenderer mRend;
    [SerializeField, HideInInspector] MeshFilter mFilt;
    [SerializeField] public Transform p1;
    [SerializeField] public Transform p2;

    void OnEnable()
    {
        if(lineMesh == null)
            lineMesh = Resources.Load<Mesh>("Line");

        if (lineMat == null)
            lineMat = Resources.Load<Material>("FlatWhite");

        var mFilt = GetComponent<MeshFilter>();
        if (mFilt == null)
        {
            mFilt = gameObject.AddComponent<MeshFilter>();
            mFilt.mesh = lineMesh;
        }

        var rend = GetComponent<MeshRenderer>();
        if (rend == null)
        {
            mRend = gameObject.AddComponent<MeshRenderer>();
            mRend.material = lineMat;
        }
    }

    void Start()
    {
    }

    void OnRenderObject() 
    {
        /*
        if (lineMesh != null && lineMesh.vertices.Length > 1 && p1 != null && p2 != null)
        {
            Vector3[] newVerts = new Vector3[lineMeshInstance.vertices.Length];
            Quaternion inverseRotation = Quaternion.Inverse(transform.rotation);

            newVerts[0] = inverseRotation * (p1.transform.position - transform.position);
            newVerts[0] = newVerts[0].Divide(transform.lossyScale);

            newVerts[1] = inverseRotation * (p2.transform.position - transform.position);
            newVerts[1] = newVerts[1].Divide(transform.lossyScale);

            lineMeshInstance.vertices = newVerts;
            lineMeshInstance.RecalculateBounds();
        }*/

        if (p1 == null || p2 == null) return;
        float length = (p1.transform.position - p2.transform.position).magnitude;
        transform.position = p1.transform.position;
        transform.localScale = new Vector3(length, 1, 1);
        transform.eulerAngles = new Vector3(0, 0,
            Mathf.Atan2(p2.transform.position.y - p1.transform.position.y, p2.transform.position.x - p1.transform.position.x) * Mathf.Rad2Deg);
    }
}
