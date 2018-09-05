using System.IO;
using UnityEngine;

public class GameDataWriter
{
    //Initializing an Binary Writer variable
    private BinaryWriter writer;

    //Contructor
    public GameDataWriter (BinaryWriter writer)
    {
        //Setting this writer to the passed in writer
        this.writer = writer;
    }

    //Writing a float with the binary writer
    public void Write(float value)
    {
        writer.Write(value);
    }

    //Writing an int with the binary writer
    public void Write(int value)
    {
        writer.Write(value);
    }

    //Writing 4 floats with the binary writer to store x,y,z,w values of the Quarternion
    //Need to get the values back in the same order in the GameDataReader
    public void Write(Quaternion value)
    {
        writer.Write(value.x);
        writer.Write(value.y);
        writer.Write(value.z);
        writer.Write(value.w);
    }

    //Writing 3 floats with the binary writer to store x,y,z values of the Vector3
    //Need to get the values back in the same order in the GameDataReader
    public void Write(Vector3 value)
    {
        writer.Write(value.x);
        writer.Write(value.y);
        writer.Write(value.z);
    }
}
