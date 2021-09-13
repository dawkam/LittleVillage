using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimulationManager : MonoBehaviour
{
    public float TimeScale;
    public int SimulationTime;
    [SerializeField] private float TimeCurrent;
    [SerializeField] private float TimeCurrentLvl;

    // Start is called before the first frame update
    void Start()
    {
        if (TimeScale == 0)
            Debug.LogError("Time Scale is 0!");
        Time.timeScale = TimeScale;
        
        if (SimulationTime == 0)
            Debug.LogError("Simulation Time is 0!");
    }

    private void Update()
    {
        TimeCurrent = Time.realtimeSinceStartup;
        TimeCurrentLvl = Time.timeSinceLevelLoad;
        if (Time.timeSinceLevelLoad >= SimulationTime)
        {
            VillageArea[] villageAreas = GetComponentsInChildren<VillageArea>();
            foreach (VillageArea va in villageAreas) 
            {
                va.SaveData();
            }
            GetComponent<CSVManager>().SaveData();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

}
