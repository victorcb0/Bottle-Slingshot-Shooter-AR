using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LaunchManager : MonoBehaviour
{
    [Header("Ball Settings")]
    public GameObject ballPrefab;   // Prefab-ul sferei
    public Transform spawnPoint;    // Punctul de lansare (de ex. camera)
    public float zOffset = 1f;      // Offset pentru poziția pe axa Z

    [Header("Launch Force Settings")]
    public float maxLaunchForce = 10f; // Forța maximă de lansare
    public float maxDistance = 500f;   // Distanța maximă pentru forța maximă

    [Header("Input Action")]
    public InputActionReference dragCurrentPositionAction; // Referință la acțiunea "Drag Current Position"

    private Vector2 startDragPosition; // Poziția de început a drag-ului
    private Vector2 endDragPosition;   // Poziția de sfârșit a drag-ului
    private bool isDragging = false;   // Indicator pentru starea de drag

    private void OnEnable()
    {
        dragCurrentPositionAction.action.started += OnDragStarted;
        dragCurrentPositionAction.action.performed += OnDragPerformed;
        dragCurrentPositionAction.action.canceled += OnDragCanceled;
        dragCurrentPositionAction.action.Enable();
    }

    private void OnDisable()
    {
        dragCurrentPositionAction.action.started -= OnDragStarted;
        dragCurrentPositionAction.action.performed -= OnDragPerformed;
        dragCurrentPositionAction.action.canceled -= OnDragCanceled;
        dragCurrentPositionAction.action.Disable();
    }

    private void OnDragStarted(InputAction.CallbackContext context)
    {
        // Când se începe drag-ul, setăm flag-ul de dragging și reținem poziția de start
        isDragging = true;
        startDragPosition = Pointer.current.position.ReadValue();
    }

    private void OnDragPerformed(InputAction.CallbackContext context)
    {
        // Actualizăm permanent endDragPosition în timp ce utilizatorul face drag
        if (isDragging)
        {
            endDragPosition = Pointer.current.position.ReadValue();
        }
    }

    private void OnDragCanceled(InputAction.CallbackContext context)
    {
        // Când utilizatorul ridică degetul, se finalizează drag-ul
        isDragging = false;
        endDragPosition = Pointer.current.position.ReadValue();

        // Instanțiem și aruncăm bila DOAR la finalul drag-ului
        ThrowBall();
    }

    // Instanțiază sfera și îi aplică forța în direcția camerei.
    private void ThrowBall()
    {
        // Validări
        if (ballPrefab == null || spawnPoint == null)
        {
            Debug.LogError("BallPrefab sau SpawnPoint nu este setat!");
            return;
        }

        // Calculează distanța dintre cele două puncte (startDragPosition și endDragPosition)
        float dragDistance = Vector2.Distance(startDragPosition, endDragPosition);

        // Calculează forța pe baza distanței drag-ului, limitată la o valoare maximă
        float launchForce = Mathf.Clamp01(dragDistance / maxDistance) * maxLaunchForce;

        Debug.Log($"Coordonatele punctului de start: {startDragPosition}");
        Debug.Log($"Coordonatele punctului de sfârșit: {endDragPosition}");
        Debug.Log($"Distanța dintre touch-uri: {dragDistance}");
        Debug.Log($"Forța de tragere calculată (proporțională cu distanța): {launchForce}");

        // Instanțiem sfera exact în fața camerei (spawnPoint) și aplicăm rotația implicită
        Vector3 adjustedSpawnPoint = spawnPoint.position + spawnPoint.forward * zOffset;
        GameObject newBall = Instantiate(ballPrefab, adjustedSpawnPoint, Quaternion.identity);

        // Luăm (sau adăugăm) un Rigidbody pe noua bilă
        Rigidbody newBallRb = newBall.GetComponent<Rigidbody>();
        if (newBallRb == null)
        {
            newBallRb = newBall.AddComponent<Rigidbody>();
        }

        // În momentul lansării, activăm gravitația
        newBallRb.useGravity = true;

        // Direcția de lansare este "în față" față de spawnPoint
        Vector3 throwDirection = spawnPoint.forward;
        newBallRb.AddForce(throwDirection * launchForce, ForceMode.Impulse);

        // Distrugem sfera după 3 secunde
        Destroy(newBall, 3f);
    }
}