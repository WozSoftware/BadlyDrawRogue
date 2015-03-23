using System.Drawing;
using Woz.Monads.ValidationMonad;
using Woz.RogueEngine.Levels;
using Woz.RogueEngine.Rules;

namespace Woz.RogueEngine.Validators
{
    public static class LevelValidators
    {
        public static IValidation<Level> IsNewActor(
            this Level level, long actorId)
        {
            return
                !level.HasActorState(actorId)
                    ? level.ToValid()
                    : string.Format(
                        "Actor Id={0} already exists",
                        actorId).ToInvalid<Level>();
        }

        public static IValidation<Level> ActorStateExists(
            this Level level, long actorId)
        {
            return
                level.HasActorState(actorId)
                    ? level.ToValid()
                    : string.Format(
                        "Unknow actor Id={0}",
                        actorId).ToInvalid<Level>();

        }

        public static IValidation<Level> IsValidLocation(
            this Level level, Point location)
        {
            return level
                .Tiles.IsValidLocation(location)
                    ? level.ToValid()
                    : string.Format(
                        "Location {0} is outside the map",
                        location).ToInvalid<Level>();
        }
    }
}