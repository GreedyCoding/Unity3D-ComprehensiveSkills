using UnityEngine;

//Having multiple components on one object could lead to trouble in saving and loading
[DisallowMultipleComponent]
public class PersistableObject : MonoBehaviour
{
    //Method used to save objects
    public virtual void Save (GameDataWriter writer)
    {
        //Write the localpositon as Vector 3
        writer.Write(transform.localPosition);
        //Write the localrotation as Quarternion
        writer.Write(transform.localRotation);
        //Write the localscale as Vector3
        writer.Write(transform.localScale);
    }

    //When loading the object we load them in the same order we saved them
    public virtual void Load (GameDataReader reader)
    {
        transform.localPosition = reader.ReadVector3();
        transform.localRotation = reader.ReadQuaternion();
        transform.localScale = reader.ReadVector3();
    }
}
