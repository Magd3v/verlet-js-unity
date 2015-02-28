using System.Collections.Generic;
using UnityEngine;

public class VerletHandler : Singleton<VerletHandler>
{
    public Verlet World;
    public Dictionary<Particle, GameObject> partObDict;
    public Dictionary<Composite, GameObject> compObDict;
    [SerializeField] float width = 8000;
    [SerializeField] float height = 3000;
    public Dictionary<Composite, int> compositeConstraintCounts; 

    void Awake()
    {
        partObDict = new Dictionary<Particle, GameObject>();
        compObDict = new Dictionary<Composite, GameObject>();
        compositeConstraintCounts = new Dictionary<Composite, int>();
        World = new Verlet(width, height);
        World.friction = 0.99f;
        World.gravity.y = 1f;
    }

    public void CreateComposite(Composite comp)
    {
        compositeConstraintCounts.Add(comp, 0);
        World.composites.Add(comp);
        compObDict.Add(comp, RebuildCompositeModel(comp));
    }

    public GameObject RebuildCompositeModel(Composite comp)
    {
        if (compObDict.ContainsKey(comp))
            Destroy(compObDict[comp]);

        foreach (Particle p in comp.particles)
        {
            partObDict.Remove(p);
        }

        var bodyGameOb = new GameObject("comp");

        foreach (Particle p in comp.particles)
        {
            var particleGameOb = new GameObject("particle");
            particleGameOb.transform.parent = bodyGameOb.transform;

            var verletPoint = particleGameOb.AddComponent<VerletPoint>();
            verletPoint.particle = p;
            verletPoint.parentComp = comp;

            partObDict.Add(p, particleGameOb);
        }

        foreach (Constraint c in comp.constraints)
        {
            DistanceConstraint dc = c as DistanceConstraint;
            if (dc != null)
            {
                var constraintGameOb = new GameObject("constraint");
                constraintGameOb.transform.parent = bodyGameOb.transform;

                var verletConstraint = constraintGameOb.AddComponent<VerletConstraint>();
                verletConstraint.p1 = partObDict[dc.a].GetComponent<VerletPoint>();
                verletConstraint.p2 = partObDict[dc.b].GetComponent<VerletPoint>();
                verletConstraint.parentComp = comp;
                verletConstraint.constraint = c;

                var lineRenderer = constraintGameOb.AddComponent<MagLineRenderer>();
                lineRenderer.p1 = partObDict[dc.a].transform;
                lineRenderer.p2 = partObDict[dc.b].transform;

                compositeConstraintCounts[comp] += 1;
            }
        }

        var vBody = bodyGameOb.AddComponent<VerletBody>();
        vBody.body = comp;
        return bodyGameOb;
    }

    public void DestroyComposite(Composite comp)
    {
        foreach (Particle p in comp.particles)
        {
            if (partObDict.ContainsKey(p))
            {
                Destroy(partObDict[p].transform.parent.gameObject);
                partObDict.Remove(p);
            }
        }

        World.composites.Remove(comp);
    }

    void Update()
    {
        World.Update(16);
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
