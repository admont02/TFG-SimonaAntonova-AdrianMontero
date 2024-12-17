using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClicLevelManager : MonoBehaviour
{
    public static ClicLevelManager Instance { get; private set; }
    public List<GameObject> priorityCarList = new List<GameObject>();
    private bool correctOrder = true;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
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
    }
    public void OnConfirmButtonClicked()
    {
        if(priorityCarList.Count >= GameManager.Instance.cochesIA.Count)
        StartCoroutine(CheckLevelCompletion());
    }
    public IEnumerator CheckLevelCompletion()
    {
        //// Lógica para comprobar si el nivel se completó correctamente
        //int expectedOrder = 0;
        //foreach (var car in priorityCarList)
        //{
        //    int carOrder;
        //    if (!int.TryParse(car.name.Substring(car.name.Length - 1), out carOrder) || carOrder != expectedOrder)
        //    {
        //        Debug.Log("Nivel Incorrecto");
        //        dialogueSystem.ShowIncorrectLevelDialog(new string[] { "Prioridades incorrectas" });
        //        return;
        //    }
        //    expectedOrder++;
        //}
        //Debug.Log("¡Nivel completado correctamente!");
        //dialogueSystem.ShowCompletedDialog();
        int id = 0;
        foreach (var item in priorityCarList)
        {
            if (item.name[item.name.Length - 1].ToString() != id.ToString())
            {
                correctOrder = false;
                GameManager.Instance.incorrectLevel.Add("Prioridades incorrectas");
                break;
            }
            id++;
        }
        if(correctOrder)
        yield return StartCoroutine(MoveCarsInOrder(priorityCarList));
        else
            yield return StartCoroutine(MoveCarsInDisorder(priorityCarList));
        GameManager.Instance.ComprobarNivel();
    }
    private IEnumerator MoveCarsInOrder(List<GameObject> carList)
    {
        foreach (var car in carList)
        {
            OtherCar otherCar = car.GetComponent<OtherCar>();
            if (otherCar != null)
            {
                otherCar.MoveToDestinations();

                while (!otherCar.HasArrived())
                {
                    yield return null; // Espera a que el coche llegue a su destino } } } GameManager.Instance.canCarMove = false;
                }
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
                otherCar.MoveToDestinations();

            }
        }
        yield return new WaitForSeconds(5.0f);
    }
}
