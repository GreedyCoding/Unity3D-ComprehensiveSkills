using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSDisplay : MonoBehaviour
{
    //References to the Text Labels and the FPS Counter Class
    public Text hightestFpsLabel;
    public Text averageFpsLabel;
    public Text lowestFpsLabel;
    public FPSCounter fpsCounter;

    //Creating a Serializable struct for the FPS counter to map different colors according to a min fps value
    [System.Serializable]
    private struct FPSNumberColor
    {
        //Settable color
        public Color color;
        //Settable minimum fps to display the color
        public int minimumFPS;
    }

    //Creating a array for the FPS number color
    [SerializeField] private FPSNumberColor[] coloring;
	
	void Update ()
    {
        //Setting the text labes with the Display function every frame
        Display(hightestFpsLabel, fpsCounter.HighestFps);
        Display(averageFpsLabel, fpsCounter.AverageFps);
        Display(lowestFpsLabel, fpsCounter.LowestFps);
	}

    void Display (Text label, int fps)
    {
        //Setting the text of the label to the input fps converted to a string
        label.text = fps.ToString();
        //Looping through the coloring array
        for (int i = 0; i < coloring.Length; i++)
        {
            //And checking the current fps against the minimum fps of the coloring struct
            if (fps >= coloring[i].minimumFPS)
            {
                //if it is set the color of the label to the according color of the struct
                label.color = coloring[i].color;
                break;
            }
        }
    }
}
