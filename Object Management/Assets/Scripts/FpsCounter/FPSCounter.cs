using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    //We store this many frames in our Fps Buffer
    [SerializeField] private int frameRange = 60;
    //Used to store the current buffer index
    int fpsBufferIndex;
    //Int array to store our last fps values to calculate averages
    int[] fpsBuffer;

    public int HighestFps { get; private set; }
    public int AverageFps { get; private set; }
    public int LowestFps { get; private set; }

    private void Update()
    {
        //If there is no fpsBuffer already or the length of it does not match the framerange initialize it
        if (fpsBuffer == null || fpsBuffer.Length != frameRange)
        {
            InitializeBuffer();
        }
        UpdateBuffer();
        CalculateFPS();
    }

    void InitializeBuffer()
    {
        //We need a framerange of at least 1 so if it is lower or 0 set it to 1
        if (frameRange <= 0)
        {
            frameRange = 1;
        }
        fpsBuffer = new int[frameRange];
        fpsBufferIndex = 0;
    }

    void UpdateBuffer()
    {
        //Store the current fps in the current fpsBuffer[index] and increase the index by 1
        fpsBuffer[fpsBufferIndex++] = (int)(1f / Time.unscaledDeltaTime);
        //If our index gets bigger then the framerange 
        if (fpsBufferIndex >= frameRange)
        {
            //we set the index back to 0 because it does not matter which values of the buffer we overwrite
            fpsBufferIndex = 0;
        }
    }

    void CalculateFPS()
    {
        //Setting up a sum variable to get the average fps
        int sum = 0;
        //Setting highest to 0 because every fps count should be above 0 so we can check against that
        int highest = 0;
        //Setting lowest to max value though because if we set it to 0 the lowest calulation would not work out
        int lowest = int.MaxValue;
        //Loopping through all the fps values in the buffer
        for (int i = 0; i < frameRange; i++)
        {
            int fps = fpsBuffer[i];
            //Adding every fps value to the sum
            sum += fps;
            //If current fps value is bigger then highest we set this fps value to be the highest
            if (fps > highest)
            {
                highest = fps;
            }
            //If current fps value is lower then lowest we set this fps value to be the lowest
            if (fps < lowest)
            {
                lowest = fps;
            }
        }
        //setting the classes gettable ints
        HighestFps = highest;
        AverageFps = sum / frameRange;
        LowestFps = lowest;
    }
}
