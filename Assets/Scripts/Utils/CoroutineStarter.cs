using UnityEngine;

namespace Utils
{
    public class CoroutineStarter : MonoBehaviour
    {
        private void Awake()
        {
            DIContainer.Instance.Register<CoroutineStarter>(this , isSingleton: true);
            DontDestroyOnLoad(this.gameObject);
        }
    }
}

