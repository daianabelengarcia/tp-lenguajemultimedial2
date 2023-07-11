using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrileteMovement : MonoBehaviour
{
    public Transform[] elements;
    public Transform[] transitionElements;
    public Transform[] ramitas1;
    public Transform[] ramitas2;
    public Transform[] ramitas3;
    public Camera mainCamera;
    public float transitionDuration = 10.0f; // Duración de la transición en segundos

    private int contadordeClicks = 0;
    public int clicks = 10;
    private bool subiendo = false;
    private Vector2[] posicionInicial;
    private Vector3 cameraInitialPosition;
    private Vector2[] posicionRamitas1;
    private Vector2[] posicionRamitas2;
    private Vector2[] posicionRamitas3;
    public float posicionfinalRamitas;

    public float velHorizontal = 1.0f; // Velocidad horizontal de los elementos
    public float ampHorizontal = 0.5f; // Amplitud del movimiento horizontal

    private void Start()
    {
        posicionInicial = new Vector2[elements.Length];
        posicionRamitas1 = new Vector2[ramitas1.Length];
        posicionRamitas2 = new Vector2[ramitas2.Length];
        posicionRamitas3 = new Vector2[ramitas3.Length];
        posicionfinalRamitas = 0f;

        for (int i = 0; i < ramitas1.Length; i++)
        {
            ramitas1[i].gameObject.SetActive(false);
            posicionRamitas1[i] = ramitas1[i].position;
        }
        for (int i = 0; i < ramitas2.Length; i++)
        {
            ramitas2[i].gameObject.SetActive(false);
            posicionRamitas2[i] = ramitas2[i].position;
        }
        for (int i = 0; i < ramitas3.Length; i++)
        {
            ramitas3[i].gameObject.SetActive(false);
            posicionRamitas3[i] = ramitas3[i].position;
        }

        for (int i = 0; i < elements.Length; i++)
        {
            posicionInicial[i] = elements[i].position;
            transitionElements[i].gameObject.SetActive(false);
        }

        cameraInitialPosition = mainCamera.transform.position;
    }

    private void Update()
    {
        if (subiendo)
        {
            Volar();
            MoveCameraUp();
        }
    }

    private void OnMouseDown()
    {
        contadordeClicks++;

        if (contadordeClicks <= clicks)
        {
            MovHorizontal();
            Caer();
        }
        else
        {
            comenzarTransicion();
        }
    }

    private void comenzarTransicion()
    {
        subiendo = true;

        // Desactivar elementos originales y activar elementos de transición
        for (int i = 0; i < elements.Length; i++)
        {
            elements[i].gameObject.SetActive(false);
            transitionElements[i].gameObject.SetActive(true);
            transitionElements[i].position = elements[i].position;
        }

        Invoke("finTransicion", transitionDuration);
    }

    private void finTransicion()
    {
        subiendo = false;
        ResetElements();
        contadordeClicks = 0;
    }

    private void MovHorizontal()
    {
        for (int i = 0; i < elements.Length; i++)
        {
            elements[i].position = posicionInicial[i];
            Vector2 targetPosition = elements[i].position;
            targetPosition.x += Random.Range(-0.3f, 0.3f); // Movimiento aleatorio en el eje X
            elements[i].position = targetPosition;
        }
    }

    private void Volar()
    {
        for (int i = 0; i < transitionElements.Length; i++)
        {
            Vector2 targetPosition = transitionElements[i].position;
            targetPosition.y += 1f * Time.deltaTime; // Movimiento hacia arriba

            // Movimiento horizontal utilizando una función senoidal
            float xOffset = Mathf.Sin(Time.time * velHorizontal) * ampHorizontal;
            targetPosition.x += xOffset * Time.deltaTime;

            Vector3 viewportPosition = mainCamera.WorldToViewportPoint(targetPosition); // Convertir a coordenadas de ventana de vista

            // Limitar los movimientos en el eje X dentro del rango de la pantalla
            viewportPosition.x = Mathf.Clamp01(viewportPosition.x);

            targetPosition = mainCamera.ViewportToWorldPoint(viewportPosition); // Convertir de vuelta a coordenadas del mundo

            transitionElements[i].position = targetPosition;
        }
    }

    private void Caer()
    {
        if (contadordeClicks == 3)
        {
            for (int i = 0; i < ramitas1.Length; i++)
            {
                ramitas1[i].gameObject.SetActive(true);

                ramitas1[i].position = posicionRamitas1[i];
                Vector2 targetPosition2 = ramitas1[i].position;
                targetPosition2.y -= 1f * Time.deltaTime;

                float xOffset = Mathf.Sin(Time.time * velHorizontal) * ampHorizontal;
                targetPosition2.x += xOffset * Time.deltaTime;

                ramitas1[i].position = targetPosition2;

                if (ramitas1[i].position.y <= posicionfinalRamitas)
                {
                    Rigidbody2D rb = ramitas1[i].GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        rb.isKinematic = true;
                    }
                }
            }
        }
        if (contadordeClicks == 6)
        {
            
            for (int i = 0; i < ramitas2.Length; i++)
            {
                ramitas2[i].gameObject.SetActive(true);

                ramitas2[i].position = posicionRamitas2[i];
                Vector2 targetPosition3 = ramitas2[i].position;
                targetPosition3.y -= 1f * Time.deltaTime;

                float xOffset = Mathf.Sin(Time.time * velHorizontal) * ampHorizontal;
                targetPosition3.x += xOffset * Time.deltaTime;

                ramitas2[i].position = targetPosition3;

                if (ramitas2[i].position.y <= posicionfinalRamitas)
                {
                    Rigidbody2D rb = ramitas2[i].GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        rb.isKinematic = true;
                    }
                }
            }
        }
        if (contadordeClicks == 9)
        {
            
            for (int i = 0; i < ramitas3.Length; i++)
            {
                ramitas3[i].gameObject.SetActive(true);

                ramitas3[i].position = posicionRamitas3[i];
                Vector2 targetPosition4 = ramitas3[i].position;
                targetPosition4.y -= 1f * Time.deltaTime;

                float xOffset = Mathf.Sin(Time.time * velHorizontal) * ampHorizontal;
                targetPosition4.x += xOffset * Time.deltaTime;

                ramitas3[i].position = targetPosition4;

                if (ramitas3[i].position.y <= posicionfinalRamitas)
                {
                    Rigidbody2D rb = ramitas3[i].GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        rb.isKinematic = true;
                    }
                }
            }
        }
    }

    private void MoveCameraUp()
    {
        Vector3 targetPosition = mainCamera.transform.position;
        targetPosition.y += 1.0f * Time.deltaTime; // Movimiento de la cámara hacia arriba a una velocidad constante
        mainCamera.transform.position = targetPosition;
    }

    private void ResetElements()
    {
        for (int i = 0; i < ramitas1.Length; i++)
        {
            ramitas1[i].gameObject.SetActive(false);
            ramitas1[i].position = posicionRamitas1[i];
        }
        for (int i = 0; i < ramitas2.Length; i++)
        {
            ramitas2[i].gameObject.SetActive(false);
            ramitas2[i].position = posicionRamitas2[i];
        }
        for (int i = 0; i < ramitas3.Length; i++)
        {
            ramitas3[i].gameObject.SetActive(false);
            ramitas3[i].position = posicionRamitas3[i];
        }

        for (int i = 0; i < elements.Length; i++)
        {
            elements[i].gameObject.SetActive(true);
            transitionElements[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < elements.Length; i++)
        {
            elements[i].position = posicionInicial[i];
        }

        mainCamera.transform.position = cameraInitialPosition;
    }
}