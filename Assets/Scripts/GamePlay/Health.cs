using UnityEngine;
using UnityEngine.Events;

public class Health
{
    public int currentHealth;
    public UnityEvent OnHealthDamaged;
    public UnityEvent OnHealthIncreased;

    public void DecreaseLife()
    {
        if (currentHealth > 0)
        {
            currentHealth--;
            OnHealthDamaged.Invoke();
        }
    }

    public void DecreaseLife(int damage)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damage;
            if (currentHealth < 0)
            {
                currentHealth = 0;
            }
            OnHealthDamaged.Invoke();
        }
    }

    public void IncreaseLife(int increase)
    {
        currentHealth += increase;
        OnHealthIncreased.Invoke();
    }

    public Health(int maxHealth)
    {
        currentHealth = maxHealth;
        OnHealthDamaged = new UnityEvent();
        OnHealthIncreased = new UnityEvent();
    }
}
