namespace HXFramework {
   public interface IQuery<TResult>: IFactoryGetable, IFactorySetable, IModelGetable, IQueryUsable {

      TResult Do();
   }
}