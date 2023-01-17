using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Entity
{

    public abstract bool CanBeAttacked();

    public abstract bool IsEnemy();

    public abstract bool canAttack();

    public abstract void Damage(int damage, Entity sender);

    public abstract int GetMaxMoveDistance();

    public abstract Health GetHealth();

}
