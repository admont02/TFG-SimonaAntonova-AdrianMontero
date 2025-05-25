using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Xasu.HighLevel;
/// <summary>
/// Clase para detectar si se incumple una señal de calle prohibida
/// </summary>
public class ForbiddenSign : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            //GameObjectTracker.Instance.Interacted("forbidden-sign-error", GameObjectTracker.TrackedGameObject.GameObject);

            if (!GameManager.Instance.incorrectLevel.Contains("Has entrado por una calle prohibida."))
                GameManager.Instance.incorrectLevel.Add("Has entrado por una calle prohibida.");

        }
    }
}
