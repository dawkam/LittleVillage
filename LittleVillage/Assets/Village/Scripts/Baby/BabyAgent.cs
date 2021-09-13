using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyAgent : VillagerAgent
{
    public VillagerRole adultVersion;

    public float currentAge;

    public void StartGrowing(VillagerRole villagerRole)
    {
        adultVersion = villagerRole;
        StartCoroutine(GrowingUp());
    }
    IEnumerator GrowingUp()
    {
        while (isAlive && currentAge< parametersGiver.AdulthoodAge)
        {
            currentAge++;
            yield return new WaitForSeconds(1f);
        }
        village.SpawnVillager(adultVersion, transform.position);
        Destroy(gameObject);
    }

    public override void Die()
    {
        base.Die();
        switch (deathReson)
        {
            case DeathReson.Monster:
                village.babysDeathByMonster++;
                break;
            case DeathReson.Hunger:
                village.babysDeathByHunger++;
                break;
            case DeathReson.Thirst:
                village.babysDeathByThirst++;
                break;
        }
    }
}
