using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.Utils;
using UnityEngine;
namespace DTWorldz.Interfaces
{
    public delegate void HealthChanged(float currentHealth, float maxHealth);
    public interface IHealth
    {
        event HealthChanged OnHealthChanged;
        float CurrentHealth { get; set; }
        float MaxHealth { get; set; }
    }
}