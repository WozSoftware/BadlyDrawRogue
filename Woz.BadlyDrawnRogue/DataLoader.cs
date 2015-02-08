using System;
using System.Xml.Linq;
using Woz.Functional.Error;
using Woz.RogueEngine.Definitions;
using Woz.RogueEngine.Entities;

namespace Woz.BadlyDrawnRogue
{
    public static class DataLoader
    {
        public static IEntityFactory LoadEntityFactory(string fileName)
        {
            return fileName
                .ToSuccees()
                .TrySelect(XDocument.Load)
                .TrySelect(document => document.Root)
                .TrySelect(EntityParser.ReadEntities)
                .TrySelect(EntityFactory.Build)
                .Return(message =>
                    new Exception(
                        string.Format(
                            "Failed to load data file {0}: {1}",
                            fileName, message)));
        }
    }
}