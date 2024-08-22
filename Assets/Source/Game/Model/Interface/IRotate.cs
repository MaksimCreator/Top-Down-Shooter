using UnityEngine;

public interface IRotate
{
    Quaternion Rotation { get; }
    float MaxDegreesDelta { get; }
    bool IsRotate { get; }
}
