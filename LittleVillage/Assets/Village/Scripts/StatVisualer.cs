using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatVisualer : MonoBehaviour
{
    VillagerAgent villagerAgent;
    ParametersGiver parametersGiver;
    Village village;

    [SerializeField] GameObject thirstBar;
    [SerializeField] TextMeshPro thirstTxt;
    [SerializeField] GameObject hungerBar;
    [SerializeField] TextMeshPro hungerTxt;
    [SerializeField] GameObject staminaBar;
    [SerializeField] TextMeshPro staminaTxt;
    [SerializeField] TextMeshPro bagTxt;

    private void Start()
    {
        parametersGiver = GameObject.FindObjectOfType<ParametersGiver>();
        villagerAgent = GetComponentInParent<VillagerAgent>();
        village = GetComponentInParent<VillageArea>().village.GetComponent<Village>();
    }

    private void Update()
    {
        SetSize(thirstBar.transform, villagerAgent.ThirstCurrent / parametersGiver.ThirstMax);
        thirstTxt.text = Convert.ToInt32(villagerAgent.ThirstCurrent * 100 / parametersGiver.ThirstMax).ToString() + "%";

        SetSize(hungerBar.transform, villagerAgent.HungerCurrent / parametersGiver.HungerMax);
        hungerTxt.text = Convert.ToInt32(villagerAgent.HungerCurrent * 100 / parametersGiver.HungerMax).ToString() + "%";

        SetSize(staminaBar.transform, villagerAgent.StaminaCurrent / parametersGiver.ComfortMin);
        staminaTxt.text = Convert.ToInt32(villagerAgent.StaminaCurrent * 100 / village.Comfort).ToString() + "%";

        if (villagerAgent is CollectorAgent collector)
            bagTxt.text = $"Food bag: {collector.FoodBag}";
        else if (villagerAgent is LumberjackAgent lumberjack)
            bagTxt.text = $"Wood bag: {lumberjack.WoodBag}";
    }

    public void SetSize(Transform bar, float normalizedSize)
    {
        bar.localScale = new Vector3(normalizedSize, 1f);
    }
}
