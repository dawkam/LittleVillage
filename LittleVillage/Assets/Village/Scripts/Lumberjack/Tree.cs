using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    private VillageArea villageArea;
    private ParametersGiver parametersGiver;
    private float lifeCurrent;
    private ParticleSystem particle;

    private void Awake()
    {
        parametersGiver = FindObjectOfType<ParametersGiver>();
        villageArea = GetComponentInParent<VillageArea>();
        lifeCurrent = parametersGiver.TreeLifeMax;
        particle = GetComponentInChildren<ParticleSystem>();
        particle.gameObject.SetActive(false);
    }

    public bool TakeDamage()
    {
        lifeCurrent -= parametersGiver.TreeLifeTick;
        particle.gameObject.SetActive(false);
        particle.gameObject.SetActive(true);
        if (lifeCurrent <= 0)
        {
            villageArea.RemoveSpecificTree(this.gameObject);
            return true;
        }
        return false;
    }



}
