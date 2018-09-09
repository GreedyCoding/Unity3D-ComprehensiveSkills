using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : PersistableObject
{
    //Used for the GPU instancing of the materials
    static MaterialPropertyBlock sharedPropertyBlock;

    private Color color;

    private MeshRenderer meshRenderer;

    //The shapeId should not be 0 from the start because it would be a valid indentifier for the shapes
    //and we will check if the value has already changed by checking against minValue again
    private int shapeId = int.MinValue;
    
    public int MaterialId { get; private set; }

    public int ShapeId
    {
        get
        {
            return shapeId;
        }
        set
        {
            //Only letting the value be set if it is unchanged and we dont input the int.minValue again
            if (shapeId == int.MinValue && value != int.MinValue)
            {
                shapeId = value;
            }
        }
    }

    private void Awake()
    {
        //Getting the meshrenderer component from the shape
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public override void Save(GameDataWriter writer)
    {
        //Overriding tha base save function
        base.Save(writer);
        //And also writing color now
        writer.Write(color);
    }

    public override void Load(GameDataReader reader)
    {
        //Overriding tha base load function
        base.Load(reader);
        //If we have a bigger version then 0 we can read the color otherwise we set the color to white
        SetColor(reader.VersionControl > 0 ? reader.ReadColor() : Color.white);
    }

    public void SetMaterial(Material material, int materialId)
    {
        //Setting material of meshrenderer to the passed in material and setting the MaterialID
        meshRenderer.material = material;
        MaterialId = materialId;
    }

    public void SetColor(Color color)
    {
        //Setting this color to the passed in color
        this.color = color;
        //Checking for the compnent to do the GPU instancing of the colors
        if (sharedPropertyBlock == null)
        {
            //if there is none we create it
            sharedPropertyBlock = new MaterialPropertyBlock();

        }
        //Set the color of the sharedPropertyBlock to the passed in color
        sharedPropertyBlock.SetColor("_Color", color);
        //And set this property block to the meshrenderer
        meshRenderer.SetPropertyBlock(sharedPropertyBlock);
    }
    
}
