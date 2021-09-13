using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class ArtisanAgent : VillagerAgent
{
    public bool isInsideVillage;
    private bool hasWood;
    protected float craftTimeCurrent;

    public ParticleSystem particle;

    public bool HasWood { get => hasWood; private set => hasWood = value; }
    public float CraftTimeCurrent { get => craftTimeCurrent; protected set => craftTimeCurrent = value; }

    protected override void  InitializeData() 
    {
        base.InitializeData();
        particle = GetComponentInChildren<ParticleSystem>();
        particle.gameObject.SetActive(false);
    }

    protected override void CollideWarehouse(Warehouse warehouse)
    {
        base.CollideWarehouse(warehouse);
        if (warehouse.TakeWood())
            hasWood = true;
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        base.OnActionReceived(actions);

        if (actions.DiscreteActions[3] == 1)
            Craft();
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        base.Heuristic(actionsOut);
        if (Input.GetKey(KeyCode.C))
        {
            //craft
            actionsOut.DiscreteActions.Array[3] = 1;
        }
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        base.CollectObservations(sensor); //18

        sensor.AddObservation(hasWood);

        sensor.AddObservation(isInsideVillage);

        //// 18  + 1  = 20 total values
    }

    private void Craft()
    {
        if (hasWood)
        {
            StopCoroutine(Crafting());
            StartCoroutine(Crafting());
        }

    }

    protected IEnumerator Crafting()
    {
        isControlEnabled = false;
        hasWood = false;
        CraftTimeCurrent = parametersGiver.CraftTime;
        while (isAlive && CraftTimeCurrent > 0)
        {
            CraftTimeCurrent--;
            yield return new WaitForSeconds(1f);
            particle.gameObject.SetActive(false);
            particle.gameObject.SetActive(true);
        }
        if (isInsideVillage)
        {
            AddReward(5f);
            village.AddComfort();
        }
        else
            AddReward(-2f);

        isControlEnabled = true;

    }

    protected override void ResetData()
    {
        isInsideVillage = false;
        hasWood = false;
        base.ResetData();
    }

    public override void Die()
    {
        base.Die();
        switch (deathReson)
        {
            case DeathReson.Monster:
                village.artisansDeathByMonster++;
                break;
            case DeathReson.Hunger:
                village.artisansDeathByHunger++;
                break;
            case DeathReson.Thirst:
                village.artisansDeathByThirst++;
                break;
        }
    }
}
