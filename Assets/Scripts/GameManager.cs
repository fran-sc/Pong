using UnityEngine;
using UnityEngine.UI;

/*
 GameManager
 -----------------
 Responsabilidad:
 - Coordina el estado global del juego (marcador, estado de ejecución y objetos globales como la pelota).
 - Proporciona métodos para incrementar el puntaje de cada jugador.
 - Maneja entradas principales del juego (iniciar partida, salir) y la actualización del UI del marcador.

 Notas sobre diseño:
 - Esta clase mantiene los contadores de puntos como enteros simples (p1Score, p2Score).
 - Los campos marcados con [SerializeField] se asignan desde el editor de Unity y permiten conectar
   referencias a objetos UI y de escena sin hacerlas públicas.
 - El uso de OnGUI aquí se limita a actualizar el texto del marcador; en producción puede sustituirse
   por un sistema UI más moderno (por ejemplo, actualizando directamente componentes Text en eventos).
*/
public class GameManager : MonoBehaviour
{
    // Referencias serializadas para asignar desde el Inspector de Unity.
    // txtScoreP1: componente Text que muestra el marcador del jugador 1.
    // txtScoreP2: componente Text que muestra el marcador del jugador 2.
    // pelota: GameObject de la pelota que se activa/desactiva para iniciar o parar la partida.
    [SerializeField] Text txtScoreP1;
    [SerializeField] Text txtScoreP2;
    [SerializeField] GameObject pelota;

    /*
     Estructuras de datos internas:
     - p1Score, p2Score: contadores enteros simples que representan el puntaje acumulado.
     - running: bandera booleana que indica si la partida está en curso. Inicialmente false.
    */
    int p1Score;
    int p2Score;
    bool running = false;

    /*
     AddPointP1 / AddPointP2
     -------------------------
     Métodos públicos usados por otros componentes (por ejemplo, colisiones de la pelota con los 'goals')
     para notificar al GameManager que un jugador ha anotado. Simplemente incrementan el contador
     correspondiente. No manejan efectos secundarios (como reiniciar la posición de la pelota) aquí,
     para mantener la separación de responsabilidades.
    */
    public void AddPointP1()
    {
        p1Score++;
    }

    public void AddPointP2()
    {
        p2Score++;
    }

    // Start se ejecuta al iniciar la escena.
    void Start()
    {
        // Oculta el cursor para una experiencia de juego inmersiva.
        Cursor.visible = false;
    }

    /*
     Update se ejecuta cada frame y gestiona:
     - Entrada del jugador para salir (Escape).
     - Entrada para iniciar la partida (Espacio). Al iniciar, activa la pelota y fija la bandera running.
     - (Opcional) un Debug.Log comentado que muestra el estado del marcador.

     Nota: la lógica de inicio es intencionalmente mínima; la activación física de la pelota se delega
     al GameObject referenciado para mantener este gestor como coordinador de alto nivel.
    */
    void Update()
    {
        // Debug.Log($"P1: {p1Score} - P2: {p2Score}");

        // Salimos del juego al pulsar la tecla Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        // Iniciamos el juego al pulsar la tecla Espacio si aún no está en ejecución
        if (Input.GetKeyDown(KeyCode.Space) && !running)
        {
            running = true;
            pelota.SetActive(true);
        }
    }

    /*
     OnGUI se usa aquí para sincronizar el texto visible del marcador con los contadores internos.
     - Convierte los enteros a string y los asigna a los componentes Text.
     - Aunque OnGUI pertenece al IMGUI antiguo, para este proyecto pequeño resulta suficiente.
     - Si se cambia a un sistema basado en eventos, se pueden actualizar los textos sólo cuando
       cambian los puntajes para mejorar eficiencia.
    */
    void OnGUI()
    {
        // Actualizamos el marcador
        txtScoreP1.text = p1Score.ToString();
        txtScoreP2.text = p2Score.ToString();
    }
}
