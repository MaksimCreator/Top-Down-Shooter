using UnityEngine;

[CreateAssetMenu(menuName = "Conffig/AllGain")]
public class AllGainConffig : ScriptableObject,IService
{
    [SerializeField] private float _cooldownAccalerationGain;
    [SerializeField] private float _cooldownInvulnerabilityGain;

    public float CooldownAccalerationGain => _cooldownAccalerationGain;
    public float CooldownInvulnerabilityGain => _cooldownInvulnerabilityGain;
}
