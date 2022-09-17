using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwap : MonoBehaviour
{
    [SerializeField] GameObject fade;

    public void StartGame()
    {
        StartCoroutine(Fade());
    }

    public IEnumerator Fade()
    {
        Time.timeScale = 1f;
        fade.SetActive(true);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Main",LoadSceneMode.Single);
        
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }
}
