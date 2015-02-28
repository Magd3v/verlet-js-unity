using UnityEngine;

[ExecuteInEditMode]
public class VerletConstraint : MonoBehaviour
{
    public Composite parentComp;
    public Constraint constraint;
    public VerletPoint p1;
    public VerletPoint p2;
    public bool Collide = true;
    public float stiffness = 0.5f;

    void OnEnable()
    {
        if (!Application.isPlaying && GetComponent<MagLineRenderer>() == null)
        {
            gameObject.AddComponent<MagLineRenderer>();
            GetComponent<MagLineRenderer>().p1 = p1.transform;
            GetComponent<MagLineRenderer>().p2 = p2.transform;
        }
    }

    void Update()
    {
        if (p1 == null || p2 == null)
        {
            parentComp.constraints.Remove(constraint);
            Destroy(gameObject);
        }
    }

    void OnDrawGizmos()
    {
        if(p1 != null && p2 != null && !Application.isPlaying)
            Gizmos.DrawLine(p1.transform.position, p2.transform.position);
    }
}
