using System.Collections.Generic;
using Assets.Scripts.Command;

public delegate void DispatcherCallback<T>(T c) where T : ICommand;

/// <summary>
/// Classe qui permet de souscrire et de publier des commandes
/// </summary>
public sealed class CommandDispatcher
{

    private static CommandDispatcher instance;

    /// <summary>
    /// Singleton 
    /// </summary>
    public static CommandDispatcher Instance { get {
            if (instance == null) {
                instance = new CommandDispatcher();
            }
            return instance;
        } 
    }

    private Dictionary<System.Type, System.Delegate> subscribers = new Dictionary<System.Type, System.Delegate>();

    /// <summary>
    /// Méthode permettant de souscrire à une commande
    /// </summary>
    /// <typeparam name="T">Le type de la commande</typeparam>
    /// <param name="callback">Méthode à exécuter après qu'une commande ait été envoyée</param>
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

    /// <summary>
    /// Méthode permettant de supprimer un subscriber
    /// </summary>
    /// <typeparam name="T">Type de la commande</typeparam>
    /// <param name="callback">Méthode à retirer</param>
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

    /// <summary>
    /// Méthode permettant de publier une commande
    /// </summary>
    /// <typeparam name="T">Type de la commande</typeparam>
    /// <param name="command">La commande </param>
    public void Publish<T>(T command) where T : ICommand
    {
        var commandType = typeof(T);
        if (subscribers.ContainsKey(commandType))
        {
            subscribers[commandType].DynamicInvoke(command);
        }
    }

}
