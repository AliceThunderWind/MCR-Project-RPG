using System.Collections.Generic;
using Assets.Scripts.Command;

public delegate void DispatcherCallback<T>(T c) where T : ICommand;

public sealed class CommandDispatcher
{

    private static CommandDispatcher instance;

    public static CommandDispatcher Instance { get {
            if (instance == null) {
                instance = new CommandDispatcher();
            }
            return instance;
        } 
    }

    private Dictionary<System.Type, System.Delegate> subscribers = new Dictionary<System.Type, System.Delegate>();

    public void Subscribe<T>(DispatcherCallback<T> callback) where T : ICommand
    {
        if (callback == null) throw new System.ArgumentNullException("Mediator->Subscribe : Callback");
        var commandType = typeof(T);
        if (subscribers.ContainsKey(commandType))
        {
            subscribers[commandType] = System.Delegate.Combine(subscribers[commandType], callback);
        }
        else
        {
            subscribers.Add(commandType, callback);
        }
    }

    public void DeleteSubscriber<T>(DispatcherCallback<T> callback) where T : ICommand
    {
        if (callback == null) throw new System.ArgumentNullException("Mediator->DeleteSubscriber : Callback");
        var commandType = typeof(T);
        if (subscribers.ContainsKey(commandType))
        {
            var sub = subscribers[commandType];
            sub = System.Delegate.Remove(sub, callback);
            if (sub == null) { 
                subscribers.Remove(commandType);
            }
            else
            {
                subscribers[commandType] = sub;
            }
        }
    }

    public void Publish<T>(T command) where T : ICommand
    {
        var commandType = typeof(T);
        if (subscribers.ContainsKey(commandType))
        {
            subscribers[commandType].DynamicInvoke(command);
        }
    }

}