using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterLevel : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // si es player cambia al nivel del trigger
        if (other.gameObject.layer == 3)
            SceneManager.LoadScene(gameObject.name);
    }
}
