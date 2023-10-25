using QPFramework;

namespace GDUGame {

   public class GDUGame: Factory<GDUGame> {
      /// <summary>
      /// All GameSystems Register here
      /// </summary>
      protected override void Init() {
         this.SetSystem<IGunSystem>(new GunSystem());
         this.SetSystem<ITimeSystem>(new TimeSystem());

         this.SetModel<IGunModel>(new GunModel());
      }
   }
}