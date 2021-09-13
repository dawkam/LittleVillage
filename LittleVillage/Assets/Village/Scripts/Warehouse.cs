using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warehouse : MonoBehaviour
{
    [SerializeField] public int FoodCount { get; private set; }
    [SerializeField] public int WoodCount { get; private set; }

    ParametersGiver parametersGiver;
    private void Start()
    {
        parametersGiver = FindObjectOfType<ParametersGiver>();
        ResetData();
    }

    public void AddFood(int count)
    {
        FoodCount += count;
    }

    public bool TakeFood()
    {
        if (FoodCount > 0)
        {
            FoodCount--;
            return true;
        }
        else
            return false;
    }


    public bool TakeFoodForBaby()
    {
        if (FoodCount > parametersGiver.BabyCost)
        {
            FoodCount-= parametersGiver.BabyCost;
            return true;
        }
        else
            return false;
    }

    public void AddWood(int count)
    {
        WoodCount += count;
    }

    public bool TakeWood()
    {
        if (WoodCount > 0)
        {
            WoodCount--;
            return true;
        }
        else
            return false;
    }

    internal void ResetData()
    {
        FoodCount = Random.Range(3, 5);
        WoodCount = 0;// Random.Range(5, 8);
    }
}
