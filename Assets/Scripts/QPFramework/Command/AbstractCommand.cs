namespace QPFramework {

   public abstract class AbstractCommand: ICommand {
      private IFactory mContainerSetter;

      void ICommand.Execute() {
         OnExecute();
      }

      IFactory IFactoryGetable.GetFactory() {
         return mContainerSetter;
      }

      void IFactorySetable.SetFactory(IFactory containerSetter) {
         mContainerSetter = containerSetter;
      }

      protected abstract void OnExecute();
   }
}