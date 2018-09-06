using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSDisplay : MonoBehaviour
{
    public Text hightestFpsLabel;
    public Text averageFpsLabel;
    public Text lowestFpsLabel;
    public FPSCounter fpsCounter;

    [System.Serializable]
    private struct FPSColor
    {
        public Color color;
        public int minimumFps;
    }

    [SerializeField]
    private FPSColor[] coloring;
	
	void Update ()
    {
        Display(hightestFpsLabel, fpsCounter.highestFps);
        Display(averageFpsLabel, fpsCounter.averageFps);
        Display(lowestFpsLabel, fpsCounter.lowestFps);
	}

    void Display (Text label, int fps)
    {
        label.text = fps.ToString();
        for (int i = 0; i < coloring.Length; i++)
        {
            if (fps >= coloring[i].minimumFps)
            {
                label.color = coloring[i].color;
                break;
            }
        }
    }
}
