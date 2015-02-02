
namespace Woz.RogueEngine.Entities
{
    public class Field<T> 
        where T : struct
    {
        private readonly string _name;
        private readonly T _value;

        public Field(string name, T value)
        {
            _name = name;
            _value = value;
        }

        public string Name
        {
            get { return _name; }
        }

        public T Value
        {
            get { return _value; }
        }

        public virtual Field<T> Set(T value)
        {
            return new Field<T>(_name, value);
        }
    }
}
