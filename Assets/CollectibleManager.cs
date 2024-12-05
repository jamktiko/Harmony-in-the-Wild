using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (SaveManager.instance.gameData.berryData!=null || SaveManager.instance.gameData.PineconeData !=null)
        {
            int i=0;
            int x=0;
            PlayerManager.instance.GenerateCollectibleData();
            foreach (var berry in PlayerManager.instance.BerryData)
            {
                GameObject.Find(berry.Key).transform.GetChild(0).gameObject.SetActive(berry.Value);
                x++;
            }
            foreach (var cone in PlayerManager.instance.PineConeData)
            {
                GameObject.Find(cone.Key).SetActive(cone.Value);
                i++;
            }

            Debug.Log(i + " pinecones loaded");
            Debug.Log(x + " berries loaded");
        }
        else
        {
            foreach (Berries berry in FindObjectsOfType<Berries>())
            {
                PlayerManager.instance.BerryData.Add(berry.transform.parent.name, berry.gameObject.activeInHierarchy);
            }
            PlayerManager.instance.Berries = 0;
            foreach (PineconesCollectable cone in FindObjectsOfType<PineconesCollectable>())
            {
                PlayerManager.instance.PineConeData.Add(cone.gameObject.name, cone.gameObject.activeInHierarchy);
            }
            PlayerManager.instance.PineCones = 0;
        }
    }
}
