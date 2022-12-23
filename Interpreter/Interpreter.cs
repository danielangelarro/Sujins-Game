using SujinsCards;

using System;
using System.Reflection;
using System.Collections.Generic;

namespace SujinsInterpreter
{
    public class Interpreter : NodeVisitor
    {
        private Parser Parser;
        private List<MonsterCard> ThisMonsterCamp = new List<MonsterCard>();
        private List<MonsterCard> EnemyMonsterCamp = new List<MonsterCard>();
        private Assembly assambly = Assembly.GetExecutingAssembly();

        public Dictionary<string, dynamic> Scope;
        public MonsterCard ThisMonster = new MonsterCard();

        private void Reset()
        {
            this.Scope = new Dictionary<string, dynamic>();

            Scope.Add("description", string.Empty);
            Scope.Add("image", string.Empty);
            Scope.Add("position", -1);
            Scope.Add("price", 0);
        }

        public Interpreter(Parser parser)
        {
            this.Parser = parser;
            
            Reset();
        }

        public Interpreter(Parser parser, ref MonsterCard thisMonster) : this(parser)
        {
            this.ThisMonster = thisMonster;
        }

        public Interpreter(Parser parser, List<MonsterCard> thisMonsterCamp, List<MonsterCard> enemyMonsterCamp) : this(parser)
        {
            this.ThisMonsterCamp = thisMonsterCamp;
            this.EnemyMonsterCamp = enemyMonsterCamp;
        }
        
        private void Error(string error = "Caracter inválido")
        {
            throw new Exception(error);
        }

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

        public override dynamic VisitInstructions(Instructions node)
        {
            foreach (var item in node.Commands)
            {
                Visit(item);
            }

            return 0;
        }

        public override dynamic VisitDeclarations(Declarations node)
        {
            foreach (var item in node.Commands)
            {
                Visit(item);
            }

            return 0;
        }

        public override dynamic VisitFunctions(Functions node)
        {
            Type methodType = assambly.GetType($"SujinsInterpreter.{ node.ClassName }");
            MethodInfo method = methodType.GetMethod(node.MethodName);

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

        public override dynamic VisitAssign(Assign node)
        {
            string name = (string)node.Left.Value;

            Scope[name] = Visit(node.Right);

            return 0;
        }

        public override dynamic VisitCondition(Condition node)
        {
            if ((bool)Visit(node.Compound))

                Visit(node.StatementList);

            return 0;
        }

        public override dynamic VisitCicle(Cicle node)
        {

            while ((bool)Visit(node.Compound))
            {
                Visit(node.StatementList);
            }

            return 0;
        }

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

        public override dynamic VisitNum(Num node)
        {
            return node.Value;
        }

        public override dynamic VisitBool(Bool node)
        {
            return (bool)node.Value;
        }

        public override dynamic VisitCadene(Cadene node)
        {
            return (string)node.Value;
        }

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

        public override dynamic VisitType(TypeVar node) { return 0; }

        public override dynamic VisitEmpty(Empty node) { return 0; }

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
                Console.WriteLine($"❌ Error: {e.Message}");

                // TODO: return e.Data.ToString();
            }

            return cards;
        }

        public string Interpret()
        {
            AST tree = Parser.Parse();

            if (tree == null)
            {
                Console.WriteLine("❌ Tree is Null");

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