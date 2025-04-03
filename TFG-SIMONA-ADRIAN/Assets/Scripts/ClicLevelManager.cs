using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Xasu.HighLevel;

public class ClicLevelManager : MonoBehaviour
{
    public static ClicLevelManager Instance { get; private set; }
    public List<GameObject> priorityCarList = new List<GameObject>();
    private bool correctOrder = true;
    public List<int> correctOrderList;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void UpdatePriorityUI()
    {
        for (int i = 0; i < priorityCarList.Count; i++)
        {
            GameObject car = priorityCarList[i];
            TextMesh priorityText = car.GetComponentInChildren<TextMesh>();
            if (priorityText != null)
            {
               
                priorityText.text = (i + 1).ToString();
            }
        }


        foreach (GameObject car in GameManager.Instance.cochesIA)
        {
            if (!priorityCarList.Contains(car))
            {
                TextMesh priorityText = car.GetComponentInChildren<TextMesh>();
                {
                    priorityText.text = "";
                }
            }
        }
    }
    public void CarClicked(GameObject car)
    {
        if (GameManager.Instance.dialogueSystem.dialoguePanel.activeSelf)
            return;
        if (!priorityCarList.Contains(car))
        {
            priorityCarList.Add(car);
        }
        else
        {
            priorityCarList.Remove(car);
        }
        UpdatePriorityUI();

        if (priorityCarList.Count >= GameManager.Instance.cochesIA.Count)
        {
            StartCoroutine(CheckLevelCompletion());
        }
    }

    
    public IEnumerator CheckLevelCompletion()
    {
        int index = 0;

        foreach (var item in priorityCarList)
        {
            OtherCar otherCar = item.GetComponent<OtherCar>();
            if (otherCar == null || index >= correctOrderList.Count || otherCar.carID != correctOrderList[index])
            {
                correctOrder = false;
                GameManager.Instance.incorrectLevel.Add("Prioridades incorrectas");
                break;
            }
            GameObjectTracker.Instance.Interacted($"car-{otherCar.carID}-order-{index + 1}");
            index++;
        }

        if (correctOrder)
            yield return StartCoroutine(MoveCarsInOrder(priorityCarList));
        else
            yield return StartCoroutine(MoveCarsInDisorder(priorityCarList));

        foreach (var car in priorityCarList)
        {
            OtherCar otherCar = car.GetComponent<OtherCar>();
            if (otherCar != null)
            {
                otherCar.clickMove = false; 
            }
        }
        GameManager.Instance.ComprobarNivel();
    }

    private IEnumerator MoveCarsInOrder(List<GameObject> carList)
    {
        foreach (var car in carList)
        {
            OtherCar otherCar = car.GetComponent<OtherCar>();
            if (otherCar != null)
            {
                otherCar.arrow.SetActive(false);
                otherCar.clickMove = true;
                yield return new WaitForSeconds(5.0f);
                
            }
        }
    }
    private IEnumerator MoveCarsInDisorder(List<GameObject> carList)
    {
        foreach (var car in carList)
        {
            OtherCar otherCar = car.GetComponent<OtherCar>();
            if (otherCar != null)
            {
                otherCar.arrow.SetActive(false);
                otherCar.clickMove = true;

            }
        }
        yield return new WaitForSeconds(3.0f);

    }
}
