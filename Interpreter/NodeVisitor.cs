namespace SujinsInterpreter
{
    /// <summary>
    /// Redirecciona al método correspondiente para procesar el tipo de instrucción correspondiente.
    /// </summary>
    public abstract class NodeVisitor
    {
        /// <summary>
        /// Se encarga de procesar las visitas.
        /// </summary>
        /// <remarks>
        /// A traves de las visitas se va recorriendo el AST para procesar el código.
        /// </remarks>
        /// <param name="node">
        /// Representa el tipo de nodo a procesar.
        /// </param>
        /// <returns></returns>
        protected dynamic Visit(AST node)
        {
           if (node is BinaryOperator)  // Retorna un nodo asociado a una `operación binaria`.

                return VisitBinaryOperator((BinaryOperator)node);

            else if (node is UnaryOperator) // Retorna un nodo asociado a una `operación unaria`.

                return VisitUnaryOperator((UnaryOperator)node);

            else if (node is Num)   // Retorna un nodo asociado a un `número`.

                return VisitNum((Num)node);

            else if (node is Cadene)    // Retorna un nodo asociado a una `cadena de letras`.

                return VisitCadene((Cadene)node);
            
            else if (node is CardsList) // Retorna un nodo asociado a una `lista de cartas mágicas`.

                return VisitCardsList((CardsList)node);

            else if (node is Instructions)  // Retorna un nodo asociado a una `lista de instrucciones`.

                return VisitInstructions((Instructions)node);

            else if (node is Declarations)  // Retorna un nodo asociado a un `conjunto de declaraciones de variables`.

                return VisitDeclarations((Declarations)node);

            else if (node is Assign)    // Retorna un nodo asociado a una `asignación de variables`.

                return VisitAssign((Assign)node);

            else if (node is Condition) // Retorna un nodo asociado a una `condición`.

                return VisitCondition((Condition)node);

            else if (node is Cicle)     // Retorna un nodo asociado a un `ciclo`.

                return VisitCicle((Cicle)node);

            else if (node is Functions) // Retorna un nodo asociado a una `llamada a una función predeclarada`.

                return VisitFunctions((Functions)node);

            else if (node is Var)   // Retorna un nodo asociado al valor de una variable.

                return VisitVar((Var)node);

            else if (node is VarDecl)   // Retorna un nodo asociado a una `declaración de las variables`.

                return VisitVarDecl((VarDecl)node);

            else if (node is Bool)  // Retorna un nodo asociado al valor de un booleano.

                return VisitBool((Bool)node);

            else if (node is TypeVar)   // Retorna un nodo asociado a un tipo de variable.

                return VisitType((TypeVar)node);

            else if (node is Empty)     // Retorna un nodo asociado a una operación vacía.

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