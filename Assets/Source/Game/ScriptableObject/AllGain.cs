using UnityEngine;

[CreateAssetMenu(menuName = "Conffig/AllGain")]
public class AllGain : ScriptableObject
{
    [SerializeField] private float _cooldownAccalerationGain;
    [SerializeField] private float _cooldownDeathGain;

    public float CooldownAccalerationGain => _cooldownAccalerationGain;
    public float CooldownInvulnerabilityGain => _cooldownDeathGain;
}