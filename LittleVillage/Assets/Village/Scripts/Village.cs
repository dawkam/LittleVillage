using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Village : MonoBehaviour, IObservable
{
    private float comfort;
    List<IObserver> observers = new List<IObserver>();
    private ParametersGiver parametersGiver;

    public GameObject well;
    public Warehouse warehouse;
    public ResearchData researchData;

    public GameObject CollectorPrefab;
    public GameObject LumberjackPrefab;
    public GameObject ArtisanPrefab;
    public GameObject BabyPrefab;

    public List<GameObject> collectors = new List<GameObject>();
    public int collectorsDeathByMonster;
    public int collectorsDeathByHunger;
    public int collectorsDeathByThirst;

    public List<GameObject> lumberjacks = new List<GameObject>();
    public int lumberjacksDeathByMonster;
    public int lumberjacksDeathByHunger;
    public int lumberjacksDeathByThirst;

    public List<GameObject> artisans = new List<GameObject>();
    public int artisansDeathByMonster;
    public int artisansDeathByHunger;
    public int artisansDeathByThirst;

    public List<GameObject> babys = new List<GameObject>();
    public int babysDeathByMonster;
    public int babysDeathByHunger;
    public int babysDeathByThirst;





    public float Comfort
    {
        get => comfort;

        private set
        {
            comfort = value;
            NotifyObservers();
        }
    }

    private void Awake()
    {
        parametersGiver = FindObjectOfType<ParametersGiver>();
        ResetData();
    }
    public void ResetData()
    {
        StopCoroutine(ComfortConsumption());
        Comfort = parametersGiver.ComfortMin; /*+ Random.Range(0, 10)*/;
        observers.Clear();
        StartCoroutine(ComfortConsumption());

        ClearList(ref collectors);

        ClearList(ref lumberjacks);

        ClearList(ref artisans);

        ClearList(ref babys);

        collectorsDeathByMonster = 0;
        collectorsDeathByHunger = 0;
        collectorsDeathByThirst = 0;

        lumberjacksDeathByMonster = 0;
        lumberjacksDeathByHunger = 0;
        lumberjacksDeathByThirst = 0;

        artisansDeathByMonster = 0;
        artisansDeathByHunger = 0;
        artisansDeathByThirst = 0;

        babysDeathByMonster = 0;
        babysDeathByHunger = 0;
        babysDeathByThirst = 0;

        warehouse.ResetData();
    }

    IEnumerator ComfortConsumption()
    {
        while (true)
        {
            if (Comfort > parametersGiver.ComfortMin)
                Comfort -= parametersGiver.ComfortTick;
            yield return new WaitForSeconds(1f);
        }
    }

    public void Attach(IObserver observer)
    {
        observers.Add(observer);
        observer.UpdateObserver();
    }

    public void Detach(IObserver observer)
    {
        observers.Remove(observer);
    }

    public void NotifyObservers()
    {
        foreach (IObserver obs in observers)
            obs.UpdateObserver();
    }

    public void AddComfort()
    {
        Comfort += parametersGiver.ComfortTick * 50;
    }

    public int GetVillagersCount()
    {
        return 1 + GetCollectorsCount() + GetLumberjacksCount() + GetArtisansCount() + GetBabysCount();
    }
    public int GetCollectorsCount()
    {
        collectors.RemoveAll(item => item == null);
        return collectors.Count;
    }
    public int GetLumberjacksCount()
    {
        lumberjacks.RemoveAll(item => item == null);
        return lumberjacks.Count;
    }
    public int GetArtisansCount()
    {
        artisans.RemoveAll(item => item == null);
        return artisans.Count;
    }

    public int GetBabysCount()
    {
        babys.RemoveAll(item => item == null);
        return babys.Count;
    }
    public void SpawnVillager(VillagerRole villagerRole, Vector3 position)
    {
        switch (villagerRole)
        {
            case VillagerRole.Collector:
                SpawnSpecificVillager(CollectorPrefab, collectors, position);
                break;
            case VillagerRole.Lumberjack:
                SpawnSpecificVillager(LumberjackPrefab, lumberjacks, position);
                break;
            case VillagerRole.Artisan:
                SpawnSpecificVillager(ArtisanPrefab, artisans, position);
                break;
        }
    }

    private GameObject SpawnSpecificVillager(GameObject prefab, List<GameObject> list, Vector3 position)
    {
        GameObject gm = Instantiate(prefab, position, gameObject.transform.rotation, gameObject.transform.parent);
        list.Add(gm);
        return gm;
    }

    public void SpawnBaby(VillagerRole villagerRole)
    {
        GameObject gm = SpawnSpecificVillager(BabyPrefab, babys, transform.position);

        VillageArea villageArea = transform.parent.GetComponent<VillageArea>();
        villageArea.PlaceGameObjectInSafeZone(gm);

        gm.GetComponent<BabyAgent>().StartGrowing(villagerRole);
    }

    private void ClearList(ref List<GameObject> list)
    {
        if (list != null)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] != null)
                {
                    Destroy(list[i]);
                }
            }
        }

        list = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        ArtisanAgent artisanAgent = other.gameObject.GetComponent<ArtisanAgent>();
        if (artisanAgent != null)
            artisanAgent.isInsideVillage = true;
    }

    private void OnTriggerExit(Collider other)
    {
        ArtisanAgent artisanAgent = other.gameObject.GetComponent<ArtisanAgent>();
        if (artisanAgent != null)
            artisanAgent.isInsideVillage = false;
    }
}
