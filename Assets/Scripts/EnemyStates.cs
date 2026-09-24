using UnityEngine;

public abstract class EnemyStates
{
    protected readonly Enemy enemy;
    protected EnemyStates(Enemy e)
    {
        enemy = e;
    }
    public virtual void OnEnter() { }
   public abstract void OnUpdate();
    public virtual void OnFixedUpdate() { }
    public virtual void OnExit() { }
    public virtual void OnSendMessage(object o) { }
}