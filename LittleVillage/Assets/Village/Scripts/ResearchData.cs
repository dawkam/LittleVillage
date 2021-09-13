using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchData
{

    public List<int> collectorsCount = new List<int>();
    public List<int> collectorsDeathByMonster = new List<int>();
    public List<int> collectorsDeathByHunger = new List<int>();
    public List<int> collectorsDeathByThirst = new List<int>();
    public List<int> lumberjacksCount = new List<int>();
    public List<int> lumberjacksDeathByMonster = new List<int>();
    public List<int> lumberjacksDeathByHunger = new List<int>();
    public List<int> lumberjacksDeathByThirst = new List<int>();
    public List<int> artisansCount = new List<int>();
    public List<int> artisansDeathByMonster = new List<int>();
    public List<int> artisansDeathByHunger = new List<int>();
    public List<int> artisansDeathByThirst = new List<int>();
    public List<int> babysCount = new List<int>();
    public List<int> babysDeathByMonster = new List<int>();
    public List<int> babysDeathByHunger = new List<int>();
    public List<int> babysDeathByThirst = new List<int>();
    public List<int> warehouseFoodCount = new List<int>();
    public List<int> sceneFoodCount = new List<int>();
    public List<int> warehouseWoodCount = new List<int>();
    public List<int> sceneTreeCount = new List<int>();
    public List<int> sceneWoodCount = new List<int>();
    public List<float> simulationTime = new List<float>();
    public List<DeathReson> mayorDeathReson = new List<DeathReson>();
    public List<float> comfort = new List<float>();

}

public enum DeathReson
{
    None,
    Monster,
    Hunger,
    Thirst
}