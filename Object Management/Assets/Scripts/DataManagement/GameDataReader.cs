using System.IO;
using UnityEngine;

public class GameDataReader
{
    public int VersionControl { get; private set; }

    //Initializing an Binary Reader variable
    private BinaryReader reader;

    //Contructor
    public GameDataReader(BinaryReader reader, int version)
    {
        //Setting this reader to the passed in reader
        this.reader = reader;
        this.VersionControl = version;
    }

    //Unity only uses SingleFloat so we read a single
    public float ReadFloat()
    {
        return reader.ReadSingle();
    }

    //Read an 32bit int
    public int ReadInt()
    {
        return reader.ReadInt32();
    }

    //Read the Quarterion by creating a variable for it and getting the x,y,z,w values back
    //in the order we stored them in the GameDataWriter and return that Quarternion
    public Quaternion ReadQuaternion()
    {
        Quaternion quaternion;
        quaternion.x = reader.ReadSingle();
        quaternion.y = reader.ReadSingle();
        quaternion.z = reader.ReadSingle();
        quaternion.w = reader.ReadSingle();
        return quaternion;
    }

    //Read the Vector3 by creating a variable for it and getting the x,y,z values back
    //in the order we stored them in the GameDataWriter and return that Vector3
    public Vector3 ReadVector3()
    {
        Vector3 vector;
        vector.x = reader.ReadSingle();
        vector.y = reader.ReadSingle();
        vector.z = reader.ReadSingle();
        return vector;
    }

    //Read the Color by creating a variable for it and getting the r,g,b,a values back
    //in the order we stored them in the GameDataWriter and return that Color
    public Color ReadColor()
    {
        Color color;
        color.r = reader.ReadSingle();
        color.g = reader.ReadSingle();
        color.b = reader.ReadSingle();
        color.a = reader.ReadSingle();
        return color;
    }
}
