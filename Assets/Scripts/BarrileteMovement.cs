using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrileteMovement : MonoBehaviour
{
    public Transform[] elements;
    public Transform[] transitionElements;
    public Camera mainCamera;
    public float transitionDuration = 10.0f; // Duración de la transición en segundos

    private int contadordeClicks = 0;
    public int clicks = 10;
    private bool subiendo = false;
    private Vector2[] posicionInicial;
    private Vector3 cameraInitialPosition;

    public float velHorizontal = 1.0f; // Velocidad horizontal de los elementos
    public float ampHorizontal = 0.5f; // Amplitud del movimiento horizontal

    private void Start()
    {
        posicionInicial = new Vector2[elements.Length];
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

    private void MoveCameraUp()
    {
        Vector3 targetPosition = mainCamera.transform.position;
        targetPosition.y += 1.0f * Time.deltaTime; // Movimiento de la cámara hacia arriba a una velocidad constante
        mainCamera.transform.position = targetPosition;
    }

    private void ResetElements()
    {
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