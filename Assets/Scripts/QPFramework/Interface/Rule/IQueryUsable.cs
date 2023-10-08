namespace QPFramework {

   public interface IQueryUsable: IFactoryGetable {
   }

   public static class CanSendQueryExtension {

      public static TResult SendQuery<TResult>(this IQueryUsable self, IQuery<TResult> query) {
         return self.GetFactory().SendQuery(query);
      }
   }
}