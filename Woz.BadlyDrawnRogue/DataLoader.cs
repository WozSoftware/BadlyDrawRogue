#region License
// Copyright (C) Woz.Software 2015
// [https://github.com/WozSoftware/BadlyDrawRogue]
//
// This file is part of Woz.BadlyDrawnRogue.
//
// Woz.BadlyDrawnRogue is free software: you can redistribute it 
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
using Woz.Functional.Monads.IOMonad;
using Woz.Linq.Xml;
using Woz.RogueEngine.Definitions;
using Woz.RogueEngine.Entities;

namespace Woz.BadlyDrawnRogue
{
    public static class DataLoader
    {
        public static IEntityFactory LoadEntityFactory(string uri)
        {
            return XDocumentIO.Load(uri)
                .Select(document => document.Root.ReadEntities())
                .Select(EntityFactory.Build)
                .Run()
                .OrElse(
                    ex =>
                    {
                        var message = string.Format(
                            "Failed to load data file {0}: {1}",
                            uri, ex.Message);
                       
                        return new Exception(message, ex);
                    });
        }
    }
}