using UnityEngine;

namespace Interfaces
{
    public interface IIndicatorProvider 
    {
        public float GetValue();
    }
    public interface IDamagable
    {
        public void TakeDamage(int damageCount);
    }
    public interface ITutorialBlock 
    { 
        public void NextBlock();
        public void ActivateBlock();
    }

}
