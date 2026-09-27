using UnityEngine;

public abstract class EnemyStates
{
    protected readonly EnemyWitch enemy;
    protected EnemyStates(EnemyWitch e)
    {
        enemy = e;
    }
    public virtual void OnEnter() { }
   public abstract void OnUpdate();
    public virtual void OnFixedUpdate() { }
    public virtual void OnExit() { }
    public virtual void OnSendMessage(object o) { }
}