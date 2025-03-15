using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForbiddenSign : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            GameManager.Instance.incorrectLevel.Add("Has entrado por una calle prohibida.");

        }
    }
}
