namespace QPFramework {

   public abstract class AbstractModel: IModel {
      private IFactory mContainerSetter;

      IFactory IFactoryGetable.GetFactory() {
         return mContainerSetter;
      }

      void IModel.Init() {
         OnInit();
      }

      void IFactorySetable.SetFactory(IFactory containerSetter) {
         mContainerSetter = containerSetter;
      }

      protected abstract void OnInit();
   }
}