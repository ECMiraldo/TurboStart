using UnityEngine;

namespace UnityUtils {
    public class Singleton<T> : MonoBehaviour where T : Component {

        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            if (Instance == null) Instance = this as T;
            else Destroy(this.gameObject);
        }
    }
}
