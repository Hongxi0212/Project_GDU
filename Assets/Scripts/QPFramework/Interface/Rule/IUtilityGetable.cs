namespace QPFramework {

   public interface IUtilityGetable: IFactoryGetable {
   }

   public static class CanGetUtilityExtension {

      public static T GetUtility<T>(this IUtilityGetable self) where T : class, IUtility {
         return self.GetFactory().GetUtility<T>();
      }
   }
}