using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xchange.Interfaces;

namespace Xchange.Stream
{
    public class Source : ISource
    {
        private IReader _Reader;
        private IErrorHandler _ErrorHandler;

        private char _PreviousCharacter;

        public long CurrentLine { get; private set; }
        public long CurrentColumn { get; private set; }
        public long CurrentPosition { get { return _Reader.Position; } }
        public long CurrentLinePosition { get; private set; }
        public bool HasFinished { get { return _PreviousCharacter == EOF; } }

        const char EOF = '\uffff';

        public Source(IReader reader, IErrorHandler errorHandler)
        {
            _Reader = reader;
            _ErrorHandler = errorHandler;
            _PreviousCharacter = (char)0;
            CurrentLine = 1;
            CurrentColumn = 0;
            CurrentLinePosition = 0;
        }


        public char GetCharacter()
        {
            char character;

            while (true)
            {
                character = (char)_Reader.ReadByte();

                if (character == '\n' || character == '\r')
                {
                    if (_PreviousCharacter != character && (_PreviousCharacter == '\n' || _PreviousCharacter == '\r'))
                    {
                        // CRLF or LFCR; skip it
                    }
                    else
                    {
                        CurrentLine += 1;
                        CurrentColumn = 0;
                        CurrentLinePosition = CurrentPosition+3;
                    }

                    _PreviousCharacter = character;
                    continue;
                }

                _PreviousCharacter = character;
                CurrentColumn += 1;

                return character;
            }
        }

        public void UngetCharacter()
        {
            _Reader.Seek(CurrentPosition - 2);
            _PreviousCharacter = (char)_Reader.ReadByte();

            var peek = (char)_Reader.ReadByte();
            _Reader.Seek(CurrentPosition - 1);

            if (peek == '\n' || peek == '\r')
            {
                _PreviousCharacter = (char)_Reader.ReadByte();
                return;
            }
            
            CurrentColumn -= 1;
        }

        public string GetLine(long linePosition)
        {
            // Seek on line position
            var lastPosition = _Reader.Position;
            _Reader.Seek(linePosition);

            // Build line
            var character = (char)_Reader.ReadByte();
            var sb = new StringBuilder();

            while (character != '\n')
            {
                sb.Append(character);

                if (character == EOF)
                {
                    break;
                }

                character = (char)_Reader.ReadByte();
            }

            // Recover previous position
            _Reader.Seek(lastPosition);

            // Remove all tabs
            var tab = '\u0009';
            var line = sb
                .Replace(tab.ToString(), " ")
                .Insert(0, tab)
                .ToString();

            return sb.ToString();
        }
    }
}
