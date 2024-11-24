using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTrafficLight : MonoBehaviour
{
    public Material[] materials; // Array de materiales que quieres alternar
    private Material[] currentMaterials; // Array de materiales que quieres alternar
    private MeshRenderer meshRenderer; // Referencia al MeshRenderer del GameObject

    private void Start()
    {
        // Obtén el MeshRenderer del GameObject
        meshRenderer = GetComponent<MeshRenderer>();

        StartCoroutine(ChangeMaterialCoroutine());
    }

    private IEnumerator ChangeMaterialCoroutine()
    {
        float currentSecs = 2.0f;
        bool fromRed = false;

        while (true)
        {
            // Estaba rojo
            if (meshRenderer.materials[3].mainTexture == materials[0].mainTexture)
            {
                currentSecs = 5.0f;
                // Apagar rojo
                meshRenderer.materials[3] = materials[3];
                // Encender ambar
                meshRenderer.materials[5] = materials[1];
                fromRed = true;
            }
            // Estaba Ambar
            else if (meshRenderer.materials[5].mainTexture == materials[1].mainTexture)
            {
                currentSecs = 1.0f;
                // Apagar ambar
                meshRenderer.materials[5] = materials[4];
                if (fromRed)
                {
                    // Encender verde
                    meshRenderer.materials[6] = materials[2];
                }
                else
                {
                    // Encender rojo
                    meshRenderer.materials[3] = materials[0];
                }

            }
            // Estaba verde
            else if (meshRenderer.materials[6].mainTexture == materials[2].mainTexture)
            {
                currentSecs = 5.0f;
                // Apagar verde
                meshRenderer.materials[6] = materials[5];
                // Encender ambar
                meshRenderer.materials[5] = materials[1];
                fromRed = false;

            }

            // Espera 2 segundos antes de cambiar al siguiente material
            yield return new WaitForSeconds(currentSecs);
        }
    }
}
