using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xchange.Interfaces
{
    public interface IReader
    {
        long Position { get; }

        void Init(string filepath);
        long Seek(long offset);
        int ReadByte();
    }
}
