using UnityEngine;
using UnityEngine.InputSystem;

public class Herman : MonoBehaviour
{
    // Прив, если ты это читаешь, ты легенда нахуй, я постарался тут расписать че за что отвечает, за сливки разбирайся сам
    public float moveSpeed = 5f;
    public float aimMoveSpeed = 2.5f;
    public float acceleration = 10f;
    public float deceleration = 7f;

    [Header("Рефы")]
    public Transform legsTransform;
    public Transform bodyTransform;
    public Camera mainCamera;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isAiming;

    //Эта хуйня при старте получает нужные вводные, мол ригидбоди, позицию мышки и тд и тп
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (mainCamera == null) mainCamera = Camera.main;
        rb.gravityScale = 0;
        rb.freezeRotation = true;
    }

    // Если не поймешь че это, лейм
    void Update()
    {
        Vector2 input = Vector2.zero;
        var keyboard = Keyboard.current;

        if (keyboard != null)
        {
            if (keyboard.wKey.isPressed) input.y += 1;
            if (keyboard.sKey.isPressed) input.y -= 1;
            if (keyboard.aKey.isPressed) input.x -= 1;
            if (keyboard.dKey.isPressed) input.x += 1;
        }
        moveInput = input.normalized;
        if (Mouse.current != null)
        {
            isAiming = Mouse.current.rightButton.isPressed;
        }
    }

    // Тут всё высчитывается и работает (половину помог написать ИИ, потому что я не умею работать с юнити :)))
    void FixedUpdate()
    {
        float currentMaxSpeed = isAiming ? aimMoveSpeed : moveSpeed;
        Vector2 targetVelocity = moveInput * currentMaxSpeed;

        float lerpSpeed = (moveInput.magnitude > 0) ? acceleration : deceleration;

        rb.linearVelocity = Vector2.MoveTowards(rb.linearVelocity, targetVelocity, lerpSpeed * Time.fixedDeltaTime);

        Rotations();
    }

    // эта хуйня отвечает за правильные повороты ног и тела
    void Rotations()
    {
        if (rb.linearVelocity.magnitude > 0.1f)
        {
            float angleLegs = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
            legsTransform.rotation = Quaternion.Slerp(legsTransform.rotation, Quaternion.Euler(0, 0, angleLegs - 90), Time.fixedDeltaTime * 10f);
        }

        if (isAiming)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            Vector3 worldMousePos = mainCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));
            Vector2 lookDir = (Vector2)worldMousePos - rb.position;
            float angleBody = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
            bodyTransform.rotation = Quaternion.Euler(0, 0, angleBody - 90);
        }
        else
        {
            bodyTransform.rotation = Quaternion.Slerp(bodyTransform.rotation, legsTransform.rotation, Time.fixedDeltaTime * 8f);
        }
    }
}
