namespace QPFramework {

   public interface ISystemGetable: IFactoryGetable {
   }

   public static class CanGetSystemExtension {

      public static T GetSystem<T>(this ISystemGetable self) where T : class, ISystem {
         return self.GetFactory().GetSystem<T>();
      }
   }
}