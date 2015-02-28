using UnityEngine;
using UnityEngine.UI;

public class FPSDisplay : MonoBehaviour
{
    Text text;

    void Awake()
    {
        text = GetComponent<Text>();
    }

    void Update()
    {
        text.text = "<color=\"#f88\">" + (1f / Time.smoothDeltaTime).ToString("###.##") + "</color> fps";
    }
}
