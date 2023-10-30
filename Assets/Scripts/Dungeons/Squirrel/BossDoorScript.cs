using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossDoorScript : MonoBehaviour
{
    [SerializeField]
    List<LeverScript> scripts=new List<LeverScript>();
    public bool isOpen;
    public int usedlevers, levers;
    [SerializeField] Material usedMat;
    // Start is called before the first frame update
    void Start()
    {
        scripts=FindObjectsOfType<LeverScript>().ToList();
       usedlevers=scripts.Where(x=>x.used).Count();
       levers= scripts.Where(x => !x.used).Count();
    }

    // Update is called once per frame
    void Update()
    {
        if (usedlevers == levers&&isOpen==false) 
        {
            isOpen = true;
            gameObject.GetComponent<MeshRenderer>().material=usedMat;
            gameObject.GetComponent<AudioSource>().Play();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (isOpen&&other.CompareTag("Player")) 
        {
            SceneManager.LoadScene("Dungeon_Squirrel_Boss");
        }
    }
}
