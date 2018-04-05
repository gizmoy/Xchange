using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xchange.Interfaces;

namespace Tests
{
    public class StringReader : IReader
    {
        private string _Program;
        private long _Position;

        public StringReader()
        {
            // empty
        }

        public long Position
        {
            get
            {
                return _Position;
            }
        }

        public void Init(string program)
        {
            _Program = program;
            _Position = 0;
        }

        public int ReadByte()
        {
            var character = (_Program.Length == Position)
                ? '\uffff'
                : _Program[(int)_Position];

            _Position += 1;

            return character;
        }

        public long Seek(long offset)
        {
            _Position = offset;
            return _Position;
        }
    }
}
