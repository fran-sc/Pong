using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] TMP_Text txtScoreP1;
    [SerializeField] TextMeshProUGUI txtScoreP2;
    [SerializeField] GameObject pelota;

    int p1Score;
    int p2Score;
    bool running = false;

    public void AddPointP1()
    {
        p1Score++;
    }

    public void AddPointP2()
    {
        p2Score++;
    }

    void Start()
    {
        Cursor.visible = false;
    }

    void Update()
    {
        // Debug.Log($"P1: {p1Score} - P2: {p2Score}");        

        // Salimos del juego al pulsar la tecla Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        // Iniciamos el juego al pulsar la tecla Espacio
        if (Input.GetKeyDown(KeyCode.Space) && !running)
        {
            running = true;
            pelota.SetActive(true);
        }
    }

    void OnGUI()
    {
        // Actualizamos el marcador
        txtScoreP1.text = p1Score.ToString();
        txtScoreP2.text = p2Score.ToString();
    }
}
