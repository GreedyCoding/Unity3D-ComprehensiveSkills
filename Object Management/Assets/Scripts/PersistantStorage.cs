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
        //using used from catlikecoding instead of try and catch
        using (var writer = new BinaryWriter(File.Open(savePath, FileMode.Create)))
        {
            writer.Write(-version);
            persistableObject.Save(new GameDataWriter(writer));
        }
    }

    public void Load (PersistableObject persistableObject)
    {
        using (var reader = new BinaryReader(File.Open(savePath, FileMode.Open)))
        {
            int version = -reader.ReadInt32();
            persistableObject.Load(new GameDataReader(reader, version));
        }
    }

}
