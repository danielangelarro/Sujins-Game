using SujinsCards;

using System;
using System.Reflection;
using System.Collections.Generic;

namespace SujinsInterpreter
{
    /// <summary>
    /// Regula y compila las acciones internas del intérprete.
    /// </summary>
    public class Interpreter : NodeVisitor
    {
        private Parser Parser;
        private List<MonsterCard> ThisMonsterCamp = new List<MonsterCard>();    // Monstruos en mi campo
        private List<MonsterCard> EnemyMonsterCamp = new List<MonsterCard>();   // Monstruos del campo enemigo
        
        // Permite interactuar con los métodos, propiedades y clases declaradas en el código.
        private Assembly assambly = Assembly.GetExecutingAssembly();

        public Dictionary<string, dynamic> Scope;   // Almacena las variables declaradas en el código y las palabras reservadas.
        public MonsterCard ThisMonster = new MonsterCard();

        /// <summary>
        /// Inicializa las variables del código.
        /// </summary>
        private void Reset()
        {
            this.Scope = new Dictionary<string, dynamic>();

            Scope.Add("description", string.Empty);
            Scope.Add("image", string.Empty);
            Scope.Add("position", -1);
            Scope.Add("price", 0);
        }

        /// <summary>
        /// Constructor de la clase.
        /// </summary>
        /// <param name="parser">
        /// Instancia de parser asociada al codigo a interpretar.
        /// </param>
        public Interpreter(Parser parser)
        {
            this.Parser = parser;
            
            Reset();
        }

        /// <summary>
        /// Constructor de la clase.
        /// </summary>
        /// <param name="parser">
        /// Instancia de parser asociada al codigo a interpretar.
        /// </param>
        /// <param name="thisMonster">
        /// Representa a un monstruo en específico sobre el que se va a ejecutar la acción.
        /// </param>
        public Interpreter(Parser parser, ref MonsterCard thisMonster) : this(parser)
        {
            this.ThisMonster = thisMonster;
        }

        /// <summary>
        /// Constructor de la clase.
        /// </summary>
        /// <param name="parser">
        /// Instancia de parser asociada al codigo a interpretar.
        /// </param>
        /// <param name="thisMonsterCamp">
        /// Monstruos del campo del jugador actual.
        /// </param>
        /// <param name="enemyMonsterCamp">
        /// Monstruos del campo del jugador enemigo.
        /// </param>
        /// <returns></returns>
        public Interpreter(Parser parser, List<MonsterCard> thisMonsterCamp, List<MonsterCard> enemyMonsterCamp) : this(parser)
        {
            this.ThisMonsterCamp = thisMonsterCamp;
            this.EnemyMonsterCamp = enemyMonsterCamp;
        }
        
        /// <summary>
        /// Lanza una excepción en caso de ejecutarse un error a la hora de interpretar.
        /// </summary>
        /// <param name="error">
        /// Texto por defecto que muestra el mensaje de error.
        /// </param>
        private void Error(string error = "Error de ejecución del código")
        {
            throw new Exception(error);
        }

        /// <summary>
        /// Ejecuta la acción correspondiente a las operaciones binarias.
        /// </summary>
        /// <param name="node">
        /// Nodo que contiene la información de la operación a ejecutar.
        /// </param>
        public override dynamic VisitBinaryOperator(BinaryOperator node)
        {
            dynamic result = 0;

            dynamic left = Visit(node.Left);
            dynamic right = Visit(node.Right);

            switch (node.Operator.Type)
            {
                case TokenTypes.PLUS:

                    result = left + right;

                    break;

                case TokenTypes.MINUS:

                    result = left - right;

                    break;

                case TokenTypes.MULT:

                    result = left * right;

                    break;

                case TokenTypes.FLOAT_DIV:

                    result = left / right;

                    break;

                case TokenTypes.INTEGER_DIV:

                    result = (int)(left / right);

                    break;

                case TokenTypes.MOD:
                    {
                        result = left % right;

                        break;
                    }


                case TokenTypes.SAME:

                    result = (left == right);

                    break;

                case TokenTypes.DIFFERENT:

                    result = (left != right);

                    break;

                case TokenTypes.LESS:

                    result = left < right;

                    break;

                case TokenTypes.GREATER:

                    result = left > right;

                    break;

                case TokenTypes.LESS_EQUAL:

                    result = left <= right;

                    break;

                case TokenTypes.GREATER_EQUAL:

                    result = left >= right;

                    break;

                case TokenTypes.AND:

                    result = (left && right);

                    break;

                case TokenTypes.OR:

                    result = (left || right);

                    break;

            }

            return result;
        }

        /// <summary>
        /// Ejecuta la acción correspondientes a las operaciones unarias.
        /// </summary>
        /// <param name="node">
        /// Nodo que contiene la información de la operación a ejecutar.
        /// </param>
        /// <returns></returns>
        public override dynamic VisitUnaryOperator(UnaryOperator node)
        {
            int result = 0;

            switch (node.Operator.Type)
            {
                case TokenTypes.PLUS:

                    result = +Visit(node.Expression);

                    break;

                case TokenTypes.MINUS:

                    result = -Visit(node.Expression);

                    break;
            }

            return result;
        }

        /// <summary>
        /// Crea las cartas mágicas a partir del código.
        /// </summary>
        /// <param name="node">
        /// Nodo que contiene la inforación correspondiente para poder crear las cartas.
        /// </param>
        public override dynamic VisitCardsList(CardsList node)
        {
            List<MagicCard> cards = new List<MagicCard>();
            string name, description, image, code;
            int price, position;
            
            foreach (CardCode item in node.Cards)
            {
                name = Visit(item.Name);
                Visit(item.Statement_List);


                description = Scope["description"].ToString();
                price = (int)Scope["price"];
                image = Scope["image"];
                code = item.Code;
                position = (int)Scope["position"];

                if (position < -1 || position > 3)
                    Error("La variable \"position\" debe ser un valor entre [0, 2].");
                
                if (description.Trim() == "")
                    Error("La descripción no puede estar vacía.");
                
                if (image.Trim() == "")
                    Error("La imagen no puede estar vacía.");

                MagicCard newCard = new MagicCard(
                    name, 
                    description, 
                    price, 
                    image, 
                    code,
                    position
                );

                cards.Add(newCard);

                Reset();
            }
            
            return cards;
        }

        /// <summary>
        /// ejecuta todas las acciones que aparecen en el ámbito actual.
        /// </summary>
        /// <param name="node">
        /// Nodo que almacena la lista de acciones que pertenecen a éste ámbito(Scope).
        /// </param>
        public override dynamic VisitInstructions(Instructions node)
        {
            foreach (var item in node.Commands)
            {
                Visit(item);
            }

            return 0;
        }

        /// <summary>
        /// Ejecuta las declaraciones de variables en el código.
        /// </summary>
        /// <param name="node">
        /// Nodo que almacena la suficiente para declarar las variables.
        /// </param>
        public override dynamic VisitDeclarations(Declarations node)
        {
            foreach (var item in node.Commands)
            {
                Visit(item);
            }

            return 0;
        }

        /// <summary>
        /// Ejecuta las funciones internas que vienen predefinidas en el intérprete.
        /// </summary>
        /// <remarks>
        /// Usando `reflection` accedo a la clase donde se almacena los métodos predefinidos internos
        /// y ejecuta la acción del método pasandole los parámetros que necesita.
        /// </remarks>
        /// <param name="node">
        /// Contiene la información necesaria para ejecutar las funciones.
        /// </param>
        public override dynamic VisitFunctions(Functions node)
        {
            Type methodType = assambly.GetType($"SujinsInterpreter.{ node.ClassName }");    // Referencia a la clase
            MethodInfo method = methodType.GetMethod(node.MethodName);  // Referencia al método a ejecutar.

    	    // Parámetros que se les pasan al método
            // Los que identifican a cada método y 3 parámetros por defecto:
            //  - Posición del monstruo sobre el que va a interactuar la carta mágica.
            //  - Especifica con 1 o 2 si el monstruo es propio o del enemigo.
            //  - Valor que modifica el parámetro seleccionado.
            dynamic[] paramsThis = new dynamic[node.Parameters.Count + 3];

            paramsThis[0] = this.ThisMonsterCamp;
            paramsThis[1] = this.EnemyMonsterCamp;
            paramsThis[2] = (int)this.Scope["position"];

            for (int i = 0; i < node.Parameters.Count; i++)
            {
                paramsThis[i + 3] = (int)Visit(node.Parameters[i]);
            }

            method.Invoke(Activator.CreateInstance(methodType), paramsThis);

            this.ThisMonsterCamp = paramsThis[0];
            this.EnemyMonsterCamp = paramsThis[1];

            return 0;
        }

        /// <summary>
        /// Ejecuta la acción correspondientes a la asignación de los valores a las variables.
        /// </summary>
        /// <param name="node">
        /// Nodo que contiene la información para asignar los valores.
        /// </param>
        public override dynamic VisitAssign(Assign node)
        {
            string name = (string)node.Left.Value;

            Scope[name] = Visit(node.Right);

            return 0;
        }

        /// <summary>
        /// Ejecuta la acción correspondientes a la ejecución de una acción siempre que 
        /// se cumpla determinada condición.
        /// </summary>
        /// <param name="node">
        /// Nodo que contiene la información para validar una condición y ejecutar la acción correspondiente.
        /// </param>
        public override dynamic VisitCondition(Condition node)
        {
            if ((bool)Visit(node.Compound))

                Visit(node.StatementList);

            return 0;
        }

        /// <summary>
        /// Ejecuta la acción correspondientes a la ejecución de una acción de forma continua
        /// mientras se cumpla determinada condición.
        /// </summary>
        /// <param name="node">
        /// Nodo que contiene la información para validar una condición y ejecutar la acción correspondiente.
        /// </param>
        public override dynamic VisitCicle(Cicle node)
        {

            while ((bool)Visit(node.Compound))
            {
                Visit(node.StatementList);
            }

            return 0;
        }

        /// <summary>
        /// Realiza la petición del valor que contiene determinada variable.
        /// </summary>
        /// <param name="node">
        /// Nodo que contiene la información necesaria para utilizar la variable.
        /// </param>
        public override dynamic VisitVar(Var node)
        {
            string name = (string)node.Value;

            dynamic value = Scope[name];

            if (!Scope.ContainsKey(name))
            {
                Error($"Assignment error: {name} ");
            }

            return value;
        }

        /// <summary>
        /// Retorna el número expresado en el código.
        /// </summary>
        /// <param name="node">
        /// Nodo que contiene el valor del número que aparece en el códgo.
        /// </param>
        public override dynamic VisitNum(Num node)
        {
            return node.Value;
        }

        /// <summary>
        /// Retorna el valor de verdad expresado en el código.
        /// </summary>
        /// <param name="node">
        /// Nodo que contiene el valor de verdad que aparece en el códgo.
        /// </param>
        public override dynamic VisitBool(Bool node)
        {
            return (bool)node.Value;
        }

        /// <summary>
        /// Retorna el valor de la cadena expresado en el código.
        /// </summary>
        /// <param name="node">
        /// Nodo que contiene el valor de la cadena que aparece en el códgo.
        /// </param>
        public override dynamic VisitCadene(Cadene node)
        {
            return (string)node.Value;
        }

        /// <summary>
        /// Declara variables individuales
        /// </summary>
        /// <param name="node">
        /// Nodo que contiene la información correspondiente para ejecutar las declaración.
        /// </param>
        public override dynamic VisitVarDecl(VarDecl node)
        {
            if (node.Type == TokenTypes.INTEGER)

                Scope.Add((string)node.Node.Value, 0);

            else if (node.Type == TokenTypes.FLOAT)

                Scope.Add((string)node.Node.Value, 0.0);

            else if (node.Type == TokenTypes.BOOLEAN)

                Scope.Add((string)node.Node.Value, false);

            return 0;
        }

        // Declaracion vacía de tipo de datos.
        public override dynamic VisitType(TypeVar node) { return 0; }

        /// <summary>
        /// Acción vacía. 
        /// </summary>
        public override dynamic VisitEmpty(Empty node) { return 0; }

        /// <summary>
        /// Obtiene las listas de cartas mágicas creadas a partir del código.
        /// </summary>
        public List<MagicCard> GetCards()
        {
            AST tree = Parser.GetCards();
            List<MagicCard> cards = new List<MagicCard>();

            try
            {
                cards = Visit(tree);
            }
            catch (System.Exception e)
            {
                Error($"❌ Error: {e.Message}");
            }

            return cards;
        }

        /// <summary>
        /// Ejecuta la acción correspondiente al código de la carta mágica.
        /// </summary>
        public string Interpret()
        {
            AST tree = Parser.Parse();

            if (tree == null)
            {
                return "❌ Tree is Null";
            }

            try
            {
                Visit(tree);
            }
            catch (System.Exception e)
            {
                Console.WriteLine($"❌ {e.Data}");

                return e.Data.ToString();
            }

            return string.Empty;
        }
    }
}