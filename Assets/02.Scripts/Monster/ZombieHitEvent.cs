using UnityEngine;

public class ZombieHitEvent : MonoBehaviour
{
   [SerializeField]private Zombie _zombie;



   public void HitEvent()
   {
       _zombie.CalculateAttackDamage();
   }
}
