using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class LumberjackAgent : VillagerAgent
{
    private int woodBag;
    public int WoodBag { get => woodBag; private set => woodBag = value; }


    protected void CollideWood(GameObject wood)
    {
        if (woodBag < parametersGiver.WoodBagSize)
        {
            if (villageArea != null)
                villageArea.RemoveSpecificWood(wood);
            else
                Destroy(wood);

            AddReward(1f);
            woodBag++;
            moveSpeedMax -= (parametersGiver.MoveSpeedMax - 1) / parametersGiver.MoveSpeedMax;
        }
    }

    protected void CollideTree(GameObject tree)
    {
        if (isControlEnabled)
        {
            Tree t = tree.GetComponent<Tree>();
            if (t != null)
            {
                SubstractStamina(parametersGiver.StaminaDashTick * 10);
                if (t.TakeDamage())
                    AddReward(5f);
                else
                    AddReward(1f);
            }
        }
    }
    protected override void CollideWarehouse(Warehouse warehouse)
    {
        base.CollideWarehouse(warehouse);
        warehouse.AddWood(woodBag);
        AddReward(woodBag);
        woodBag = 0;
        moveSpeedMax = parametersGiver.MoveSpeedMax;

        if (villageArea.TreesCount == 0 && isTraining)
        {
            AddReward(5f);
            EndEpisode();
        }
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        if (collision.transform.CompareTag("Tree"))
        {
            CollideTree(collision.gameObject);
        }
        else if (collision.transform.CompareTag("Wood"))
        {
            CollideWood(collision.gameObject);
        }

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        base.CollectObservations(sensor); //17

        sensor.AddObservation(woodBag);

        sensor.AddObservation(moveSpeedCurrent);

        sensor.AddObservation(parametersGiver.WoodBagSize);

        //// 18 + 1 + 1 + 1= 21 total values
    }

    protected override void ResetData()
    {
        woodBag = 0;
        base.ResetData();
    }
    public override void Die()
    {
        base.Die();
        switch (deathReson)
        {
            case DeathReson.Monster:
                village.lumberjacksDeathByMonster++;
                break;
            case DeathReson.Hunger:
                village.lumberjacksDeathByHunger++;
                break;
            case DeathReson.Thirst:
                village.lumberjacksDeathByThirst++;
                break;
        }
    }

}
