using QPFramework;

namespace GDUGame {

   public class GDUGame: Factory<GDUGame> {

      protected override void Init() {
         this.SetSystem<IGunSystem>(new GunSystem());
      }
   }
}