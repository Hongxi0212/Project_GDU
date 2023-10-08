namespace QPFramework {

   public interface IModelGetable: IFactoryGetable {
   }

   public static class CanGetModelExtension {

      public static T GetModel<T>(this IModelGetable self) where T : class, IModel {
         return self.GetFactory().GetModel<T>();
      }
   }
}