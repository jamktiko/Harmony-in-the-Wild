using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    [SerializeField] private int experience;
    [SerializeField] private int level;
    private void Awake()
    {
        if (DontDestroyOnLoadManagers.instance != null)
        {
            Debug.LogWarning("There is more than one Game Events Manager in the scene");
            Destroy(gameObject);
        }

        else
        {
            instance = this;

        }
    }
    public int LevelCheck() 
    {
        level = experience / 100;
        Debug.Log("Player leveled up to level "+level);
        return level;
    }
    //public Vector3 GetDefaultPlayerPosition()
    //{
    //    return defaultPlayerPosition;
    //}
}
