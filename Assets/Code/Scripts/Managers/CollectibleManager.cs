using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (SaveManager.Instance.GameData.BerryData != "{}" && SaveManager.Instance.GameData.PineconeData != "{}")
        {
            int i = 0;
            int x = 0;
            PlayerManager.Instance.GenerateCollectibleData();
            foreach (var berry in PlayerManager.Instance.BerryData)
            {
                GameObject.Find(berry.Key).transform.GetChild(0).gameObject.SetActive(berry.Value);

                // disable interaction indicator if these berries have already been collected
                if (berry.Value == false)
                {
                    GameObject.Find(berry.Key).transform.GetChild(2).gameObject.SetActive(false);
                }

                x++;
            }
            foreach (var cone in PlayerManager.Instance.PineConeData)
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
                PlayerManager.Instance.BerryData.Add(berry.transform.parent.name, berry.gameObject.activeInHierarchy);
            }
            PlayerManager.Instance.Berries = 0;
            foreach (PineconesCollectable cone in FindObjectsOfType<PineconesCollectable>())
            {
                PlayerManager.Instance.PineConeData.Add(cone.gameObject.name, cone.gameObject.activeInHierarchy);
            }
            PlayerManager.Instance.PineCones = 0;
        }
    }
}
