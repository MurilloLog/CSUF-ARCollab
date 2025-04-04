using UnityEngine;

namespace ARDrawing.Core.Singletons
{
    /// <summary>
    /// A generic singleton base class for MonoBehaviour components in Unity.
    /// Ensures that only one instance of the component exists in the scene.
    /// </summary>
    /// <typeparam name="T">The type of the component inheriting from this Singleton. The script was moved from Many Scripts >> Old AR Scripts folder</typeparam>
    public class Singleton<T> : MonoBehaviour
        where T : Component
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    var objs = FindObjectsOfType(typeof(T)) as T[];
                    if (objs.Length > 0)
                    {
                        _instance = objs[0];
                    }
                    if (objs.Length > 1)
                    {
                        Debug.LogError("More than one " + typeof(T).Name + " in the scene");
                    }
                    if (_instance == null)
                    {
                        GameObject obj = new GameObject();
                        obj.name = string.Format("_{0}", typeof(T).Name);
                        _instance = obj.AddComponent<T>();
                    }
                }
                return _instance;
            }
        }
    }
}