using System;

namespace QPFramework {

   public class Singleton<T> where T : Singleton<T> {
      private static T mInstance;

      public static T Instance {
         get {
            if(mInstance == null) {
               var type = typeof(T);
               var ctorInfos = type.GetConstructors(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
               var ctorInfo = Array.Find(ctorInfos, m => m.GetParameters().Length == 0);

               if(ctorInfo == null) {
                  throw new Exception("No NonPublic Constructor Without Parameters Found in " + type.Name);
               }

               mInstance = ctorInfo.Invoke(null) as T;
            }

            return mInstance;
         }
      }
   }
}