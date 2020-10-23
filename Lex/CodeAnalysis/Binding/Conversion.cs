using Lex.CodeAnalysis.Symbols;

namespace Lex.CodeAnalysis.Binding
{
    internal sealed class Conversion 
   {
       public static  readonly Conversion None = new Conversion(exists: false, isIdentity: false, isImplicit: false);
       public static  readonly Conversion Identity = new Conversion(exists: true, isIdentity: true, isImplicit: true);
       public static  readonly Conversion Implicit = new Conversion(exists: true, isIdentity: false, isImplicit: true);
       public static  readonly Conversion Explicit = new Conversion(exists: true, isIdentity: false, isImplicit: false);
       public Conversion(bool exists, bool isIdentity, bool isImplicit)
       {
            Exist = exists;
            IsIdentity = isIdentity;
            IsImplicity = isImplicit;
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
                    return Conversion.Explicit;
            }

            return Conversion.None;
        
       }
   }
}