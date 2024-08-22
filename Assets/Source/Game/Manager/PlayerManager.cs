using UnityEngine;

namespace Manager
{
    public class PlayerManager : MonoBehaviour
    {
        private PlayerController _controler;

        public void Init(PlayerController controler)
        => _controler = controler;

        private void Update()
        => _controler.Update(Time.deltaTime);
    }
}