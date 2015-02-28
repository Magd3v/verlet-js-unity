using System.Collections.Generic;
using UnityEngine;

public class VerletHandler : Singleton<VerletHandler>
{
    public Verlet World;
    public Dictionary<Particle, GameObject> vertObDict;
    public Dictionary<Composite, GameObject> compObDict;
    [SerializeField] float width = 8000;
    [SerializeField] float height = 3000;
    public Dictionary<Composite, int> compositeConstraintCounts; 

    void Awake()
    {
        vertObDict = new Dictionary<Particle, GameObject>();
        compObDict = new Dictionary<Composite, GameObject>();
        compositeConstraintCounts = new Dictionary<Composite, int>();
        World = new Verlet(width, height);
        World.friction = 0.99f;
        World.gravity.y = 1f;
    }

    public void CreateBody(Composite comp)
    {
        compositeConstraintCounts.Add(comp, 0);
        World.composites.Add(comp);
        compObDict.Add(comp, RebuildBody(comp));
    }

    public GameObject RebuildBody(Composite comp)
    {
        if (compObDict.ContainsKey(comp))
            Destroy(compObDict[comp]);

        foreach (Particle p in comp.particles)
        {
            vertObDict.Remove(p);
        }

        var bodyGameOb = new GameObject("body");

        foreach (Particle p in comp.particles)
        {
            var particleGameOb = new GameObject("particle");
            particleGameOb.transform.parent = bodyGameOb.transform;

            var verletPoint = particleGameOb.AddComponent<VerletPoint>();
            verletPoint.particle = p;
            verletPoint.parentComp = comp;

            vertObDict.Add(p, particleGameOb);
        }

        foreach (Constraint c in comp.constraints)
        {
            DistanceConstraint dc = c as DistanceConstraint;
            if (dc != null)
            {
                var constraintGameOb = new GameObject("constraint");
                constraintGameOb.transform.parent = bodyGameOb.transform;

                var verletConstraint = constraintGameOb.AddComponent<VerletConstraint>();
                verletConstraint.p1 = vertObDict[dc.a].GetComponent<VerletPoint>();
                verletConstraint.p2 = vertObDict[dc.b].GetComponent<VerletPoint>();

                var lineRenderer = constraintGameOb.AddComponent<MagLineRenderer>();
                lineRenderer.p1 = vertObDict[dc.a].transform;
                lineRenderer.p2 = vertObDict[dc.b].transform;

                compositeConstraintCounts[comp] += 1;
            }
        }

        var vBody = bodyGameOb.AddComponent<VerletBody>();
        vBody.body = comp;
        return bodyGameOb;
    }

    public void DestroyBody(Composite body)
    {
        foreach (Particle p in body.particles)
        {
            if (vertObDict.ContainsKey(p))
            {
                Destroy(vertObDict[p].transform.parent.gameObject);
                vertObDict.Remove(p);
            }
        }

        World.composites.Remove(body);
    }

    void Update()
    {
        World.Update(16);

        foreach (Composite composite in World.composites)
        {
            if (composite == null) continue;
            foreach (Particle p in composite.particles)
            {
                if (p == null) continue;
                if (!vertObDict.ContainsKey(p)) continue;
                vertObDict[p].transform.position = new Vector3(p.pos.x, -p.pos.y, 0);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(new Vector3(width / 2f, height / -2f, 0), new Vector3(width, height, 0f));

        if (World == null) return;
        foreach (Composite composite in World.composites)
        {
            foreach (Constraint c in composite.constraints)
            {
                if(!(c is DistanceConstraint)) continue;

                var dc = c as DistanceConstraint;
                Gizmos.color = new Color(0.5f, 0.8f, 0.8f);
                Gizmos.DrawLine(new Vector3(dc.a.pos.x, -dc.a.pos.y, 0),
                    new Vector3(dc.b.pos.x, -dc.b.pos.y, 0));
            }
        }
    }
}
