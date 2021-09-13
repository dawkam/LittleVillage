using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class MayorAgent : VillagerAgent
{
    private float babyTime;

    protected override void InitializeData()
    {
        base.InitializeData();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        base.CollectObservations(sensor); //18

        sensor.AddObservation(village.warehouse.WoodCount);

        sensor.AddObservation(village.warehouse.FoodCount);

        sensor.AddObservation(village.GetVillagersCount());

        sensor.AddObservation(village.GetCollectorsCount());

        sensor.AddObservation(village.GetLumberjacksCount());

        sensor.AddObservation(village.GetArtisansCount());

        //// 18 + 1 + 1 + 1 + 1 + 1 + 1 = 24 total values

    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        base.OnActionReceived(actions);

        if (actions.DiscreteActions[3] == 1)
            CreateVillager(VillagerRole.Collector);
        if (actions.DiscreteActions[4] == 1)
            CreateVillager(VillagerRole.Lumberjack);
        if (actions.DiscreteActions[5] == 1)
            CreateVillager(VillagerRole.Artisan);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        base.Heuristic(actionsOut);
        if (Input.GetKey(KeyCode.C))
        {
            actionsOut.DiscreteActions.Array[3] = 1;
            if (village.warehouse.FoodCount < village.GetVillagersCount())
                AddReward(3f);
        }
        if (Input.GetKey(KeyCode.V))
        {
            actionsOut.DiscreteActions.Array[4] = 1;
            if (village.warehouse.WoodCount < village.GetArtisansCount())
                AddReward(3f);
        }
        if (Input.GetKey(KeyCode.B))
        {
            actionsOut.DiscreteActions.Array[5] = 1;
            if (village.warehouse.WoodCount > village.GetArtisansCount())
                AddReward(3f);
        }
    }
    private void CreateVillager(VillagerRole villagerRole)
    {
        if (babyTime <= 0 && village.warehouse.TakeFoodForBaby())
        {
            AddReward(1f);
            StartCoroutine(BabyCoolDown());
            village.SpawnBaby(villagerRole);
        }
    }

    IEnumerator BabyCoolDown()
    {
        babyTime = parametersGiver.BabyCoolDown;
        while (isAlive && babyTime >= 0)
        {
            babyTime--;
            yield return new WaitForSeconds(1f);
        }
    }

    public override void Die()
    {
        base.Die();
    }
}
