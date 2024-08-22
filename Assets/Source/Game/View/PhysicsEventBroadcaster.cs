using UnityEngine;

public class PhysicsEventBroadcaster : MonoBehaviour
{
    private object _model;
    private PhysicsRouter _router;

    public void Init(object model, PhysicsRouter router)
    {
        _model = model;
        _router = router;
    }

    public new bool TryGetComponent<T>(out T type) where T : class
    {
        if (_model is T component)
        {
            type = component;
            return true;
        }
        type = null;
        return false;
    }

    private void OnCollisionEnter(Collision collision)
    => TryBindCollision(collision.gameObject);

    private void OnTriggerEnter(Collider other)
    => TryBindCollision(other.gameObject);

    private void OnTriggerExit(Collider other)
    => TryBindCollision(other.gameObject);

    private void TryBindCollision(GameObject model)
    {
        if (model.TryGetComponent(out PhysicsEventBroadcaster broadcaster))
            _router.TryAddCollision(_model, broadcaster._model);
    }
}