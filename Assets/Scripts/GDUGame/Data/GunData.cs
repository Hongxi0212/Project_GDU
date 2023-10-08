using QPFramework;

namespace GDUGame {
   /// <summary>
   /// Record the Data of Gun when player enter a level
   /// </summary>
   public class GunData {
      public BindableProperty<int> BulletCount;

      public BindableProperty<int> SpareRoundsCount;
   }
}