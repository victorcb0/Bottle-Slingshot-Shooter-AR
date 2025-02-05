using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TouchVisualizerManager : MonoBehaviour
{
    public InputActionReference dragCurrentPositionAction; // Referință la "Drag Current Position"
    public GameObject touchStartCirclePrefab; // Prefab pentru cercul de la începutul touch-ului
    public GameObject touchCurrentCirclePrefab; // Prefab pentru cercul de la poziția curentă
    public Image lineImage; // Referință la obiectul de tip Image care va reprezenta linia
    public float maxLineLength = 500f; // Lungimea maximă configurabilă a liniei

    private GameObject touchStartCircleInstance; // Instanța cercului de la începutul touch-ului
    private GameObject touchCurrentCircleInstance; // Instanța cercului de la poziția curentă
    private Vector2 startPosition; // Poziția de început

    private Canvas canvas; // Referință la Canvas

    void Start()
    {
        // Găsește Canvas-ul din scenă
        canvas = FindObjectOfType<Canvas>();

        if (canvas == null)
        {
            Debug.LogError("Canvas-ul nu a fost găsit în scenă!");
        }

        if (lineImage != null)
        {
            lineImage.gameObject.SetActive(false); // Ascunde linia la început
            lineImage.type = Image.Type.Filled; // Setează tipul imaginii la "Filled"
            lineImage.fillMethod = Image.FillMethod.Vertical; // Metoda de umplere pe verticală
            lineImage.fillOrigin = 0; // Umple de jos în sus
        }
    }

    void OnEnable()
    {
        // Activează acțiunea
        dragCurrentPositionAction.action.Enable();

        // Abonare la evenimentele de input
        dragCurrentPositionAction.action.started += OnTouchStarted;
        dragCurrentPositionAction.action.performed += OnTouchPerformed;
        dragCurrentPositionAction.action.canceled += OnTouchCanceled;
    }

    void OnDisable()
    {
        // Dezabonare de la evenimentele de input
        dragCurrentPositionAction.action.started -= OnTouchStarted;
        dragCurrentPositionAction.action.performed -= OnTouchPerformed;
        dragCurrentPositionAction.action.canceled -= OnTouchCanceled;

        // Dezactivează acțiunea
        dragCurrentPositionAction.action.Disable();
    }

    private void OnTouchStarted(InputAction.CallbackContext context)
    {
        // Creează cercul pentru poziția de start
        if (touchStartCircleInstance == null)
        {
            touchStartCircleInstance = Instantiate(touchStartCirclePrefab, canvas.transform);
        }

        startPosition = context.ReadValue<Vector2>();
        touchStartCircleInstance.transform.position = startPosition;
        touchStartCircleInstance.SetActive(true);

        Debug.Log("Touch started at: " + startPosition);
    }

    private void OnTouchPerformed(InputAction.CallbackContext context)
    {
        Vector2 currentPosition = context.ReadValue<Vector2>();

        // Creează cercul pentru poziția curentă dacă nu există deja
        if (touchCurrentCircleInstance == null)
        {
            touchCurrentCircleInstance = Instantiate(touchCurrentCirclePrefab, canvas.transform);
        }

        touchCurrentCircleInstance.transform.position = currentPosition;
        touchCurrentCircleInstance.SetActive(true);

        // Actualizează linia între poziția de start și cea curentă
        UpdateLine(startPosition, currentPosition);

        Debug.Log("Dragging at: " + currentPosition);
    }

    private void OnTouchCanceled(InputAction.CallbackContext context)
    {
        // Ascunde cercurile și linia când touch-ul este eliberat
        if (touchStartCircleInstance != null)
        {
            touchStartCircleInstance.SetActive(false);
        }

        if (touchCurrentCircleInstance != null)
        {
            touchCurrentCircleInstance.SetActive(false);
        }

        if (lineImage != null)
        {
            lineImage.gameObject.SetActive(false);
        }

        Debug.Log("Touch ended");
    }

    private void UpdateLine(Vector2 start, Vector2 end)
    {
        if (lineImage == null) return;

        // Calculăm direcția și lungimea
        Vector2 direction = end - start;
        float distance = Mathf.Min(direction.magnitude, maxLineLength);

        // Configurăm linia
        RectTransform lineRect = lineImage.rectTransform;
        lineRect.pivot = new Vector2(0.5f, 0); // Setează pivotul la baza imaginii
        lineRect.position = start; // Poziționează baza imaginii la centrul cercului de start
        lineRect.sizeDelta = new Vector2(lineRect.sizeDelta.x, maxLineLength); // Menține dimensiunea completă
        lineRect.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90); // Setează rotația liniei

        // Actualizează procentul de umplere
        lineImage.fillAmount = distance / maxLineLength;

        lineImage.gameObject.SetActive(true);
    }
}
