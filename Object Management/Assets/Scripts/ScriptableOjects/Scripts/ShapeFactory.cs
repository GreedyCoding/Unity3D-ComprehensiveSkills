using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu]
public class ShapeFactory : ScriptableObject
{
    [SerializeField] private Shape[] prefabs;
    [SerializeField] private Material[] materials;
    [SerializeField] private bool recyclingEnabled;

    private List<Shape>[] objectPools;

    //Creating a refernece to the poolscene we pool our objects to
    private Scene poolScene;

    public Shape GetShape (int shapeId, int materialId)
    {
        //Initializing a shape instance
        Shape instance;
        //Recycling can be disabled just in case of other use cases of the objectspawner
        if (recyclingEnabled)
        {
            //If recycling is enabled and there are no pools
            if (objectPools == null)
            {
                CreatePools();
            }
            //There are differnt pools for all the shapes so we chose the targetpool by passing in its shape id
            List<Shape> targetPool = objectPools[shapeId];
            //Getting last index
            int lastIndex = targetPool.Count - 1;
            //If there are shapes in the targetpool
            if (lastIndex >= 0)
            {
                //We can simply take the last shape out of the pool (they are basically all the same except color)
                instance = targetPool[lastIndex];
                //set the picked gameObject active
                instance.gameObject.SetActive(true);
                //and remove the last item from the list we just got the shape from
                targetPool.RemoveAt(lastIndex);
            }
            //If there are no shapes in the pool
            else
            {
                //A new shape needs to be instantiated
                instance = Instantiate(prefabs[shapeId]);
                //and the shapeId set to the passed in shapeId
                instance.ShapeId = shapeId;
                //Migrate our shapes to the pool scene after creating them
                SceneManager.MoveGameObjectToScene(instance.gameObject, poolScene);
            }
        }
        //If recycling is disabled
        else
        {
            //A new shape needs to be instantiated every time
            instance = Instantiate(prefabs[shapeId]);
            //and the shapeId set to the passed in shapeId
            instance.ShapeId = shapeId;
        }
        //Last we set the matrial on the chosen object
        //Setting the material with the setmaterial function from the shape class because the set is private
        instance.SetMaterial(materials[materialId], materialId);
        return instance;
    }

    public Shape GetRandomShape()
    {   
        //Return a Shape with a random prefab and a random material
        return GetShape(Random.Range(0, prefabs.Length), Random.Range(0, materials.Length));
    }

    public void ReclaimShape (Shape shapeToRecycle)
    {
        //If recycling is enabled we put the shape that should be destroyed back into the according pool
        if (recyclingEnabled)
        {
            //If there are no pools yet we create them beforehand
            if (objectPools == null)
            {
                CreatePools();
            }
            //Put the object into the pool according to the shapeId
            objectPools[shapeToRecycle.ShapeId].Add(shapeToRecycle);
            //and set the gameobject we just pooled to inactive so it will not be rendered
            shapeToRecycle.gameObject.SetActive(false);
        }
        else
        {
            //If recycling is disable simply destroy the gameobject
            Destroy(shapeToRecycle.gameObject);
        }
    }

    void CreatePools()
    {
        //Initialize the array of shape lists
        objectPools = new List<Shape>[prefabs.Length];
        for (int i = 0; i < objectPools.Length; i++)
        {
            //Create a new list of shapes for every spot
            objectPools[i] = new List<Shape>();
        }
        if (Application.isEditor)
        {
            //Need to check if the poolscene is there already before creating them or recompiling wont work in play mode
            poolScene = SceneManager.GetSceneByName("PoolScene");
            if (poolScene.isLoaded)
            {
                //Get all the gameobjects rooted to the poolscene and store them in the rootObjects array
                //doing this so we can resue the objects when we recompile in the editore
                GameObject[] rootObjects = poolScene.GetRootGameObjects();
                for (int i = 0; i < rootObjects.Length; i++)
                {
                    //Get the shape component from every rooted object to the poolscene
                    Shape pooledShape = rootObjects[i].GetComponent<Shape>();
                    //If the pooledshape is inactive
                    if (!pooledShape.gameObject.activeSelf)
                    {
                        //We add it to the corrisponding objectpool
                        objectPools[pooledShape.ShapeId].Add(pooledShape);
                    }
                }
                return;
            }
        }
        //Create a poolscene we can store our objects in
        poolScene = SceneManager.CreateScene("PoolScene");
    }
}
