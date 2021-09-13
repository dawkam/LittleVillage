using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class CollectorAgent : VillagerAgent
{
    private int foodBag;
    public int FoodBag { get => foodBag; private set => foodBag = value; }


    protected override void CollideFood(GameObject collectable)
    {
        if (foodBag < parametersGiver.FoodBagSize)
        {
            if (villageArea != null)
                villageArea.RemoveSpecificCollectable(collectable);
            else
                Destroy(collectable);

            AddReward(1f);
            foodBag++;
            moveSpeedMax -= (parametersGiver.MoveSpeedMax - 1) / parametersGiver.MoveSpeedMax;
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        base.OnActionReceived(actions);

        if (actions.DiscreteActions[3] == 1)
            EatFromBag();
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        base.Heuristic(actionsOut);
        if (Input.GetKey(KeyCode.C))
        {
            //eat from bag
            actionsOut.DiscreteActions.Array[3] = 1;
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        base.CollectObservations(sensor); //17

        sensor.AddObservation(foodBag);

        sensor.AddObservation(moveSpeedCurrent);

        sensor.AddObservation(parametersGiver.FoodBagSize);

        //// 18 + 1 + 1 + 1= 21 total values
    }

    private void EatFromBag()
    {
        if (HungerCurrent + parametersGiver.FoodValue < parametersGiver.HungerMax && foodBag > 0)
        {
            HungerCurrent = parametersGiver.HungerMax;
            foodBag--;
            moveSpeedMax += (parametersGiver.MoveSpeedMax - 1) / parametersGiver.MoveSpeedMax;
        }
    }

    protected override void CollideWarehouse(Warehouse warehouse)
    {
        base.CollideWarehouse(warehouse);
        warehouse.AddFood(foodBag);
        AddReward(foodBag);
        foodBag = 0;
        moveSpeedMax = parametersGiver.MoveSpeedMax;

        if (villageArea.FruitsCount == 0 && isTraining)
        {
            AddReward(5f);
            EndEpisode();
        }
    }

    protected override void ResetData()
    {
        foodBag = 0;
        base.ResetData();
    }

    public override void Die()
    {
        base.Die();
        switch (deathReson)
        {
            case DeathReson.Monster:
                village.collectorsDeathByMonster++;
                break;
            case DeathReson.Hunger:
                village.collectorsDeathByHunger++;
                break;
            case DeathReson.Thirst:
                village.collectorsDeathByThirst++;
                break;
        }
    }
}
