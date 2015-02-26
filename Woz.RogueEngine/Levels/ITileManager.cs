using System;
using System.Collections.Generic;
using System.Drawing;
using Woz.RogueEngine.Entities;

namespace Woz.RogueEngine.Levels
{
    public interface ITileManager 
        : IEnumerable<Tuple<Point, IEntity>>
    {
        Size Size { get; }
        Rectangle Bounds { get; }
        IEntity this[Point location] { get; }

        ITileManager SetTile(Point location, IEntity tile);
        ITileManager EditTile(Point location, Func<IEntity, IEntity> tileEditor);
    }
}