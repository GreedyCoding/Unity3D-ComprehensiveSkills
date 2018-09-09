using System.IO;
using UnityEngine;

public class PersistantStorage : MonoBehaviour
{
    //Creating a string variable to store the path
    string savePath;

    private void Awake()
    {
        //Creating the path by using the combine method with the PersitantDataPath of unity and our desired filename
        savePath = Path.Combine(Application.persistentDataPath, "saveFile");
    }

    //Will be used to store the gamestate in a file at the in awake created path
    public void Save (PersistableObject persistableObject, int version)
    {
        //using used from catlikecoding instead of try and catch so there is always a writer available
        using (var writer = new BinaryWriter(File.Open(savePath, FileMode.Create)))
        {
            //write the input verion to the memory negated
            writer.Write(-version);
            //and save the input persiatable object using a new GameDataWriter with the "using" writer
            persistableObject.Save(new GameDataWriter(writer));
        }
    }

    public void Load (PersistableObject persistableObject)
    {
        //using used from catlikecoding instead of try and catch so there is always a reader available
        using (var reader = new BinaryReader(File.Open(savePath, FileMode.Open)))
        {
            //read the version from the memory and negate it
            int version = -reader.ReadInt32();
            //load the input persistable object with the "using" reader and the version we already got from the savefile
            persistableObject.Load(new GameDataReader(reader, version));
        }
    }

}
