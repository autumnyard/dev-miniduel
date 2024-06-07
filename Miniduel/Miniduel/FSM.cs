using System;
using System.Collections.Generic;

namespace AutumnYard.Miniduel
{
    [Obsolete("Unnecessary, too convoluted.")]
    public class FSM<T>
        where T : Enum
    {
        private T _state;
        private Dictionary<T, T> _allow;

        public FSM()
        {
            _state = default;
            _allow = new Dictionary<T, T>();
        }

        public void Allow(T from, T to)
        {
            _allow.Add(from, to);
        }

        public bool ChangeTo(T newState)
        {
            if (_allow == null)
                return false;

            if (_allow.ContainsKey(newState))
            {
                if (!_allow[_state].Equals(newState))
                    return false;
            }

            _state = newState;
            return true;
        }

        public bool Is(T state) => _state.Equals(state);
        public bool IsNot(T state) => !_state.Equals(state);
    }
}
