using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GamplayMenuView : MonoBehaviour
{
    [SerializeField] private Button _exit;
    [SerializeField] private TextMeshProUGUI _score;

    private Wallet _wallet;

    public void Init(Wallet walletPlayer,Action EnterExitMenuView)
    {
        _wallet = walletPlayer;

        _exit.onClick.RemoveAllListeners();
        _exit.onClick.AddListener(() =>
        {
            Disable();
            EnterExitMenuView();
        });
    }

    public void Disable()
    => gameObject.SetActive(false);

    public void Enter()
    => gameObject.SetActive(true);

    private void UpdateScore()
    => _score.text = _wallet.Score.ToString();

    private void OnEnable()
    => _wallet.OnUpdate += UpdateScore;

    private void OnDisable()
    => _wallet.OnUpdate -= UpdateScore;
}