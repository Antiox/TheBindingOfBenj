using System;
using System.Collections.Generic;

namespace GameLibrary
{
    public class EventManager
    {
        private readonly Dictionary<Type, Action<IGameEvent>> events = new Dictionary<Type, Action<IGameEvent>>();
        private readonly Dictionary<Delegate, Action<IGameEvent>> callbacks = new Dictionary<Delegate, Action<IGameEvent>>();


        #region Singleton
        private static EventManager instance;
        public static EventManager Instance
        {
            get
            {
                instance ??= new EventManager();
                return instance;
            }
        }
        private EventManager() { }
        #endregion


        /// <summary>
        /// Permet à une classe de s'abonner à un événement
        /// </summary>
        /// <typeparam name="T">Type d'événément auquel la classe s'abonne</typeparam>
        /// <param name="e"></param>
        public void AddListener<T>(Action<T> e) where T : IGameEvent
        {
            if (!callbacks.ContainsKey(e))
            {
                void action(IGameEvent a) => e((T)a);
                callbacks[e] = action;

                if (events.TryGetValue(typeof(T), out var internalAction))
                    events[typeof(T)] = internalAction += action;
                else
                    events[typeof(T)] = action;
            }
        }

        /// <summary>
        /// Permet de se désabonner d'un événement
        /// </summary>
        /// <typeparam name="T">Type d'événement</typeparam>
        /// <param name="e"></param>
        public void RemoveListener<T>(Action<T> e) where T : IGameEvent
        {
            if (callbacks.TryGetValue(e, out var action))
            {
                if (events.TryGetValue(typeof(T), out var tempAction))
                {
                    tempAction -= action;
                    if (tempAction == null)
                        events.Remove(typeof(T));
                    else
                        events[typeof(T)] = tempAction;
                }

                callbacks.Remove(e);
            }
        }

        /// <summary>
        /// Permet d'indiquer à l'EventManager qu'il faut notifier les classes abonnées à l'événement e
        /// </summary>
        /// <param name="e"></param>
        public void Dispatch(IGameEvent e)
        {
            if (events.TryGetValue(e.GetType(), out var action))
                action.Invoke(e);
        }

        /// <summary>
        /// Vidage complet des abonnements
        /// </summary>
        public void Clear()
        {
            events.Clear();
            callbacks.Clear();
        }
    }
}
