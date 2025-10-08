using System.Collections;
using UnityEngine;

/*
 PelotaController
 -----------------
 Responsabilidad:
 - Gestionar el comportamiento físico y el reinicio de la pelota entre goles.
 - Calcular y aplicar el impulso inicial en una dirección aleatoria con variación angular.
 - Notificar al `GameManager` cuando se marca un gol para actualizar el marcador.

 Diseño y campos:
 - `force`: magnitud del impulso aplicado a la pelota (serializado para ajuste desde Inspector).
 - `delay`: tiempo de espera antes de relanzar la pelota tras un gol.
 - `manager`: referencia al `GameManager` para actualizar el marcador.
 - Constantes para limitar ángulo y posición vertical de lanzamiento.
 - `rb`: referencia al Rigidbody2D usada para aplicar fuerzas.
*/
public class PelotaController : MonoBehaviour
{
    // Parámetros ajustables desde Inspector
    [SerializeField] float force;
    [SerializeField] float delay;
    [SerializeField] GameManager manager;

    // Rango de ángulos (grados) usados al calcular el vector de lanzamiento
    const float MIN_ANG = 30f;
    const float MAX_ANG = 50f;

    // Rango vertical de posición inicial de la pelota al relanzarla
    const float MAX_Y = 2.5f;
    const float MIN_Y = -2.5f;

    // Componente Rigidbody2D para interacción física
    Rigidbody2D rb;
    
    // Start se ejecuta una vez al inicio: cacheamos componentes y hacemos el primer lanzamiento
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // Elegimos una dirección X aleatoria (-1 o 1) para el primer lanzamiento
        int direccionX = Random.Range(0, 2) == 0 ? -1 : 1;
        StartCoroutine(LanzarPelota(direccionX));
    }

    void Update() 
    {
        // Intencionalmente vacío: la lógica de movimiento queda delegada al Rigidbody2D y a las corrutinas
    }

    // OnCollisionEnter2D se ejecuta cuando la pelota choca físicamente con otro collider
    void OnCollisionEnter2D(Collision2D other)
    {
        string tag = other.gameObject.tag;

        // Sólo registramos colisiones con las palas; la física se encarga del rebote
        if (tag == "Pala1" || tag == "Pala2")
        {
            Debug.Log($"Colisión con {tag}");
        }
    }

    /*
     OnTriggerEnter2D se usa para detectar cuando la pelota atraviesa una portería (trigger).
     - Actualiza el marcador en el GameManager.
     - Reinicia la pelota lanzándola hacia la dirección opuesta al gol.
    */
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Gol en {other.gameObject.tag}!!");

        // Actualizamos el marcador y reiniciamos la pelota
        if (other.gameObject.tag == "Porteria2")
        {
            // Porteria2 significa que el jugador 1 anotó
            manager.AddPointP1();
            // Lanzamos la pelota hacia la derecha (1)
            StartCoroutine(LanzarPelota(1));
        }
        else if (other.gameObject.tag == "Porteria1")
        {
            // Porteria1 significa que el jugador 2 anotó
            manager.AddPointP2();
            // Lanzamos la pelota hacia la izquierda (-1)
            StartCoroutine(LanzarPelota(-1));
        }
    }

    /*
     LanzarPelota(int direccionX)
     -------------------------------
     Corrutina que espera `delay` segundos, reposiciona la pelota en una Y aleatoria dentro del rango,
     calcula un vector de impulso con un ángulo aleatorio entre MIN_ANG y MAX_ANG y aplica la fuerza.

     Notas de implementación:
     - Convertimos grados a radianes para usar Mathf.Sin/Mathf.Cos.
     - Normalizamos el vector de impulso mediante las funciones trigonométricas y lo multiplicamos por `force`.
     - Reiniciamos la velocidad lineal a cero antes de añadir el nuevo impulso para evitar acumulaciones.
    */
    IEnumerator LanzarPelota(int direccionX)
    {
        // Esperamos un tiempo (delay) antes de lanzar la pelota
        yield return new WaitForSeconds(delay);

        // Cálculo de la posición vertical inicial (aleatoria dentro del rango)
        float posY = Random.Range(MIN_Y, MAX_Y);
        transform.position = new Vector3(0, posY, 0);

        // Obtenemos un ángulo aleatorio en grados y lo convertimos a radianes
        float angulo = Random.Range(MIN_ANG, MAX_ANG);
        angulo *= Mathf.Deg2Rad; // conversión a radianes

        // Coordenada x del vector de impulso (coseno del ángulo, con la dirección indicada)
        float x = Mathf.Cos(angulo) * direccionX; 

        // Coordenada y del vector de impulso (seno del ángulo con signo aleatorio)
        int direccionY = Random.Range(0, 2) == 0 ? -1 : 1;
        float y = Mathf.Sin(angulo) * direccionY;
        
        // Aplicamos el impulso a la pelota: borramos la velocidad anterior y aplicamos una fuerza de impulso
        Vector2 impulso = new Vector2(x, y);
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(impulso * force, ForceMode2D.Impulse);
    }
}
