using System;
using System.Collections.Generic;

namespace QPFramework {

   public class ObjectContainer {
      private Dictionary<Type, object> mInstance = new Dictionary<Type, object>();

      public void Inject<T>(T instance) {
         var key = typeof(T);
         if(mInstance.ContainsKey(key)) {
            mInstance[key] = instance;
         }
         else {
            mInstance.Add(key, instance);
         }
      }

      public T Extract<T>() where T : class {
         var key = typeof(T);

         if(mInstance.TryGetValue(key, out var retInstance)) {
            return retInstance as T;
         }

         return null;
      }
   }
}