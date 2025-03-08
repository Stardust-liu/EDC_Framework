using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISendCommand{}
public static class ISendCommandExtension{
    public static void SendCommand<T>(this ISendCommand commmand) 
    where T : BaseCommand, new()
    {
        ((ICommand)new T()).Execute();
    }
}
