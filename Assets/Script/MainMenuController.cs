using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Configuración de Transición")]
    public float transitionTime = 1f;
    public GameObject imageOut;             // Tu panel negro de UI
    public Animator transitionFadeout;     // Tu Animator con el trigger "StartFade"

    [Header("Paneles del Menú")]
    public GameObject menuPrincipal;
    public GameObject menuAreUSure;

    private void Start()
    {
        if (imageOut != null) imageOut.SetActive(false);
        if (menuPrincipal != null) menuPrincipal.SetActive(true);
        if (menuAreUSure != null) menuAreUSure.SetActive(false);
    }

    // --- EL BOTÓN DE CONTINUAR / JUGAR ---
    public void StartGame()
    {
        if (SavePlay.Instance != null)
        {
            // Cargamos los datos guardados en el disco
            SavePlay.Instance.LoadData();

            // Si es la primera vez que juega, mandamos a la escena inicial (ej: 5) y checkpoint 0
            if (SavePlay.Instance.lastScene <= 0)
            {
                RespawnSystem.CurrentCheckpointIndex = 0;
                SavePlay.Instance.lastCheckpoint = 0;
                SavePlay.Instance.lastScene = 5; // Tu zona de juego inicial
                SavePlay.Instance.SaveData();
            }
            else
            {
                // Si ya tenía partida, el RespawnSystem leerá el checkpoint guardado
                RespawnSystem.CurrentCheckpointIndex = SavePlay.Instance.lastCheckpoint;
            }

            // Iniciamos la carga con el fundido a negro
            StartCoroutine(LoadSceneRoutine(SavePlay.Instance.lastScene));
        }
    }

    // --- LÓGICA DE NUEVA PARTIDA (RESET PREFS) ---
    public void ResetPrefs()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("PlayerPrefs borrados por completo");

        if (SavePlay.Instance != null)
        {
            SavePlay.Instance.lastScene = 5; // Tu escena inicial por defecto
            SavePlay.Instance.lastCheckpoint = 0;
            SavePlay.Instance.firstGameActive = false;
            SavePlay.Instance.vida = 5;
            SavePlay.Instance.ataque = 1;
            SavePlay.Instance.maxHealth = 5;
            SavePlay.Instance.SaveData();
        }

        RespawnSystem.CurrentCheckpointIndex = 0;

        // Tras borrar todo, iniciamos el juego en la escena inicial
        StartCoroutine(LoadSceneRoutine(5));
    }

    // --- MÉTODOS AUXILIARES DE TU MENÚ ORIGINAL ---
    public void AreYouSure()
    {
        if (menuPrincipal != null) menuPrincipal.SetActive(false);
        if (menuAreUSure != null) menuAreUSure.SetActive(true);
    }

    public void NoImNot()
    {
        if (menuPrincipal != null) menuPrincipal.SetActive(true);
        if (menuAreUSure != null) menuAreUSure.SetActive(false);
    }

    public void YesIAm()
    {
        ResetPrefs();
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Saliendo del juego...");
    }

    // Corrutina simple para hacer el fundido antes de cargar la escena desde el menú
    private IEnumerator LoadSceneRoutine(int sceneIndex)
    {
        if (imageOut != null) imageOut.SetActive(true);
        if (transitionFadeout != null) transitionFadeout.SetTrigger("StartFade");

        yield return new WaitForSecondsRealtime(transitionTime);

        SceneManager.LoadScene(sceneIndex);
    }
}
