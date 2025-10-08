using UnityEngine;

/*
 PalaController
 -----------------
 Responsabilidad:
 - Controla el movimiento vertical de una pala (player 1 o player 2) en respuesta a la entrada del teclado.
 - Restringe el movimiento a un rango vertical definido por MIN_Y y MAX_Y.

 Diseño y campos:
 - Las constantes MIN_Y / MAX_Y definen los límites de movimiento en el eje Y.
 - El campo `speed` (serializado) permite ajustar la velocidad desde el Inspector sin exponerlo públicamente.
 - El script determina si controla la pala 1 o 2 consultando la etiqueta (`tag`) del GameObject.
*/
public class PalaController : MonoBehaviour
{
    // Límites verticales para evitar que la pala salga del área de juego.
    const float MAX_Y = 4.2f;
    const float MIN_Y = -4.2f;

    // Velocidad de movimiento en unidades por segundo (ajustable desde el Inspector).
    [SerializeField] float speed = 7f;

    // Update se invoca una vez por frame y decide qué esquema de control usar según la etiqueta.
    void Update()
    {
        // Si este GameObject tiene la etiqueta "Pala1" utiliza las teclas W/S
        if (CompareTag("Pala1"))
        {
            Player1Movement();
        }
        // Si tiene la etiqueta "Pala2" utiliza las flechas Arriba/Abajo
        else if (CompareTag("Pala2"))
        {
            Player2Movement();
        }
    }

    // Movimiento de la pala del jugador 1
    void Player1Movement()
    {
        // Subir mientras no supere MAX_Y
        if (Input.GetKey("w") && transform.position.y < MAX_Y)
        {
            // Mover hacia arriba con velocidad ajustada por Time.deltaTime para ser frame-rate independiente
            transform.Translate(Vector3.up * speed * Time.deltaTime);
        }

        // Bajar mientras no supere MIN_Y
        if (Input.GetKey("s") && transform.position.y > MIN_Y)
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);
        }
    }


    // Movimiento de la pala del jugador 2
    void Player2Movement()
    {
        // Subir usando la flecha "up" y respetando el límite superior
        if (Input.GetKey("up") && transform.position.y < MAX_Y)
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
        }

        // Bajar usando la flecha "down" y respetando el límite inferior
        if (Input.GetKey("down") && transform.position.y > MIN_Y)
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);
        }
    }
}
