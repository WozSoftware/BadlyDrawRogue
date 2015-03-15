using System.Drawing;
using Woz.Functional.Monads.ValidationMonad;
using Woz.RogueEngine.Entities;
using Woz.RogueEngine.Levels;
using Woz.RogueEngine.Rules;

namespace Woz.RogueEngine.Validators
{
    public static class LevelValidators
    {
        public static IValidation<ILevel> CanSpawnActor(
            this ILevel level, IEntity actor, Point location)
        {
            return Validator
                .For(level)
                .SelectMany(x => x.RuleIsValidLocation(location))

        }
    }
}