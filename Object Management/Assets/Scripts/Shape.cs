using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : PersistableObject
{
    static MaterialPropertyBlock sharedPropertyBlock;

    private Color color;

    private MeshRenderer meshRenderer;

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
            if (shapeId == int.MinValue && value != int.MinValue)
            {
                shapeId = value;
            }
        }
    }

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }
    public override void Save(GameDataWriter writer)
    {
        base.Save(writer);
        writer.Write(color);
    }

    public override void Load(GameDataReader reader)
    {
        base.Load(reader);
        SetColor(reader.VersionControl > 0 ? reader.ReadColor() : Color.white);
    }

    public void SetMaterial(Material material, int materialId)
    {
        meshRenderer.material = material;
        MaterialId = materialId;
    }

    public void SetColor(Color color)
    {
        this.color = color;
        if (sharedPropertyBlock == null)
        {
            sharedPropertyBlock = new MaterialPropertyBlock();

        }
        sharedPropertyBlock.SetColor("_Color", color);
        meshRenderer.SetPropertyBlock(sharedPropertyBlock);
    }
    
}
