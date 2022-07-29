using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    [SerializeField]
    float movementSpeed;
    [SerializeField]
    float rotationSpeed;
    public Text moneyText;

    public GameManager gameManager;
    public GameObject Paper;
    public GameObject Moneys;
    public int paperCount;
    public int tablePaperCount;
    public int money;

    bool dragStart = false;

    Touch _touch;

    Vector3 _touchDown;
    Vector3 _touchUp;

    public Animator anim;


    void Start()
    {
        
    }


    void Update()
    {
        if (Input.touchCount > 0)
        {
            _touch = Input.GetTouch(0);

            if (_touch.phase == TouchPhase.Began)
            {
                dragStart = true;
                anim.SetBool("isMoving", true);
                _touchDown = _touch.position;
                _touchUp = _touch.position;

            }

        }
        if (dragStart)
        {
            if (_touch.phase == TouchPhase.Moved)
            {
                _touchDown = _touch.position;
            }
            if (_touch.phase == TouchPhase.Ended)
            {
                _touchDown = _touch.position;
                dragStart = false;
                anim.SetBool("isMoving", false);

            }
            gameObject.transform.rotation = Quaternion.RotateTowards(transform.rotation, CalculateRotation(), rotationSpeed * Time.deltaTime);
            transform.Translate(Vector3.forward * Time.deltaTime * movementSpeed);
        }

        tablePaperCount = Mathf.Clamp(tablePaperCount, 0, 20);
        paperCount = Mathf.Clamp(paperCount, 0, 20);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("PickupArea"))
        {
            if (paperCount < 20)
            {
                paperCount += Paper.GetComponentsInChildren<Transform>().GetLength(0);

            }
            else
                paperCount = 20;

        }
        if (other.tag.Equals("DropPaperArea"))
        {
            if (tablePaperCount < 20)
            {
                tablePaperCount += paperCount;
            }
            else
                tablePaperCount = 20;

            StartCoroutine(DropPapers());
        }
        if (other.tag.Equals("CollectMoneyArea"))
        {
            money += Moneys.GetComponentsInChildren<Transform>().GetLength(0);
            CollectMoney();            
            tablePaperCount = 0;
            moneyText.text = money.ToString();
        }


    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals("PickupArea"))
        {
            StartCoroutine(PickupPaper());
            gameManager.isPicking = true;

        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("PickupArea"))
        {

            gameManager.isPicking = false;
        }
        if (other.tag.Equals("DropPaperArea"))
        {
            paperCount = 0;

        }
    }
    IEnumerator PickupPaper()
    {

        int papercount = Paper.transform.childCount;

        for (int i = papercount - 1; i >= 0; i--)
        {
            if (gameManager.Paper[i].activeInHierarchy)
            {
                yield return new WaitForSeconds(.1f);
                gameManager.Paper[i].SetActive(false);
            }
        }
    }

    IEnumerator DropPapers()
    {
        for (int i = paperCount - 1; i >= 0; i--)
        {
            if (!gameManager.TablePaper[i].activeInHierarchy)
            {
                yield return new WaitForSeconds(.1f);
                gameManager.TablePaper[i].SetActive(true);
            }
        }
    }
    void CollectMoney()
    {
        foreach (var item in gameManager.Money)
        {

            if (item.activeInHierarchy)
            {
                
                item.SetActive(false);

            }

        }
    }

    Quaternion CalculateRotation()
    {
        Quaternion temp = Quaternion.LookRotation(CalculateDirection(), Vector3.up);
        return temp;
    }


    Vector3 CalculateDirection()
    {
        Vector3 temp = (_touchDown - _touchUp).normalized;
        temp.z = temp.y;
        temp.y = 0;
        return temp;
    }
}
