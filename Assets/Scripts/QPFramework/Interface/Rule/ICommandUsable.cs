namespace QPFramework {

   public interface ICommandUsable: IFactoryGetable {
   }

   public static class CanSendCommandExtension {

      public static void SendCommand<T>(this ICommandUsable self) where T : ICommand, new() {
         self.GetFactory().SendCommand<T>();
      }

      public static void SendCommand<T>(this ICommandUsable self, T command) where T : ICommand {
         self.GetFactory().SendCommand<T>(command);
      }
   }
}