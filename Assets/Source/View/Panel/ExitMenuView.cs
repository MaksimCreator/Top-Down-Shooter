using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ExitMenuView : MonoBehaviour
{
    [SerializeField] private Button _exit;
    [SerializeField] private Button _break;
    [SerializeField] private TextMeshProUGUI _score;

    public void Enter(Action actionBreak,int score)
    {
        gameObject.SetActive(true);
        _score.text = score.ToString();

        _exit.onClick.RemoveAllListeners();
        _break.onClick.RemoveAllListeners();

        _exit.onClick.AddListener(() =>
        {
            if(PlayerPrefs.GetInt(Constant.MyBest, 0) < score)
                PlayerPrefs.SetInt(Constant.MyBest, score);

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        });

        _break.onClick.AddListener(() => 
        {
            gameObject.SetActive(false);
            actionBreak();
        });
    }
}