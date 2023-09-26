using System;
using UnityEngine;

public interface IBaseState
{
    public bool IsWaiting{ get; set; }
    [Tooltip("The UpdateRate is in Millisecond")]
    public int UpdateRate { get; set; }
    public abstract void Tick();
    public virtual void OnEnter() { }
    public virtual void OnExit() { }
}

public interface IBaseStateWithName<TEnum>: IBaseState where TEnum : Enum,  IComparable, IFormattable, IConvertible
{
    public TEnum StateName { get; set; }
}
