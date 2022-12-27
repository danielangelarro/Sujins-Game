using System.Linq.Expressions;
using System.Collections.Generic;

namespace SujinsInterpreter
{
    /// <summary>
    /// Clase genérica que representa a todos los nodos del AST (árbol de sintáxis abstracta).
    /// </summary>
    public class AST { }

    /// <summary>
    /// Nodo que representa una lista con todas las carta mágicas.
    /// </summary>
    /// <remarks>
    /// Está compuesto por códigos de cartas mágicas.
    /// </remarks>
    public class CardsList : AST
    {
        public List<CardCode> Cards = new List<CardCode>();
    }

    /// <summary>
    /// Nodo que representa el código asociado al código de las cartas mágicas.
    /// </summary>
    /// <remarks>
    /// Está compuesto por el nombre de la carta, la lista de instrucciones que relaiza,
    /// su descripción,  la posición a la que afecta y el código asociado a ella.
    /// </remarks>
    public class CardCode : AST
    {
        public AST Name;
        public AST Statement_List;

        public string Description;
        public int Position;
        public string Code;
    }

    /// <summary>
    /// Nodo que representa una operación binaria.
    /// </summary>
    /// <remarks>
    /// Está compuesto por un nodo izquierdo, un nodo derecho y la operación asociada.
    /// </remarks>
    public class BinaryOperator : AST
    {
        public AST Left, Right;
        public Token Operator;

        public BinaryOperator(AST left, Token op, AST right)
        {
            Left = left;
            Operator = op;
            Right = right;
        }
    }

    /// <summary>
    /// Representa una operación unaria
    /// </summary>
    /// <remarks>
    /// Está compuesto por la operación asociada y la expresión que se modifica.
    /// </remarks>
    public class UnaryOperator : AST
    {
        public Token Operator;
        public AST Expression;

        public UnaryOperator(Token op, AST expr)
        {
            Operator = op;
            Expression = expr;
        }
    }

    /// <summary>
    /// Nodo que representa un número.
    /// </summary>
    /// <remarks>
    /// Está compuesto por el token asociado a los números y el valor del mismo.
    /// </remarks>
    public class Num : AST
    {
        public Token Token;
        public dynamic Value;


        public Num(Token token)
        {
            Token = token;
            Value = token.Value;
        }
    }

    /// <summary>
    /// Nodo que representa un valor de verdad.
    /// </summary>
    /// <remarks>
    /// Está compuesto por el token asociado a los valores de verdad y el valor del mismo.
    /// </remarks>
    public class Bool : AST
    {
        public Token Token;
        public bool Value;

        public Bool(Token token)
        {
            Token = token;
            Value = (bool)token.Value;
        }
    }

    /// <summary>
    /// Nodo que representa el valor de una cadena.
    /// </summary>
    /// <remarks>
    /// Está compuesto por el token asociado a las cadenas y el valor del mismo.
    /// </remarks>
    public class Cadene : AST
    {
        public Token Token;
        public string Value;

        public Cadene(Token token)
        {
            Token = token;
            Value = (string)token.Value;
        }
    }

    /// <summary>
    /// Nodo que representa el valor asociado a una variable.
    /// </summary>
    /// <remarks>
    /// Está compuesto por el token asociado a las variables y el valor de la misma.
    /// </remarks>
    public class TypeVar : AST
    {
        public Token Token;
        public dynamic Value;

        public TypeVar(Token token)
        {
            Token = token;
            Value = token.Value;
        }
    }

    /// <summary>
    /// Nodo que representa el conjunto de instrucciones que contiene un fragmento del
    /// código de la carta.
    /// </summary>
    /// <remarks>
    /// Está compuesto por una lista de nodos representando los distintos tipos de instrucciones
    /// a realizar.
    /// </remarks>
    public class Instructions : AST
    {
        public List<AST> Commands;

        public Instructions()
        {
            Commands = new List<AST>();
        }
    }

    /// <summary>
    /// Nodo que representa una declaración de una nueva variale.
    /// </summary>
    /// <remarks>
    /// Está compuesto por una lista de nodos individuales que representan delcaraciones e variables.
    /// </remarks>
    public class Declarations : AST
    {
        public List<AST> Commands;

        public Declarations()
        {
            Commands = new List<AST>();
        }
    }

    /// <summary>
    /// Nodo que representa la acción de asignar un valor a una variable.
    /// </summary>
    /// <remarks>
    /// Está compuesto por un nodo izquierdo (variable), el token asociado a la operación de asignación
    /// y un nodo derecho (valor que recibirá la variable).
    /// </remarks>
    public class Assign : AST
    {
        public Var Left;
        public Token Operator;
        public AST Right;

        public Assign(Var left, Token op, AST right)
        {
            Left = left;
            Operator = op;
            Right = right;
        }
    }

    /// <summary>
    /// Nodo que representa el llamado a una función predefinida en el interprete.
    /// </summary>
    /// <remarks>
    /// Está compuesto por el nombre de la clase donde se encuentra el método a invocar, el nombre de dicho 
    /// método y los parámetros que recibe.
    /// </remarks>
    public class Functions : AST
    {
        public string ClassName;
        public string MethodName;
        public List<AST> Parameters;

        public Functions(string className, string methodName)
        {
            this.ClassName = className;
            this.MethodName = methodName;
            this.Parameters = new List<AST>();
        }
    }

    /// <summary>
    /// Nodo que representa una variable declarada.
    /// </summary>
    /// <remarks>
    /// Está compuesto por el token asociado a las variables definidas y el valor de las mismas.
    /// </remarks>
    public class Var : AST
    {
        public Token Token;
        public dynamic Value;

        public Var(Token token)
        {
            Token = token;
            Value = token.Value;
        }
    }

    /// <summary>
    /// Nodo que representa la accíon de declarar una variable.
    /// </summary>v
    /// <remarks>
    /// Está compuesto por el token asociado a la declaración de variable y la variable a declarar.
    /// </remarks>
    public class VarDecl : AST
    {
        public Var Node;
        public TokenTypes Type;

        public VarDecl(Var node, TokenTypes type)
        {
            Node = node;
            Type = type;
        }
    }

    /// <summary>
    /// Nodo que representa una operación vacía.
    /// </summary>
    public class Empty : AST
    {

    }

    /// <summary>
    /// Nodo que representa una condición o instrucción de tipo IF.
    /// </summary>
    /// <remarks>
    /// Está compuesto por un nodo de condicion y una lista de instrucciones. Si se cumple la condición 
    /// entonces se puede ejecutar las acciones.
    /// </remarks>
    public class Condition : AST
    {
        public AST Compound;
        public AST StatementList;

        public Condition(AST compound, AST statements)
        {
            Compound = compound;
            StatementList = statements;
        }
    }

    /// <summary>
    /// Nodo que representa un ciclo de acciones a repetir.
    /// </summary>
    /// <remarks>
    /// Está compuesto por un nodo de condicion y una lista de instrucciones. Mientras se cumple la condición 
    /// entonces se puede ejecutar las acciones. Esto lo hace de manera repetitiva.
    /// </remarks>
    public class Cicle : AST
    {
        public AST Compound;
        public AST StatementList;

        public Cicle(AST compound, AST statements)
        {
            Compound = compound;
            StatementList = statements;
        }
    }

}