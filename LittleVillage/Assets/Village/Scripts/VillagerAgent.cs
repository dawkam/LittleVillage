using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class VillagerAgent : Agent, IObserver
{
    [SerializeField] protected bool isTraining = true;

    #region Currrent data
    protected float hungerCurrent;
    protected float thirstCurrent;
    protected float staminaCurrent;
    protected float restTimeCurrent;

    protected float comfort;

    protected float moveSpeedCurrent;

    protected bool isAlive;
    protected bool isControlEnabled;
    #endregion

    protected ParametersGiver parametersGiver;
    new protected Rigidbody rigidbody;
    protected VillageArea villageArea;
    protected Village village;

    protected readonly int rewardModifier = 1;
    protected float moveSpeedMax;

    public DeathReson deathReson;

    public float HungerCurrent { get => hungerCurrent; protected set => hungerCurrent = value; }
    public float ThirstCurrent { get => thirstCurrent; protected set => thirstCurrent = value; }
    public float StaminaCurrent { get => staminaCurrent; protected set => staminaCurrent = value; }
    public float RestTimeCurrent { get => restTimeCurrent; protected set => restTimeCurrent = value; }

    public override void Initialize()
    {
        base.Initialize();
        InitializeData();

        if (parametersGiver == null || village.warehouse == null || village.well == null)
            Debug.LogError("Parameters giver or well or warehouse is missing!!");
    }

    protected virtual void InitializeData()
    {
        rigidbody = GetComponent<Rigidbody>();
        villageArea = this.transform.parent.GetComponent<VillageArea>();
        parametersGiver = FindObjectOfType<ParametersGiver>();
        village = villageArea.GetComponentInChildren<Village>();
        moveSpeedMax = parametersGiver.MoveSpeedMax;
        deathReson = DeathReson.None;
    }

    public override void OnEpisodeBegin()
    {
        ResetData();
        StartCoroutine(ExistancePunishment());
        StartCoroutine(StaminaRegeration());
    }
    protected virtual void ResetData()
    {
        parametersGiver.ResetParametrs();
        HungerCurrent = parametersGiver.HungerMax;
        ThirstCurrent = parametersGiver.ThirstMax;

        StaminaCurrent = parametersGiver.ComfortMin;
        moveSpeedMax = parametersGiver.MoveSpeedMax;
        moveSpeedCurrent = moveSpeedMax;

        isControlEnabled = true;
        isAlive = true;
        if (isTraining)
        {
            village.ResetData();
            villageArea.ResetArea();
        }
        village.Attach(this);
    }

    /// <summary>
    /// Perform actions based on a vector of numbers
    /// </summary>
    /// <param name="vectorAction">The list of actions to take</param>
    public override void OnActionReceived(ActionBuffers actions)
    {

        // Convert the first action to forward movement
        float forwardAmount = actions.DiscreteActions[0];

        // Convert the second action to turning left or right
        float turnAmount = 0f;
        if (actions.DiscreteActions[1] == 1f)
        {
            turnAmount = -1f;
        }
        else if (actions.DiscreteActions[1] == 2f)
        {
            turnAmount = 1f;
        }

        bool isDash = actions.DiscreteActions[2] == 1f;


        ApplyMovement(forwardAmount, turnAmount, isDash);


        //if (actions.DiscreteActions[2] == 1)
        //    Eat();


        // Apply a tiny negative reward every step to encourage action
        //if (MaxStep > 0) AddReward(-1f / MaxStep);
    }

    protected void ApplyMovement(float forwardAmount, float turnAmount, bool isDash)
    {
        if (isControlEnabled)
        {
            if (isDash)
            {
                Dash();
                forwardAmount = 1;
            }
            Move(forwardAmount, turnAmount);
        }
    }

    /// <summary>
    /// Read inputs from the keyboard and convert them to a list of actions.
    /// This is called only when the player wants to control the agent and has set
    /// Behavior Type to "Heuristic Only" in the Behavior Parameters inspector.
    /// </summary>
    /// <returns>A vectorAction array of floats that will be passed into <see cref="AgentAction(float[])"/></returns>
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        Array.Clear(actionsOut.DiscreteActions.Array, 0, actionsOut.DiscreteActions.Length);
        if (Input.GetKey(KeyCode.W))
        {
            // move forward
            actionsOut.DiscreteActions.Array[0] = 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            // turn left
            actionsOut.DiscreteActions.Array[1] = 1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            // turn right
            actionsOut.DiscreteActions.Array[1] = 2;
        }
        if (Input.GetKey(KeyCode.X))
        {
            //dash
            actionsOut.DiscreteActions.Array[2] = 1;
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Whether the villager is alive (1 float = 1 value)
        sensor.AddObservation(isAlive);

        // Distance to the well (1 float = 1 value)
        sensor.AddObservation(Vector3.Distance(village.well.transform.position, transform.position));

        // Distance to the warehouse (1 float = 1 value)
        sensor.AddObservation(Vector3.Distance(village.warehouse.transform.position, transform.position));

        // Food count in warehouse (1 float = 1 value)
        sensor.AddObservation(village.warehouse.FoodCount);

        // Direction to well (1 Vector3 = 3 values)
        sensor.AddObservation((village.well.transform.position - transform.position).normalized);

        // Direction to warehouse (1 Vector3 = 3 values)
        sensor.AddObservation((village.warehouse.transform.position - transform.position).normalized);

        // Direction villager is facing (1 Vector3 = 3 values)
        sensor.AddObservation(transform.forward);

        // Whether the villager is hungry (1 float = 1 value)
        sensor.AddObservation(HungerCurrent);

        // Whether the villager is thirsty (1 float = 1 value)
        sensor.AddObservation(ThirstCurrent);

        // Whether the villager is tired (1 float = 1 value)
        sensor.AddObservation(StaminaCurrent);

        // Whether the villager is tired (1 float = 1 value)
        sensor.AddObservation(RestTimeCurrent);

        sensor.AddObservation(comfort);
        //// 1 + 1 + 1 + 1 + 3 + 3 + 3 + 1 + 1 + 1 + 1 + 1 = 18 total values
    }

    protected void FixedUpdate()
    {
        // Request a decision every 5 steps. RequestDecision() automatically calls RequestAction(),
        // but for the steps in between, we need to call it explicitly to take action using the results
        // of the previous decision
        if (StepCount % 5 == 0)
        {
            RequestDecision();
        }
        else
        {
            RequestAction();
        }
    }
    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Food"))
        {
            CollideFood(collision.gameObject);
        }
        else if (collision.transform.CompareTag("Warehouse"))
        {
            Warehouse warehouse = collision.gameObject.GetComponent<Warehouse>();
            CollideWarehouse(warehouse);
        }
        else if (collision.transform.CompareTag("Well"))
        {
            Drink();
        }
    }

    #region Simple action

    protected void Move(float forwardAmount, float turnAmount)
    {
        rigidbody.MovePosition(transform.position + transform.forward * forwardAmount * moveSpeedCurrent * Time.fixedDeltaTime);
        transform.Rotate(transform.up * turnAmount * parametersGiver.TurnSpeed * Time.fixedDeltaTime);
        moveSpeedCurrent = moveSpeedMax;
    }

    protected void Dash()
    {
        moveSpeedCurrent = parametersGiver.DashPower;
        SubstractStamina(parametersGiver.StaminaDashTick);
    }
    protected virtual void CollideFood(GameObject collectable)
    {
        if (HungerCurrent + parametersGiver.FoodValue < parametersGiver.HungerMax)
        {
            if (villageArea != null)
                villageArea.RemoveSpecificCollectable(collectable);
            else
                Destroy(collectable);

            HungerCurrent = parametersGiver.HungerMax;
            //AddReward(0.5f);
        }
    }

    protected virtual void CollideWarehouse(Warehouse warehouse)
    {
        if (HungerCurrent + parametersGiver.FoodValue < parametersGiver.HungerMax && warehouse.TakeFood())
        {
            HungerCurrent = parametersGiver.HungerMax;
            //AddReward(5f);
        }
    }

    protected void Drink()
    {
        if (ThirstCurrent + parametersGiver.WaterValue < parametersGiver.ThirstMax)
        {
            ThirstCurrent = parametersGiver.ThirstMax;
            //AddReward(5f);
        }
    }

    public virtual void Die()
    {
        isAlive = false;
        village.Detach(this);
        AddReward(-10f);
        StopAllCoroutines();

        if (isTraining)
            EndEpisode();
        else
            Destroy(this.gameObject);
    }
    #endregion


    protected void SubstractStamina(float value)
    {
        staminaCurrent -= value;
        if (StaminaCurrent <= 0)
        {
            StopCoroutine(Tiredness());
            StartCoroutine(Tiredness());
        }
    }

    #region Coroutines
    protected IEnumerator Tiredness()
    {
        AddReward(-0.5f);
        isControlEnabled = false;
        RestTimeCurrent = parametersGiver.RestTime;
        while (RestTimeCurrent > 0)
        {
            RestTimeCurrent--;
            yield return new WaitForSeconds(1f);
        }
        isControlEnabled = true;
    }

    IEnumerator ExistancePunishment()
    {
        while (isAlive)
        {
            HungerCurrent -= parametersGiver.HungerTick;
            if (HungerCurrent > 0)
                AddReward(-1f / (HungerCurrent * rewardModifier));
            else
            {
                deathReson = DeathReson.Hunger;
                break;
            }

            ThirstCurrent -= parametersGiver.ThirstTick;
            if (ThirstCurrent > 0)
                AddReward(-1f / (ThirstCurrent * rewardModifier));
            else
            {
                deathReson = DeathReson.Thirst;
                break;
            }

            yield return new WaitForSeconds(1f);
        }
        Die();
    }

    IEnumerator StaminaRegeration()
    {
        while (isAlive)
        {
            if (StaminaCurrent < comfort)
                StaminaCurrent += parametersGiver.StaminaTick;
            yield return null;
        }
    }

    public void UpdateObserver()
    {
        comfort = village.Comfort;
    }

    #endregion
}

