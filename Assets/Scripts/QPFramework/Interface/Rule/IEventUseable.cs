using UnityEngine;
using UnityEngine.Events;

namespace QPFramework {

   public interface IEventUseable: IFactoryGetable {
   }

   public static class UseEventExtension {

      public static void SendEvent(this IEventUseable self, string eventName) {
         self.GetFactory().SendEvent(eventName);
      }

      public static void RegisterEvent(this IEventUseable self, string eventName, UnityAction listener) {
         self.GetFactory().RegisterEvent(eventName, listener);
      }
   }
}