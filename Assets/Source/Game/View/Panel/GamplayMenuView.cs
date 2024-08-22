using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GamplayMenuView : MonoBehaviour
{
    private const float DelayPrint = 0.65f;

    [SerializeField] private Button _exit;
    [SerializeField] private TextMeshProUGUI _score;

    private IDisposable _disposable;
    private Wallet _wallet;

    public void Init(Wallet walletPlayer,Action<Action,int> exit)
    {
        _exit.onClick.RemoveAllListeners();
        _exit.onClick.AddListener(() => 
        {
            exit(Enter,_wallet.Score);
            gameObject.SetActive(false);
        });

        _wallet = walletPlayer;
    }

    public void Disable()
    => gameObject.SetActive(false);

    private void Enter()
    => gameObject.SetActive(true);

    private void UpdateScore()
    => _score.text = _wallet.Score.ToString();

    private void OnEnable()
    => _wallet.OnUpdate += UpdateScore;

    private void OnDisable()
    => _wallet.OnUpdate -= UpdateScore;
}