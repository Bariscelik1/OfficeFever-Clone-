using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public List<GameObject> Paper;
    public List<GameObject> TablePaper;
    public List<GameObject> Money;
    public List<GameObject> Table2Paper;
    public List<GameObject> Table2Money;
    public bool isPicking;
    public Character character;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {               
        StartCoroutine(CreatePaper());
        StartCoroutine(CreateMoney());
        StartCoroutine(DeactiveTablePapers());
        
    }

    IEnumerator CreatePaper()
    {
        
        foreach (var item in Paper)
        {
            if (!isPicking)
            {
                if (!item.activeInHierarchy)
                {
                    yield return new WaitForSeconds(.3f);
                    item.SetActive(true);

                }
            }
            
        }
        
    }
   
    IEnumerator CreateMoney()
    {
        
        for (int i = character.tablePaperCount - 1; i >= 0; i--)
        {
            if (!Money[i].activeInHierarchy)
            {
                yield return new WaitForSeconds(.7f);
                Money[i].SetActive(true);
                
            }
        }
        
    }

    IEnumerator DeactiveTablePapers()
    {
        for (int i = character.tablePaperCount - 1; i >= 0; i--)
        {
            if (TablePaper[i].activeInHierarchy)
            {
                yield return new WaitForSeconds(.7f);
                TablePaper[i].SetActive(false);
                

            }
        }

    }
   



}
