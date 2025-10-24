using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float moveSpeed = 5f;    // Velocidad de movimiento horizontal
    public float jumpForce = 10f;   // Fuerza del salto

    [Header("Configuración de Suelo")]
    public Transform groundCheck;         // Un GameObject vacío puesto a los pies del jugador
    public LayerMask groundLayer;         // La capa que define que es "suelo"
    public float groundCheckRadius = 0.2f; // Radio del circulo para detectar el suelo

    // Componentes y variables privadas
    private Rigidbody2D rb;
    private float horizontalInput;
    private bool isGrounded;
    private bool isFacingRight = true; // Para saber hacia donde mira el personaje

    // Start se llama una vez al inicio
    void Start()
    {
        // Obtenemos el componente Rigidbody2D para poder usar físicas
        rb = GetComponent<Rigidbody2D>();
    }

    // Update se llama una vez por frame (ideal para inputs)
    void Update()
    {
        // 1. OBTENER INPUT HORIZONTAL
        // Input.GetAxis("Horizontal") usa las teclas A/D y las flechas izquierda/derecha
        // Devuelve un valor entre -1 (izquierda) y 1 (derecha)
        horizontalInput = Input.GetAxis("Horizontal");

        // 2. DETECTAR SALTO
        // Input.GetButtonDown("Jump") detecta la barra espaciadora (por defecto en Unity)
        // Solo podemos saltar si "isGrounded" es verdadero
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // Aplicamos una fuerza vertical instantánea
            // Usamos VelocityChange para ignorar la masa del jugador y dar un salto consistente
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        // 3. VOLTEAR EL SPRITE
        FlipSprite();
    }

    // FixedUpdate se llama en un intervalo fijo (ideal para físicas)
    void FixedUpdate()
    {
        // 4. COMPROBAR SI ESTÁ EN EL SUELO
        // Dibuja un círculo invisible en la posici�n de 'groundCheck'
        // Si ese círculo toca algo en la 'groundLayer', isGrounded es true
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // 5. APLICAR MOVIMIENTO HORIZONTAL
        // Movemos el Rigidbody cambiando su velocidad (velocity)
        // Mantenemos la velocidad vertical (rb.velocity.y) para que la gravedad y el salto sigan funcionando
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
    }

    // Funci�n para voltear el sprite del jugador
    void FlipSprite()
    {
        // Si nos movemos a la izquierda (input < 0) pero miramos a la derecha (isFacingRight = true)
        if (horizontalInput < 0 && isFacingRight)
        {
            // Volteamos al jugador
            transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, transform.localScale.z);
            isFacingRight = false; // Ahora miramos a la izquierda
        }
        // Si nos movemos a la derecha (input > 0) pero miramos a la izquierda (isFacingRight = false)
        else if (horizontalInput > 0 && !isFacingRight)
        {
            // Volteamos al jugador
            transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, transform.localScale.z);
            isFacingRight = true; // Ahora miramos a la derecha
        }
    }

    // (Opcional) Dibuja el círculo de groundCheck en el editor para que puedas verlo
    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}