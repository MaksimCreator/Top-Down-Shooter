using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenuView : MonoBehaviour
{
    [SerializeField] private Button _start;
    [SerializeField] private Button _end;
    [SerializeField] private TextMeshProUGUI _myBest;

    private void Awake()
    {
        _myBest.text = $"My best : {PlayerPrefs.GetInt(Constant.MyBest, 0)}";

        _start.onClick.RemoveAllListeners();
        _end.onClick.RemoveAllListeners();

        _start.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
        _end.onClick.AddListener(Application.Quit);
    }
}
