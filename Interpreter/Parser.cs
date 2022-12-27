using System;
using System.Reflection;

namespace SujinsInterpreter
{
    /// <summary>
    /// Clase encargada de parsear y cada instrucción y verificar que cumpla con
    /// las sintaxis requerida en cada caso.
    /// </summary>
    public class Parser
    {
        private Lexer Lexer;    // estructura que me permite acceder a cada token presente en el código.
        private Token CurrentToken; // Token actual

        /// <summary>
        /// Constructor de la clase.
        /// </summary>
        /// <param name="lexer">
        /// Instancia del lexer formado por el código correspondiente.
        /// </param>
        public Parser(Lexer lexer)
        {
            Lexer = lexer;
            CurrentToken = lexer.GetNextToken();
        }

        /// <summary>
        /// Retorna un error de sintaxis en caso de que este ocurra mostrando la línea donde ocurrió dicho error.
        /// </summary>
        /// <param name="error">
        /// Texto alternativo que describe el tipo de error asociado.
        /// </param>
        private void Error(string error = "Error de sintaxis")
        {
            throw new Exception($"[Line {Lexer.Line + 1}]: " + error);
        }

        /// <summary>
        /// Verifica que el token actual sea igual al esperado para cumplir con la gramática
        /// de la sintaxis y pasa al siguiente token. en caso contrario lanza una excepción.
        /// </summary>
        /// <param name="type">
        /// Token esperado.
        /// </param>
        private void Process(TokenTypes type)
        {
            if (CurrentToken.Type == type)

                CurrentToken = Lexer.GetNextToken();

            else

                Error($"Sintaxis Error: Se esperaba '{type.ToString()}' y se obtuvo '{CurrentToken.Type.ToString()}'.");
        }

        /// <summary>
        /// Verifica que cada posible factor cumpla con las sintaxis requeridas.
        /// </summary>
        /// <return>
        /// Devuelve un nodo que representa los distintos tipos de factores que pueden aparecer.
        /// </return>
        private AST Factor()
        {
            AST node = new AST();
            Token token = new Token(CurrentToken);

            switch (token.Type)
            {
                // Uso de alguna variable
                case TokenTypes.ID:

                    Process(TokenTypes.ID);

                    node = new Var(token);

                    break;

                // Número positivo, factor unario de la forma +5
                case TokenTypes.PLUS:   

                    Process(TokenTypes.PLUS);

                    node = new UnaryOperator(token, Factor());

                    break;

                // Número negativo, factor unario de la forma -5
                case TokenTypes.MINUS: 

                    Process(TokenTypes.MINUS);

                    node = new UnaryOperator(token, Factor());

                    break;

                // Número entero
                case TokenTypes.INTEGER:

                    Process(TokenTypes.INTEGER);

                    node = new Num(token);

                    break;

                // Número de tipo flotante
                case TokenTypes.FLOAT:

                    Process(TokenTypes.FLOAT);

                    node = new Num(token);

                    break;

                // Valor de expreesión booleano.
                case TokenTypes.BOOLEAN:

                    Process(TokenTypes.BOOLEAN);

                    node = new Bool(token);

                    break;

                // Valor de una cadena de caracteres.
                case TokenTypes.STRING:

                    Process(TokenTypes.STRING);

                    node = new Cadene(token);

                    break;

                // Uso de la palabra reservada `True`
                case TokenTypes.TRUE:

                    Process(TokenTypes.TRUE);

                    node = new Bool(new Token(TokenTypes.TRUE, true));

                    break;

                // Uso de la palabra reservada `False`
                case TokenTypes.FALSE:

                    Process(TokenTypes.FALSE);

                    node = new Bool(new Token(TokenTypes.FALSE, false));

                    break;

                // Uso de los paréntesis. Valida que siempre que sea abirto uno tiene que cerrarse de nuevo.
                case TokenTypes.L_PARENT:

                    Process(TokenTypes.L_PARENT);

                    node = Compounds();

                    Process(TokenTypes.R_PARENT);

                    break;
            }

            return node;
        }

        /// <summary>
        /// Verifica que cada posible término cumpla con las sintaxis requeridas.  Un termino esta formdo por uno
        /// o más factores y una operación asociada (*, /, //, %).
        /// </summary>
        /// <returns>
        /// Devuelve un nodo que representa los distintos tipos de nodos que pueden aparecer.
        /// </returns>
        private AST Termine()
        {
            AST node = Factor();
            Token token = new Token(CurrentToken);

            while (CurrentToken.Type == TokenTypes.MULT || CurrentToken.Type == TokenTypes.INTEGER_DIV
                    || CurrentToken.Type == TokenTypes.FLOAT_DIV || CurrentToken.Type == TokenTypes.MOD)
            {
                token = new Token(CurrentToken);

                if (token.Type == TokenTypes.MULT)

                    Process(TokenTypes.MULT);

                else if (token.Type == TokenTypes.INTEGER_DIV)

                    Process(TokenTypes.INTEGER_DIV);

                else if (token.Type == TokenTypes.FLOAT_DIV)

                    Process(TokenTypes.FLOAT_DIV);

                else if (token.Type == TokenTypes.MOD)

                    Process(TokenTypes.MOD);

                node = new BinaryOperator(node, token, Factor());
            }


            return node;
        }

        /// <summary>
        /// Verifica que cada posible expresión cumpla con las sintaxis requeridas. Una expresión
        /// está formada por uno o más terminos separados por operaciones (+, -)
        /// </summary>
        /// <returns>
        /// Devuelve un nodo que representa los distintos tipos de nodos que pueden aparecer.
        /// </returns>
        private AST Expression()
        {
            AST node = Termine();
            Token token = new Token(CurrentToken);

            while (CurrentToken.Type == TokenTypes.PLUS || CurrentToken.Type == TokenTypes.MINUS)
            {
                token = new Token(CurrentToken);

                if (token.Type == TokenTypes.PLUS)

                    Process(TokenTypes.PLUS);

                else if (token.Type == TokenTypes.MINUS)

                    Process(TokenTypes.MINUS);

                node = new BinaryOperator(node, token, Termine());
            }

            return node;
        }

        /// <summary>
        /// Comprueba que el token actual sea un símbolo de comparación.
        /// </summary>
        /// <returns>
        /// Verdadero si el token es de tipo comparación y falso en caso contrario.
        /// </returns>
        private bool IsBooleanOperator()
        {
            Token type = CurrentToken;

            return type.Type == TokenTypes.SAME || type.Type == TokenTypes.DIFFERENT ||
                    type.Type == TokenTypes.LESS || type.Type == TokenTypes.GREATER ||
                    type.Type == TokenTypes.LESS_EQUAL || type.Type == TokenTypes.GREATER_EQUAL ||
                    type.Type == TokenTypes.NOX;
        }

        /// <summary>
        /// Verifica que el nodo represente una expresión de comparación. Una expresión de comparación está
        /// compuesto por uno o más expresiones separadas por símbolos de comparación.
        /// </summary>
        /// <returns>
        /// Devuelve un nodo que representa los distintos tipos de nodos que pueden aparecer.
        /// </returns>
        private AST Comparer()
        {
            AST node = Expression();
            Token token = new Token(CurrentToken);

            while (IsBooleanOperator())
            {
                token = new Token(CurrentToken);

                if (token.Type == TokenTypes.SAME)

                    Process(TokenTypes.SAME);

                else if (token.Type == TokenTypes.DIFFERENT)

                    Process(TokenTypes.DIFFERENT);

                else if (token.Type == TokenTypes.LESS)

                    Process(TokenTypes.LESS);

                else if (token.Type == TokenTypes.LESS_EQUAL)

                    Process(TokenTypes.LESS_EQUAL);

                else if (token.Type == TokenTypes.GREATER)

                    Process(TokenTypes.GREATER);

                else if (token.Type == TokenTypes.GREATER_EQUAL)

                    Process(TokenTypes.GREATER_EQUAL);

                else if (token.Type == TokenTypes.NOX)

                    Process(TokenTypes.NOX);

                node = new BinaryOperator(node, token, Expression());
            }


            return node;
        }

        /// <summary>
        /// Verifica que el nodo represente una expresión de comparación general. Este tipo de expresión está
        /// compuesta por uno o más expresiones de comparación separados por operadores de tipo `Y` y `O`.
        /// </summary>
        /// <returns>
        /// Devuelve un nodo que representa los distintos tipos de nodos que pueden aparecer.
        /// </returns>
        private AST Compounds()
        {
            AST node = Comparer();
            Token token = new Token(CurrentToken);

            while (CurrentToken.Type == TokenTypes.AND || CurrentToken.Type == TokenTypes.OR)
            {
                token = new Token(CurrentToken);

                if (token.Type == TokenTypes.AND)

                    Process(TokenTypes.AND);

                else if (token.Type == TokenTypes.OR)

                    Process(TokenTypes.OR);

                node = new BinaryOperator(node, token, Comparer());
            }


            return node;
        }

        /// <summary>
        /// Representa un nodo de acción vacía, o sea, que no lleva a cabo ningún tipo de acción.
        /// </summary>
        private AST Empty()
        {
            return new Empty();
        }

        /// <summary>
        /// Se encarga de verificar si el token actual representa una variable.
        /// </summary>
        /// <returns>
        /// Devuelve un objeto que representa la variable.
        /// </returns>
        private AST Variable()
        {
            AST node = new Var(CurrentToken);
            Process(TokenTypes.ID);

            return node;
        }

        /// <summary>
        /// Verifica si el nodo representa un tipo de operación de asignación. Este está 
        /// compuesta por una variable, el token de asignación y el componente que dará valor a la variable.
        /// </summary>
        private AST Assignment()
        {

            AST node = Variable();
            Token token = new Token(CurrentToken);

            Process(TokenTypes.ASSIGN);

            return new Assign((Var)node, token, Compounds());
        }

        /// <summary>
        /// Verifica si se cumple con la estructura que debe llevar una expresión condicional. Esta está
        /// compuesta por un token IF, un nodo de tipo Compunds encerrado entre paréntesis y una acción
        /// a ejecutar en caso de que la condición asociada se cumpla que se encuentra dentro de llaves.
        /// </summary>
        private AST Conditional()
        {
            Process(TokenTypes.IF);
            Process(TokenTypes.L_PARENT);

            AST node = Compounds();

            Process(TokenTypes.R_PARENT);
            Process(TokenTypes.L_KEYS);

            node = new Condition(node, StatementList());

            Process(TokenTypes.R_KEYS);

            return node;
        }

        /// <summary>
        /// Verifica si se cumple con la estructura que debe llevar un ciclo. Esta está
        /// compuesta por un token WHILE, un nodo de tipo Compunds encerrado entre paréntesis y una acción
        /// a ejecutar una y otra vez mientras que la condición asociada se cumpla que se encuentra 
        /// dentro de llaves.
        /// </summary>
        private AST Cicle()
        {
            Process(TokenTypes.WHILE);
            Process(TokenTypes.L_PARENT);

            AST node = Compounds();

            Process(TokenTypes.R_PARENT);
            Process(TokenTypes.L_KEYS);

            node = new Cicle(node, StatementList());

            Process(TokenTypes.R_KEYS);

            return node;
        }

        /// <summary>
        /// Verifica si se hace el llamado a una función interna del intérprete y que cumpla 
        /// con la sintaxis requerida. Ésta está compuesta por un token que representa el llamado
        /// a una función, el nombre de la función y entre paréntesis los parámetros que necesita 
        /// para ejecutarse.
        /// </summary>
        private AST Function()
        {
            Process(TokenTypes.FUNCTIONS);

            string methodName = "";

            Var method = (Var)Variable();
            methodName = method.Value;

            Functions node = new Functions("Methods", methodName);

            Process(TokenTypes.L_PARENT);

            node.Parameters.Add(Compounds());

            while (CurrentToken.Type == TokenTypes.COMMA)
            {
                Process(TokenTypes.COMMA);

                node.Parameters.Add(Compounds());
            }

            Process(TokenTypes.R_PARENT);

            return node;
        }

        /// <summary>
        /// Verifica si se cumple con la sintaxis de una declaración. Ésta está compuesta por un token 
        /// representando el tipo de dato y una o mas nombres de variables separados por comas.
        /// </summary>
        private AST Declaration()
        {
            Declarations declarations = new Declarations();

            TokenTypes type = (TokenTypes)(TypeData());

            declarations.Commands.Add(new VarDecl(new Var(CurrentToken), type));
            Process(TokenTypes.ID);

            while (CurrentToken.Type == TokenTypes.COMMA)
            {
                Process(TokenTypes.COMMA);

                declarations.Commands.Add(new VarDecl(new Var(CurrentToken), type));

                Process(TokenTypes.ID);
            }

            return declarations;
        }

        /// <summary>
        /// Verifica que el token actual represente un tipo de dato.
        /// </summary>
        private bool IsDataType()
        {
            return CurrentToken.Type == TokenTypes.INTEGER || CurrentToken.Type == TokenTypes.FLOAT
                    || CurrentToken.Type == TokenTypes.BOOLEAN || CurrentToken.Type == TokenTypes.STRING;
        }

        /// <summary>
        /// Verifica si el nodo actual reresenta un tipo de operaciones disponibles en el cuerpo del código.
        /// </summary>
        private AST Statement()
        {
            AST node = Empty();

            if (CurrentToken.Type == TokenTypes.L_PARENT)
            {
                Process(TokenTypes.L_PARENT);

                Statement();

                Process(TokenTypes.R_PARENT);
            }

            if (IsDataType())

                node = Declaration();

            else if (CurrentToken.Type == TokenTypes.ID)

                node = Assignment();

            else if (CurrentToken.Type == TokenTypes.IF)

                node = Conditional();

            else if (CurrentToken.Type == TokenTypes.WHILE)

                node = Cicle();

            else if (CurrentToken.Type == TokenTypes.FUNCTIONS)

                node = Function();

            return node;
        }

        /// <summary>
        /// Representa si cumple con los requisitos de una lista de instrucciones separadas por un punto y coma.
        /// </summary>
        /// <returns></returns>
        private AST StatementList()
        {
            Instructions instructions = new Instructions();
            instructions.Commands.Add(Statement());

            while (CurrentToken.Type == TokenTypes.SEMI)
            {
                Process(TokenTypes.SEMI);
                instructions.Commands.Add(Statement());
            }

            return instructions;
        }

        /// <summary>
        /// Verifica que el nodo actual represente un tipo de dato.
        /// </summary>
        /// <returns></returns>
        private TokenTypes TypeData()
        {
            TokenTypes token = TokenTypes.INTEGER;

            if (CurrentToken.Type == TokenTypes.INTEGER)
            {
                Process(TokenTypes.INTEGER);
                token = TokenTypes.INTEGER;
            }

            else if (CurrentToken.Type == TokenTypes.FLOAT)
            {
                Process(TokenTypes.FLOAT);
                token = TokenTypes.FLOAT;
            }

            else if (CurrentToken.Type == TokenTypes.BOOLEAN)
            {
                Process(TokenTypes.BOOLEAN);
                token = TokenTypes.BOOLEAN;
            }

            else if (CurrentToken.Type == TokenTypes.STRING)
            {
                Process(TokenTypes.STRING);
                token = TokenTypes.STRING;
            }

            return token;
        }

        /// <summary>
        /// Verifica que el nodo represente un bloque de acción compuesto por un token que
        /// representa el comienzo del bloque, el nombre de la carta y la lista de
        /// instrucciones a realizar dentro de llaves.
        /// </summary>
        /// <returns></returns>
        private AST Block()
        {
            Process(TokenTypes.CARD);

            CardCode card = new CardCode();

            card.Name = Factor();

            Process(TokenTypes.L_KEYS);

            card.Statement_List = StatementList();

            Process(TokenTypes.R_KEYS);

            card.Code = Lexer.CardCodeText.Trim();

            Lexer.NextNewCard();

            Process(TokenTypes.SEMI);

            return card;
        }

        /// <summary>
        /// Construye el nodo formado por uno o más bloques de cartas.
        /// </summary>
        public AST GetCards()
        {
            CardsList cards = new CardsList();
            cards.Cards.Add((CardCode)Block());

            while (CurrentToken.Type != TokenTypes.EOF)
            {
                cards.Cards.Add((CardCode)Block());
            }

            return cards;
        }

        /// <summary>
        /// Se encarga de verificr que el codigo cumpla con la sintaxis requerida para cada
        /// bloque de carta específico.
        /// </summary>
        public AST Parse()
        {
            AST node = Block();

            if (CurrentToken.Type != TokenTypes.EOF)

                Error("El programa no tiene la estructura deseada.");

            return node;
        }
    }
}