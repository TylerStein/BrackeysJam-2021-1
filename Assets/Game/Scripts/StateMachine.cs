using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArchSpace.Utilities
{
    [System.Serializable]
    public class StateMachine
    {
        private Dictionary<string, State> _states;
        [SerializeField] private State _activeState;

        public StateMachine() {
            _states = new Dictionary<string, State>();
            _activeState = null;
        }

        public void Command(string command) {
            if (_activeState != null) {
                _activeState.Command(this, command);
            }
        }

        public void AddState(State state) {
            if (_states.ContainsKey(state.stateName) == true) return;
            _states.Add(state.stateName, state);
        }

        public void RemoveState(State state) {
            _states.Remove(state.stateName);
        }

        public bool StartState(string stateName) {
            if (_states.ContainsKey(stateName) == false) return false;

            string lastStateName = "";
            if (_activeState != null) {
                lastStateName = _activeState.stateName;
                _activeState.Exit(this, stateName);
            }

            _activeState = _states[stateName];
            _activeState.Enter(this, lastStateName);

            return true;
        }

        public bool UpdateActiveState() {
            if (_activeState == null) return false;
             _activeState.Update(this);
            return true;
        }

        public bool ExitState() {
            if (_activeState == null) return false;
            _activeState.Exit(this, null);
            _activeState = null;
            return true;
        }
    }

    [System.Serializable]
    public class State
    {
        public string stateName;
        private System.Action<StateMachine, string> _onEnter;
        private System.Action<StateMachine> _onUpdate;
        private System.Action<StateMachine, string> _onExit;

        public Dictionary<string, string> transitions;

        public State(string name, System.Action<StateMachine> updateFunc = null, System.Action<StateMachine, string> entryFunc = null, System.Action<StateMachine, string> exitFunc = null) {
            stateName = name;
            _onEnter = entryFunc;
            _onExit = exitFunc;
            _onUpdate = updateFunc;
            transitions = new Dictionary<string, string>();
        }


        public void AddTransition(string command, string state) {
            if (transitions.ContainsKey(command) == false) {
                transitions.Add(command, state);
            }
        }

        public void Command(StateMachine stateMachine, string command) {
            if (transitions.ContainsKey(command) == true) {
                stateMachine.StartState(transitions[command]);
            }
        }

        public void Enter(StateMachine stateMachine, string lastState) {
            if (_onEnter != null) _onEnter(stateMachine, lastState);
        }

        public void Exit(StateMachine stateMachine, string nextState) {
            if (_onExit != null) _onExit(stateMachine, nextState);
        }

        public void Update(StateMachine stateMachine) {
            if (_onUpdate != null) _onUpdate(stateMachine);
        }
    }

    [System.Serializable]
    public class StateTransition
    {
        public string commandName;
        public string stateName;
    }
}