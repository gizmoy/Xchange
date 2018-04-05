using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xchange.Semantic.Interfaces;

namespace Xchange.Semantic.Entities
{
    public class Literal : IExpressionOperand, IConditionOperand
    {
        public decimal Data { get; set; }
        public bool CastedToBool { get; set; } = false;
        public bool LoopJump { get; set; } = false;
        public bool IsBreak { get; set; } = false;

        public Literal Execute(ScopeInstance scope, IDictionary<string, Function> functions)
        {
            Literal copy = new Literal();
            copy.Data = Data;

            return copy;
        }

        public bool IsTruthy()
        {
            return Data == 1.0m;
        }

        public static bool operator ==(Literal first, Literal second)
        {
            if (ReferenceEquals(first, null))
            {
                return ReferenceEquals(second, null);
            }

            return first.Equals(second);
        }


        public static bool operator !=(Literal first, Literal second)
        {
            return !(first == second);
        }

        public override bool Equals(object obj)
        {
            var item = obj as Literal;

            if (item == null)
            {
                return false;
            }

            return Data == item.Data &&
                   CastedToBool == item.CastedToBool &&
                   LoopJump == item.LoopJump &&
                   IsBreak == item.IsBreak;
        }

        public override int GetHashCode()
        {
            // Overflow is fine, just wrap
            unchecked
            {
                int hash = 17;

                // Suitable nullity checks etc, of course :)
                hash = hash * 23 + Data.GetHashCode();
                hash = hash * 23 + CastedToBool.GetHashCode();
                hash = hash * 23 + LoopJump.GetHashCode();
                hash = hash * 23 + IsBreak.GetHashCode();

                return hash;
            }
        }

        public static bool operator <(Literal first, Literal second)
        {

            return first.Data < second.Data;
        }

        public static bool operator <=(Literal first, Literal second)
        {
            return first.Data <= second.Data;
        }


        public static bool operator >(Literal first, Literal second)
        {
            return first.Data > second.Data;
        }


        public static bool operator >=(Literal first, Literal second)
        {
            return first.Data >= second.Data;
        }

        public static Literal operator +(Literal first, Literal second)
        {
            first.Data += second.Data;

            return first;
        }

        public static Literal operator -(Literal first, Literal second)
        {
            first.Data -= second.Data;

            return first;
        }

        public static Literal operator *(Literal first, Literal second)
        {
            first.Data *= second.Data;

            return first;
        }

        public static Literal operator /(Literal first, Literal second)
        {
            first.Data /= second.Data;

            return first;
        }

        public static Literal operator %(Literal first, Literal second)
        {
            first.Data %= second.Data;

            return first;
        }
    }
}
