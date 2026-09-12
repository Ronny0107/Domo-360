using UnityEngine;
using UnityEngine.InputSystem;

public class Look360 : MonoBehaviour
{
    private NIS inputActions;

    [Header("Configuracion")]
    public float sensitivity = 0.1f;
    public Transform playerBody; // objeto padre para rotación horizontal

    [SerializeField] Transform[] Domos;
    private int domoActualIndex = 0;

    private float xRotation = 0f;
    private Vector2 lookInput;

    private void Awake()
    {
        inputActions = new NIS();
    }

    private void OnEnable()
    {
        inputActions.View.Enable();

        inputActions.View.Look.performed += OnLook;

        inputActions.View.Next.performed += OnNext;
        inputActions.View.Last.performed += OnLast;

        inputActions.View.Look.canceled += OnLook;
    }

    private void OnDisable()
    {
        inputActions.View.Look.performed -= OnLook;
        inputActions.View.Look.canceled -= OnLook;

        inputActions.View.Next.performed -= OnNext;
        inputActions.View.Last.performed -= OnLast;

        inputActions.View.Disable();
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        RotateCamera();
    }

    private void RotateCamera()
    {
        float mouseX = lookInput.x * sensitivity;
        float mouseY = lookInput.y * sensitivity;

        // Rotación vertical (arriba / abajo)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rotación horizontal (izquierda / derecha)
        if (playerBody != null)
        {
            playerBody.Rotate(Vector3.up * mouseX);
        }
    }

    private void OnNext(InputAction.CallbackContext context)
    {
        AumentarDomo();
    }

    private void OnLast(InputAction.CallbackContext context)
    {
        VolverDomo();
    }

    void AumentarDomo()
    {
        if (Domos == null || Domos.Length == 0) return;

        domoActualIndex++;

        if (domoActualIndex >= Domos.Length)
        {
            domoActualIndex = 0; // del último vuelve al primero
        }

        TeletransportarADomo();
    }
    void VolverDomo()
    {
        if (Domos == null || Domos.Length == 0) return;

        domoActualIndex--;

        if (domoActualIndex < 0)
        {
            domoActualIndex = Domos.Length - 1; // del primero vuelve al último
        }

        TeletransportarADomo(); 
    }

    void TeletransportarADomo()
    {
        if (playerBody != null)
            {
                playerBody.position = Domos[domoActualIndex].position;
            }
    }
}
