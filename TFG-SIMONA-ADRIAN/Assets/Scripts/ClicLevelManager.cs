using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClicLevelManager : MonoBehaviour
{
    public static ClicLevelManager Instance { get; private set; }
    public List<GameObject> priorityCarList = new List<GameObject>();


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
        if (!priorityCarList.Contains(car))
        {
            priorityCarList.Add(car);
        }
        else
        {
            priorityCarList.Remove(car);
        }
    }

    public void CheckLevelCompletion()
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
                GameManager.Instance.incorrectLevel.Add("Prioridades incorrectas");
                break;
            }
            id++;
        }
        GameManager.Instance.ComprobarNivel();
    }
}
