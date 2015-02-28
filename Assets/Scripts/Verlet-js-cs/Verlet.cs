using System;
using System.Collections.Generic;
using UnityEngine;

public class Particle
{
    public Vector2 pos = Vector2.zero;
    public Vector2 lastPos = Vector2.zero;

    public Particle(Vector2 pos)
    {
        this.pos = pos;
        this.lastPos = pos;
    }
}

public class Verlet
{
    float width;
    float height;

    public Vector2 gravity = new Vector2(0, 0.2f);
    public float friction = 0.99f;
    public float groundFriction = 0.8f;

    public List<Composite> composites;

    public Verlet(float width, float height)
    {
        composites = new List<Composite>();
        this.width = width;
        this.height = height;
    }

    public void Update(float step)
    {
        //Move
        foreach (Composite c in composites)
        {
            foreach (Particle p in c.particles)
            {
                Vector2 vel = (p.pos - p.lastPos) * friction;
                p.lastPos = p.pos;
                p.pos += gravity;
                p.pos += vel;
            }
        }

        //Handle constraints
        float stepCoef = 1f / step;
        foreach (Composite c in composites)
        {
            for (int i = 0; i < step; ++i)
            {
                foreach (Constraint co in c.constraints)
                {
                    co.Relax(stepCoef);
                }
            }
        }

        //Keep in bounds
        foreach (Composite c in composites)
        {
            foreach (Particle p in c.particles)
            {
                LimitToBounds(p);
            }
        }
    }

    public void LimitToBounds(Particle p)
    {
        p.pos = new Vector2(Mathf.Clamp(p.pos.x, 0, width), Mathf.Clamp(p.pos.y, 0, height));
    }
}

public class Composite
{
    public List<Particle> particles;
    public List<Constraint> constraints;
    public Action<Particle> OnParticleAdded;

    public Composite()
    {
        particles = new List<Particle>();
        constraints = new List<Constraint>();
    }

    public Constraint Pin(Particle particle, Vector2 pos)
    {
        var pc = new PinConstraint(particle, pos);
        constraints.Add(pc);
        return pc;
    }

    public static Composite operator +(Composite c1, Composite c2)
    {
        Composite combined = new Composite();
        combined.particles.AddRange(c1.particles);
        combined.particles.AddRange(c2.particles);
        combined.constraints.AddRange(c1.constraints);
        combined.constraints.AddRange(c2.constraints);
        return combined;
    }
}