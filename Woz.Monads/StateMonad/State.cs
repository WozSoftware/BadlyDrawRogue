#region License
// Copyright (C) Woz.Software 2015
// [https://github.com/WozSoftware/BadlyDrawRogue]
//
// This file is part of Woz.Monads.
//
// Woz.Linq is free software: you can redistribute it 
// and/or modify it under the terms of the GNU General Public 
// License as published by the Free Software Foundation, either 
// version 3 of the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
#endregion

using System;
using System.Diagnostics;

namespace Woz.Monads.StateMonad
{
    public delegate StateResult<TState, T> State<TState, T>(TState state);

    public static class State
    {
        public static State<TState, T> ToState<TState, T>(this T value)
        {
            return state => StateResult.Create(state, value);
        }
 
        public static State<TState, TState> Get<TState>()
        {
            return state => StateResult.Create(state, state);
        }

        public static State<TState, Unit> Put<TState>(TState state)
        {
            return _ => StateResult.Create(state, Unit.Value);
        }

        public static State<TState, Unit> Modify<TState>(Func<TState, TState> operation)
        {
            Debug.Assert(operation != null);

            return state => StateResult.Create(operation(state), Unit.Value);
        }

        public static StateResult<TState, T> 
            Run<TState, T>(this State<TState, T> self, TState state)
        {
            Debug.Assert(self != null);

            return self(state);
        }

        public static TState Exec<TState, T>(this State<TState, T> self, TState state)
        {
            Debug.Assert(self != null);

            return self(state).State;
        }

        public static T Eval<TState, T>(this State<TState, T> self, TState state)
        {
            Debug.Assert(self != null);

            return self(state).Value;
        }

        // M<T> -> Func<T, TResult> -> M<TResult>
        public static State<TState, TResult> Select<TState, T, TResult>(
            this State<TState, T> self, Func<T, TResult> operation)
        {
            Debug.Assert(self != null);
            Debug.Assert(operation != null);

            return state =>
            {
                var stateResult = self(state);
                return StateResult.Create(
                    stateResult.State,
                    operation(stateResult.Value));
            };
        }

        // M<T> -> Func<T, M<TResult>> -> M<TResult>
        public static State<TState, TResult> 
            SelectMany<TState, T, TResult>(
                this State<TState, T> self,
                Func<T, State<TState, TResult>> operation)
        {
            Debug.Assert(self != null);
            Debug.Assert(operation != null);

            return state =>
            {
                var stateResult = self(state);
                return operation(stateResult.Value)(stateResult.State);
            };
        }

        // M<T1> -> Func<T1, M<T2>> -> Func<T1, T2, TResult> -> M<TResult>
        public static State<TState, TResult> 
            SelectMany<TState, T1, T2, TResult>(
                this State<TState, T1> self,
                Func<T1, State<TState, T2>> transform,
                Func<T1, T2, TResult> composer)
        {
            Debug.Assert(self != null);
            Debug.Assert(transform != null);
            Debug.Assert(composer != null);

            return self.SelectMany(
                v1 => transform(v1).SelectMany(
                    v2 => composer(v1, v2).ToState<TState, TResult>()));
        }
    }
}