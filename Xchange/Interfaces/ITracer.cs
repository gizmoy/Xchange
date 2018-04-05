using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xchange.Interfaces
{
    public interface ITracer
    {
        bool Enabled { get; set; }
        void Reset();
        void Enter(string message = "");
        void Leave(string message = "leave");
        void Info(string message);
        void PrintNesting();
    }
}
