using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xchange.Interfaces;

namespace Xchange.Stream
{
    public class Reader : IReader
    {
        private System.IO.FileStream _InnerFileStream;
        private IErrorHandler _ErrorHandler;

        public Reader(IErrorHandler error)
        {
            _ErrorHandler = error;
        }

        public long Position
        {
            get
            {
                return _InnerFileStream.Position;
            }
        }

        public void Init(string filepath)
        {
            try
            {
                _InnerFileStream = new System.IO.FileStream(filepath, FileMode.Open);
            }
            catch (Exception)
            {
                _ErrorHandler.Error($"Failed to open file: {filepath}");
            }
        }

        public int ReadByte()
        {
            return _InnerFileStream.ReadByte();
        }

        public long Seek(long offset)
        {
            return _InnerFileStream.Seek(offset, SeekOrigin.Begin);
        }
    }
}
