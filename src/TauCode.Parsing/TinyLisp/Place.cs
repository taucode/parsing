using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.TinyLisp
{
    public class Place
    {
        private Element _value = null!; // set by 'SetValue'

        public Place(Element value)
        {
            this.SetValue(value);
        }

        public Element GetValue()
        {
            return _value;
        }

        public void SetValue(Element value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
        }
    }
}
