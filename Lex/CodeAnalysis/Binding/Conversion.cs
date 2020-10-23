using Lex.CodeAnalysis.Symbols;

namespace Lex.CodeAnalysis.Binding
{
    internal sealed class Conversion 
   {
       public Conversion(bool exist, bool isIdentity, bool isImplicity)
       {
            Exist = exist;
            IsIdentity = isIdentity;
            IsImplicity = isImplicity;
        }

        public bool Exist { get; }
        public bool IsIdentity { get; }
        public bool IsImplicity { get; }
        public bool IsExplictic => Exist && !IsImplicity;

        public static Conversion Classify(TypeSymbol from,TypeSymbol to)
       {
           //Identity
           if(from == to)
            return Conversion.Identity;

            if(from == TypeSymbol.Bool || from == TypeSymbol.Int)
            {
                if(to == TypeSymbol.String)
                    return Conversion.Explicty;
            }

            return Conversion.None;
        
       }
   }
}