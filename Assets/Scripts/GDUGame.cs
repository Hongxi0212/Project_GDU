using QFramework;

namespace GDUGame {
   public class GDUGame: ContainerSetter<GDUGame> {
      protected override void Init() {
         this.RegisterSystem<IInputSystem>(new InputSystem());
      }
   }
}