﻿#region License
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
using System.Xml.Linq;
using Woz.Functional.Try;
using Woz.RogueEngine.Definitions;
using Woz.RogueEngine.Entities;

namespace Woz.BadlyDrawnRogue
{
    public static class DataLoader
    {
        public static IEntityFactory LoadEntityFactory(string fileName)
        {
            return fileName
                .ToSuccess()
                .Select(XDocument.Load)
                .Select(document => document.Root)
                .Select(EntityParser.ReadEntities)
                .Select(EntityFactory.Build)
                .OrElse(message =>
                    new Exception(
                        string.Format(
                            "Failed to load data file {0}: {1}",
                            fileName, message)));
        }
    }
}