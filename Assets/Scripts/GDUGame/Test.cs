using GDUGame;
using UnityEngine;

/// <summary>
/// Used for Test
/// Allow any Modify
/// </summary>
public class Test: GDUController {

   private void Start() {
      TestInputSystem();
   }

   public void TestInputSystem() {
      //this.RegisterEvent(InputInfo.Keyboard_A, TestSuccessful);
   }

   private void TestSuccessful() {
      Debug.Log("Test Successful!");
   }
}