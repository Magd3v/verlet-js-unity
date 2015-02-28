using System.Collections.Generic;
using UnityEngine;

public class VerletBody : MonoBehaviour
{
    public Composite body;
    [SerializeField] bool kinematic;

    void Start()
    {
        if (body == null) BuildComposite();
        UpdatePos();
    }

    public void BuildComposite()
    {
        Debug.Log("Built composite");

        body = new Composite();
        VerletHandler.Instance.compObDict.Add(body, gameObject);
        VerletHandler.Instance.compositeConstraintCounts.Add(body, 0);

        VerletPoint[] childPoints = GetComponentsInChildren<VerletPoint>();

        //Create verts
        List<Particle> verts = new List<Particle>();
        foreach (VerletPoint p in childPoints)
        {
            var vert = new Particle(new Vector2(p.transform.position.x, -p.transform.position.y));
            if (p.Anchored) body.constraints.Add(new PinConstraint(vert));
            p.parentComp = body;

            VerletHandler.Instance.vertObDict.Add(vert, p.gameObject);
            p.particle = vert;
            verts.Add(vert);
            body.particles.Add(vert);
        }

        VerletConstraint[] childEdges = GetComponentsInChildren<VerletConstraint>();

        //Link em up
        foreach (VerletConstraint e in childEdges)
        {
            body.constraints.Add(new DistanceConstraint(e.p1.particle, e.p2.particle, e.stiffness));
            VerletHandler.Instance.compositeConstraintCounts[body] += 1;
        }

        VerletHandler.Instance.World.composites.Add(body);
    }

    void UpdatePos()
    {
        transform.localScale = Vector3.one;

        Vector3 totalAverage = Vector3.zero;

        foreach (Particle p in body.particles)
        {
            totalAverage += new Vector3(p.pos.x, -p.pos.y, 0);
        }

        totalAverage = new Vector2(totalAverage.x / body.particles.Count, totalAverage.y / body.particles.Count);
        transform.position = new Vector3(totalAverage.x, totalAverage.y, 0);
    }

    void OnRenderObject()
    {
        UpdatePos();
    }
}
