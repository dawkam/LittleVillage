using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParametersGiver : MonoBehaviour
{
    #region Villager

    [Header("Hunger")]
    [SerializeField] private float hungerMax;
    [SerializeField] private float hungerTick;
    [SerializeField] private float foodValue;

    [Header("Thirst")]
    [SerializeField] private float thirstMax;
    [SerializeField] private float thirstTick;
    [SerializeField] private float waterValue;

    [Header("Stamina")]
    [SerializeField] private float comfrotMin;
    [SerializeField] private float comfortTick;
    [SerializeField] private float staminaTick;
    [SerializeField] private float staminaDashTick;
    [SerializeField] private float restTime;


    [Header("Movement")]
    [SerializeField] private float moveSpeedMax;
    [SerializeField] private float dashPower;
    [SerializeField] private float turnSpeed;


    [Header("Collector")]
    [SerializeField] private int foodBagSize;

    [Header("Lumberjack")]
    [SerializeField] private int woodBagSize;
     private int woodSpawnCount;
    private int treeSpawnCount;

    [Header("Artisan")]
    [SerializeField] private float craftTime;

    [Header("Mayer")]
    [SerializeField] private float babyCoolDown;
    [SerializeField] private int babyCost;

    [Header("Baby")]
    [SerializeField] private float adulthoodAge;

    [Header("World")]
    [SerializeField] private float treeLifeMax;
    [SerializeField] private float treeLifeTick;
    [SerializeField] private int friutsMin;
    [SerializeField] private int friutsMax;
    [SerializeField] private int treesMin;
    [SerializeField] private int treesMax;
    
    [SerializeField] private int resurcesSpawnTime;

    [SerializeField] private int fruitsOnSceneMax;
    [SerializeField] private int treesOnSceneMax;

    [Header("System")]
    [SerializeField] private int collectDataTime;


    #region Existence
    public float HungerMax
    {
        get
        {
            if (hungerMax == 0)
                Debug.LogError("hungerMax is 0!!");
            return hungerMax;
        }
        private set => hungerMax = value;
    }
    public float HungerTick
    {
        get
        {
            if (hungerTick == 0)
                Debug.LogError("hungerTick is 0!!");
            return hungerTick;
        }
        private set => hungerTick = value;
    }
    public float FoodValue
    {
        get
        {
            if (foodValue == 0)
                Debug.LogError("foodValue is 0!!");
            return foodValue;
        }
        private set => foodValue = value;
    }

    public float ThirstMax
    {
        get
        {
            if (thirstMax == 0)
                Debug.LogError("thirstMax is 0!");
            return thirstMax;
        }
        private set => thirstMax = value;
    }
    public float ThirstTick
    {
        get
        {
            if (thirstTick == 0)
                Debug.LogError("thirstTick is 0!");
            return thirstTick;
        }
        private set => thirstTick = value;
    }
    public float WaterValue
    {
        get
        {
            if (waterValue == 0)
                Debug.LogError("waterValue is 0!!");
            return waterValue;
        }
        private set => waterValue = value;
    }

    public float ComfortMin
    {
        get
        {
            if (comfrotMin == 0)
                Debug.LogError("comfrotMin is 0!!");
            return comfrotMin;
        }
        private set => comfrotMin = value;
    }
    public float ComfortTick
    {
        get
        {
            if (comfortTick == 0)
                Debug.LogError("comfortTick is 0!!");
            return comfortTick;
        }
        private set => comfortTick = value;
    }
    public float StaminaTick
    {
        get
        {
            if (staminaTick == 0)
                Debug.LogError("staminaTick is 0!!");
            return staminaTick;
        }
        private set => staminaTick = value;
    }
    #endregion

    #region Movement
    public float MoveSpeedMax
    {
        get
        {
            if (moveSpeedMax == 0)
                Debug.LogError("moveSpeed is 0!!");
            return moveSpeedMax;
        }
        private set => moveSpeedMax = value;
    }

    public float DashPower
    {
        get
        {
            if (dashPower == 0)
                Debug.LogError("dashPower is 0!!");
            return dashPower;
        }
        private set => dashPower = value;
    }
    public float TurnSpeed
    {
        get
        {
            if (turnSpeed == 0)
                Debug.LogError("turnSpeed is 0!!");
            return turnSpeed;
        }
        private set => turnSpeed = value;
    }
    public float RestTime
    {
        get
        {
            if (restTime == 0)
                Debug.LogError("restTime is 0!!");
            return restTime;
        }
        private set => restTime = value;
    }

    public float StaminaDashTick
    {
        get
        {
            if (staminaDashTick == 0)
                Debug.LogError("staminaDashTick is 0!!");
            return staminaDashTick;
        }
        private set => staminaDashTick = value;
    }


    #endregion
    #endregion
    public int FoodBagSize
    {
        get
        {
            if (foodBagSize == 0)
                Debug.LogError("foodBagSize is 0!!");
            return foodBagSize;
        }
        private set => foodBagSize = value;

    }

    #region World

    public int WoodBagSize
    {
        get
        {
            if (woodBagSize == 0)
                Debug.LogError("woodBagSize is 0!!");
            return woodBagSize;
        }
        private set => woodBagSize = value;

    }
    public int WoodSpawnCount
    {
        get
        {
            if (woodSpawnCount == 0)
                Debug.LogError("woodSpawnCount is 0!!");
            return woodSpawnCount;
        }
        private set => woodSpawnCount = value;

    }
    public float TreeLifeMax
    {
        get
        {
            if (treeLifeMax == 0)
                Debug.LogError("treeLifeMax is 0!!");
            return treeLifeMax;
        }
        private set => treeLifeMax = value;

    }
    public float TreeLifeTick
    {
        get
        {
            if (treeLifeTick == 0)
                Debug.LogError("treeLifeTick is 0!!");
            return treeLifeTick;
        }
        private set => treeLifeTick = value;

    }

    public int FriutsMin
    {
        get
        {
            //if (friutsMinCount == 0)
            //    Debug.LogError("friutsMinCount is 0!!");
            return friutsMin;
        }
        private set => friutsMin = value;
    }
    public int FriutsMax
    {
        get
        {
            //if (friutsMaxCount == 0)
            //    Debug.LogError("friutsMaxCount is 0!!");
            return friutsMax;
        }
        private set => friutsMax = value;
    }
    public int TreeMin
    {
        get
        {
            //if (woodsMinCount == 0)
            //    Debug.LogError("woodsMinCount is 0!!");
            return treesMin;
        }
        private set => treesMin = value;
    }
    public int TreeMax
    {
        get
        {
            //if (woodsMaxCount == 0)
            //    Debug.LogError("woodsMaxCount is 0!!");
            return treesMax;
        }
        private set => treesMax = value;
    }
    #endregion

    public float CraftTime
    {
        get
        {
            if (craftTime == 0)
                Debug.LogError("craftTime is 0!!");
            return craftTime;
        }
        private set => craftTime = value;
    }    
    public float BabyCoolDown
    {
        get
        {
            if (babyCoolDown == 0)
                Debug.LogError("babyCoolDown is 0!!");
            return babyCoolDown;
        }
        private set => babyCoolDown = value;
    }
    public int BabyCost
    {
        get
        {
            if (babyCost == 0)
                Debug.LogError("babyCost is 0!!");
            return babyCost;
        }
        private set => babyCost = value;
    }

    public float AdulthoodAge
    {
        get
        {
            if (adulthoodAge == 0)
                Debug.LogError("adulthoodAge is 0!!");
            return adulthoodAge;
        }
        private set => adulthoodAge = value;
    }

    public int ResourcesSpawnTime {
        get
        {
            if (resurcesSpawnTime == 0)
                Debug.LogError("resurcesSpawnTime is 0!!");
            return resurcesSpawnTime;
        }
        private set => resurcesSpawnTime = value;
    }

    public int CollectDataTime
    {
        get
        {
            if (collectDataTime == 0)
                Debug.LogError("collectDataTime is 0!!");
            return collectDataTime;
        }
        private set => collectDataTime = value;
    }

    public int FruitsOnSceneMax 
    {
        get
        {
            if (fruitsOnSceneMax == 0)
                Debug.LogError("fruitsOnSceneMax is 0!!");
            return fruitsOnSceneMax;
        }
        private set => fruitsOnSceneMax = value;
    }
    public int TreesOnSceneMax 
    {
        get
        {
            if (treesOnSceneMax == 0)
                Debug.LogError("treesOnSceneMax is 0!!");
            return treesOnSceneMax;
        }
        private set => treesOnSceneMax = value;
    }

    public void ResetParametrs()
    {
        //FoodBagSize = Random.Range(3, 5);
        //ComfortMin = Random.Range(5, 15);
        WoodSpawnCount = Random.Range(1,3);
    }
}
