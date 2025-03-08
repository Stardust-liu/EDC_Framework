using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface ICommand{
    void Execute();
}
public abstract class BaseCommand : ICommand, ISendCommand
{
    void ICommand.Execute(){
        Execute();
    }
    protected abstract void Execute();
}
