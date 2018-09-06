using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ShapeFactory : ScriptableObject
{
    [SerializeField] private Shape[] prefabs;
    [SerializeField] private Material[] materials;

    public Shape GetShape (int shapeId, int materialId)
    {
        Shape instance = Instantiate(prefabs[shapeId]);
        //Setting the shape id
        instance.ShapeId = shapeId;
        //Setting the material with the setmaterial function from the shape because the set is private
        instance.SetMaterial(materials[materialId], materialId);
        return instance;
    }

    public Shape GetRandomShape()
    {   
        //Return a Shape with a random prefab and a random material
        return GetShape(Random.Range(0, prefabs.Length), Random.Range(0, materials.Length));
    }
}
