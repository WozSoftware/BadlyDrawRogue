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
namespace Woz.Monads.StateMonad
{
    public static class StateResult
    {
        public static StateResult<TState, TValue>
            Create<TState, TValue>(TState state, TValue value)
        {
            return new StateResult<TState, TValue>(state, value);
        }
    }

    public class StateResult<TState, TValue>
    {
        private readonly TState _state;
        private readonly TValue _value;

        internal StateResult(TState state, TValue value)
        {
            _state = state;
            _value = value;
        }

        public TState State
        {
            get { return _state; }
        }

        public TValue Value
        {
            get { return _value; }
        }
    }
}