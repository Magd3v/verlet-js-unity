using UnityEngine;

public class ColorFlash : MonoBehaviour
{
    [SerializeField] float interval = 0.05f;
    float time;

    void Update()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
        }
        else
        {
            renderer.material.SetColor("_Color",
                new Color(Random.Range(0f, 0.5f), Random.Range(0f, 0.5f), Random.Range(0.5f, 1f)));
            time = interval;
        }
    }
}
