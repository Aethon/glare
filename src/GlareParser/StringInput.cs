namespace Aethon.GlareParser
{
    public sealed class StringInput: Input<char>
    {
        private readonly string _data;
        private int _index;

        public StringInput(string data)
        {
            _data = data;
        }

        protected override (bool, char) GetNext()
        {
            return (_index >= _data.Length)
                ? (true, default)
                : (false, _data[_index++]);
        }
    }
}