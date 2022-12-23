namespace SujinsInterpreter
{
    public abstract class NodeVisitor
    {
        protected delegate int DelegateMethod(AST node);

        protected dynamic Visit(AST node)
        {
            if (node is BinaryOperator)

                return VisitBinaryOperator((BinaryOperator)node);

            else if (node is UnaryOperator)

                return VisitUnaryOperator((UnaryOperator)node);

            else if (node is Num)

                return VisitNum((Num)node);

            else if (node is Cadene)

                return VisitCadene((Cadene)node);
            
            else if (node is CardsList)

                return VisitCardsList((CardsList)node);

            else if (node is Instructions)

                return VisitInstructions((Instructions)node);

            else if (node is Declarations)

                return VisitDeclarations((Declarations)node);

            else if (node is Assign)

                return VisitAssign((Assign)node);

            else if (node is Condition)

                return VisitCondition((Condition)node);

            else if (node is Cicle)

                return VisitCicle((Cicle)node);

            else if (node is Functions)

                return VisitFunctions((Functions)node);

            else if (node is Var)

                return VisitVar((Var)node);

            else if (node is VarDecl)

                return VisitVarDecl((VarDecl)node);

            else if (node is Bool)

                return VisitBool((Bool)node);

            else if (node is TypeVar)

                return VisitType((TypeVar)node);

            else if (node is Empty)

                return VisitEmpty((Empty)node);

            return null;
        }

        public abstract dynamic VisitBinaryOperator(BinaryOperator node);
        public abstract dynamic VisitUnaryOperator(UnaryOperator node);
        public abstract dynamic VisitCardsList(CardsList node);
        public abstract dynamic VisitInstructions(Instructions node);
        public abstract dynamic VisitDeclarations(Declarations node);
        public abstract dynamic VisitAssign(Assign node);
        public abstract dynamic VisitCondition(Condition node);
        public abstract dynamic VisitCicle(Cicle node);
        public abstract dynamic VisitFunctions(Functions node);
        public abstract dynamic VisitVar(Var node);
        public abstract dynamic VisitVarDecl(VarDecl node);
        public abstract dynamic VisitNum(Num node);
        public abstract dynamic VisitBool(Bool node);
        public abstract dynamic VisitCadene(Cadene node);
        public abstract dynamic VisitType(TypeVar node);
        public abstract dynamic VisitEmpty(Empty node);
    }
}