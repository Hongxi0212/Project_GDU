#if UNITY_EDITOR

using UnityEditor;

#endif

using UnityEngine;

namespace QPFramework.CounterApp {

   public interface IStorage: IUtility {

      void SaveInt(string key, int value = 0);

      int LoadInt(string key, int defaultValue = 0);

      public class PlayerPrefsStorage: IStorage {

         public int LoadInt(string key, int defaultValue = 0) {
            return PlayerPrefs.GetInt(key, defaultValue);
         }

         public void SaveInt(string key, int value = 0) {
            PlayerPrefs.SetInt(key, value);
         }
      }

      public class EditorPrefsStorage: IStorage {

         public int LoadInt(string key, int defaultValue = 0) {
#if UNITY_EDITOR
            return EditorPrefs.GetInt(key, defaultValue);
#else
                return 0;
#endif
         }

         public void SaveInt(string key, int value = 0) {
#if UNITY_EDITOR
            EditorPrefs.SetInt(key, value);
#endif
         }
      }
   }
}