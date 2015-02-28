using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InfoBox : MonoBehaviour
{
    Text text;

    void Awake()
    {
        text = GetComponent<Text>();
    }

    void Update()
    {
        text.text = "<color=\"#88f\">" + VerletHandler.Instance.World.composites.Count + "</color>\tcomposites\n";
        text.text += "<color=\"#88f\">" + VerletHandler.Instance.World.composites.Sum(c => c.particles.Count) + "</color>\tparticles\n";
        text.text += "<color=\"#88f\">" + VerletHandler.Instance.World.composites.Sum(c => c.constraints.Count) + "</color>\tconstraints";
    }
}
