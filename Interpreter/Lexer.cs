using System;

namespace SujinsInterpreter
{
    /// <summary>
    /// Clase encargada de ir tokenizando el código y devolver los distintos tokens que componen
    /// el código.
    /// </summary>
    public class Lexer
    {
        string Text;        // Texto del código original.
        int Pos;            // Posición del código en la que va en el recorrido actualmente.
        char? CurrentChar;  // Caracter que esta procesando en cada iteración.
        string Mode;        // Marca si solamente se desea compilar o correr el código.
        ReservateKeywords key;  // Almacena las palabras reervadas y las variables declaradas.

        /// <summary>
        /// Constructor de la clase.
        /// </summary>
        /// <param name="text">
        /// Texto del código original.
        /// </param>
        /// <param name="mode">
        /// Marca si solamente se desea compilar o correr el código.
        /// </param>
        public Lexer(string text, string mode)
        {
            Text = text;
            Pos = 0;
            CurrentChar = text[Pos];
            key = new ReservateKeywords();
            Mode = mode;
            CardCodeText = "";

            NextNewCard();
        }

        /// <summary>
        /// Almacena el código de cada carta mágica que se haya creado.
        /// </summary>
        public string CardCodeText { get; private set; }
        
        /// <summary>
        /// Marca que línea del código se encuentra procesando.
        /// </summary>
        /// <value></value>
        public int Line { get; private set; }

        /// <summary>
        /// Guarda el código asociado a una carta.
        /// </summary>
        public void NextNewCard()
        {
            CardCodeText = $"{CurrentChar}";
        }

        /// <summary>
        /// Lanza una excepcion cuando algo falla en la sintaxis del código.
        /// </summary>
        /// <param name="error">
        /// Texto alternativo para describir el tipo de excepción.
        /// </param>
        private void Error(string error = "Sintáxis inválida")
        {
            throw new Exception("[Lexer]: " + error);
        }

        /// <summary>
        /// Permite observar el caracter a continuación antes de procesarlo.
        /// </summary>
        /// <returns>
        /// Siguiente caracter.
        /// </returns>
        private char? SeekNextChar()
        {
            int pos = Pos + 1;

            if (pos == Text.Length)

                return null;

            return Text[pos];
        }

        /// <summary>
        /// Procesa el caracter actual y avanza al siguiente siempre que sea posible.
        /// </summary>
        private void Next()
        {
            Pos += 1;

            if (Pos == Text.Length)

                CurrentChar = null;

            else
            {
                CurrentChar = Text[Pos];
                CardCodeText += CurrentChar;

                // Si ha llegado al final de una línea avanza a la siguiente.
                // "\r\n" Salto de línea en Windows
                // '\n' Salto de línea en Linux.
                if ((CurrentChar == '\r' && SeekNextChar() == '\n') || CurrentChar == '\n')
                    Line++;
            }
        }

        /// <summary>
        /// Salta todos los caracteres vacíos como espacios y fin de líneas.
        /// </summary>
        private void SkipSpace()
        {
            while (CurrentChar != null && Char.IsWhiteSpace((char)CurrentChar))

                Next();
                char.IsSeparator('c');
        }

        /// <summary>
        /// Salta la parte del código comentada.
        /// </summary>
        private void SkipComment()
        {
            while (CurrentChar != null && !(CurrentChar == '>' && SeekNextChar() == ']'))

                Next();

            Next();
            Next();
        }

        /// <summary>
        /// Salta las funciones para no procesarlas.
        /// </summary>
        /// <remarks>
        /// Solamente utilizada cuando se desea compilar y no ejecutar el código.
        /// </remarks>
        private void SkipFunction()
        {
            while (CurrentChar != null && CurrentChar != ';')

                Next();

            Next();
        }

        /// <summary>
        /// Repreenta una cadena de palabras declaradas en el código.
        /// </summary>
        /// <returns>
        /// Un token con la cadena de palabras declaradas.
        /// </returns>
        private Token Cadene()
        {
            string value = "";

            Next();

            while (CurrentChar != null && CurrentChar != '\'')
            {
                value += CurrentChar;
                Next();
            }

            Next();

            return new Token(TokenTypes.STRING, value);
        }

        /// <summary>
        /// Representa los números que hayan sido escritos en el códgo.q
        /// </summary>
        /// <remarks>
        /// Un número comienza cn un dígito y está representado por dígitos y un punto que
        /// representa si el número es de tipo flotante.
        /// </remarks>
        /// <returns>
        /// Un token con el numero representado el código.
        /// </returns>
        private Token Number()
        {
            string value = "";

            while (CurrentChar != null && char.IsDigit((char)CurrentChar))
            {
                value += CurrentChar;
                Next();
            }

            if (CurrentChar == '.')
            {
                value += CurrentChar;
                Next();

                while (CurrentChar != null && char.IsDigit((char)CurrentChar))
                {
                    value += CurrentChar;
                    Next();
                }

                return new Token(TokenTypes.FLOAT, float.Parse(value));
            }


            return new Token(TokenTypes.INTEGER, float.Parse(value));
        }

        /// <summary>
        /// Representa las posibles variables  declaradas en el texto. Esto se cumple si el texto
        /// no representa una palabra reservada, en caso contrario retorna ésta.
        /// </summary>
        /// <remarks>
        /// Una variable comienza por una letra y puede contener letras y números.
        /// </remarks>
        /// <returns>
        /// Un token representando la variable asociada.
        /// </returns>
        public Token Variable()
        {
            string name = "";

            while (CurrentChar != null && char.IsLetterOrDigit((char)CurrentChar))
            {
                name += CurrentChar;
                Next();
            }

            if (key.Keyword.ContainsKey(name))

                return new Token(key.Keyword[name], key.Keyword[name]);

            return new Token(TokenTypes.ID, name);
        }

        /// <summary>
        /// Obtiene el siguiente token a procesar
        /// </summary>
        /// <returns>
        /// Eltoken asociado al actual fragmento del código a procesar.
        /// </returns>
        public Token GetNextToken()
        {
            while (CurrentChar != null)
            {
                // Salta los espacios en blanco.
                if (char.IsWhiteSpace((char)CurrentChar))
                {
                    SkipSpace();
                    continue;
                }

                // Salta las funciones si esta en modo debug.
                if (CurrentChar == '$' && Mode == "debug")
                {
                    Next();
                    SkipFunction();
                    
                    continue;     
                }

                // Salta los comentarios representados por [< comment >]
                if (CurrentChar == '[' && SeekNextChar() == '<')
                {
                    Next();
                    Next();
                    SkipComment();

                    continue;
                }

                // Retorna una cadena representada por 'cadene'
                if (CurrentChar == '\'')
                {
                    return Cadene();
                }

                // Retorna una variable declarada, en caso de ser una palabra reservada retorna ésta.
                if (char.IsLetter((char)CurrentChar))
                {
                    return Variable();
                }

                // Retorna el número representado.
                if (char.IsDigit((char)CurrentChar))

                    return Number();

                switch (CurrentChar)
                {
                    case '+':   // Retorna el token que representa una suma.

                        Next();

                        if (CurrentChar == '=')
                        {
                            Next();

                            return new Token(TokenTypes.ASSIGN_PLUS, "+=");
                        }

                        return new Token(TokenTypes.PLUS, '+');

                    case '-':   // Retorna el token que representa una resta.

                        Next();

                        if (CurrentChar == '=')
                        {
                            Next();

                            return new Token(TokenTypes.ASSIGN_MINUS, "-=");
                        }

                        return new Token(TokenTypes.MINUS, '-');

                    case '*':   // Retorna el token que representa una multiplicación.

                        Next();

                        if (CurrentChar == '=')
                        {
                            Next();

                            return new Token(TokenTypes.ASSIGN_MUL, "*=");
                        }

                        return new Token(TokenTypes.MULT, '*');

                    case '/':   // Retorna el token que representa una división.

                        Next();

                        if (CurrentChar == '=')
                        {
                            Next();

                            return new Token(TokenTypes.ASSIGN_DIV, "/=");
                        }

                        if (CurrentChar == '/')
                        {
                            Next();

                            return new Token(TokenTypes.INTEGER_DIV, "//");
                        }

                        return new Token(TokenTypes.FLOAT_DIV, '/');

                    case '%':   // Retorna el token que representa una modulación.

                        Next();

                        if (CurrentChar == '=')
                        {
                            Next();

                            return new Token(TokenTypes.ASSIGN_MOD, "%=");
                        }

                        return new Token(TokenTypes.MOD, '%');

                    case '(':   // Retorna el token que representa un paréntesis izquierdo.

                        Next();

                        return new Token(TokenTypes.L_PARENT, '(');

                    case ')':   // Retorna el token que representa un paréntesis derecho.

                        Next();

                        return new Token(TokenTypes.R_PARENT, ')');

                    case '{':   // Retorna el token que representa una llave izquierda.

                        Next();

                        return new Token(TokenTypes.L_KEYS, '{');

                    case '}':   // Retorna el token que representa una llave derecha.

                        Next();

                        return new Token(TokenTypes.R_KEYS, '}');

                    case '=':   // Retorna el token que representa una asignación o igualdad.

                        Next();

                        if (CurrentChar == '=')
                        {
                            Next();

                            return new Token(TokenTypes.SAME, "==");
                        }

                        return new Token(TokenTypes.ASSIGN, "=");

                    case '!':   // Retorna el token que representa una negación o diferenciación.

                        Next();

                        if (CurrentChar == '=')
                        {
                            Next();

                            return new Token(TokenTypes.DIFFERENT, "!=");
                        }

                        return new Token(TokenTypes.NOX, "!");

                    case '<':   // Retorna el token que representa un `menor que` o un `menor o igual que`.

                        Next();

                        if (CurrentChar == '=')
                        {
                            Next();

                            return new Token(TokenTypes.LESS_EQUAL, "<=");
                        }

                        return new Token(TokenTypes.LESS, '<');

                    case '>': // Retorna el token que representa un `mayor que` o un `mayor o igual que`.

                        Next();

                        if (CurrentChar == '=')
                        {
                            Next();

                            return new Token(TokenTypes.GREATER_EQUAL, ">=");
                        }

                        return new Token(TokenTypes.GREATER, '>');

                    case '&':   // Retorna el token que representa una conjución `Y`.

                        if (SeekNextChar() == '&')
                        {
                            Next();
                            Next();

                            return new Token(TokenTypes.AND, "&&");
                        }

                        break;

                    case '|':   // Retorna el token que representa una conjución `O`.

                        if (SeekNextChar() == '|')
                        {
                            Next();
                            Next();

                            return new Token(TokenTypes.OR, "||");
                        }

                        break;

                    case ',':   // Retorna el token que representa una coma.

                        Next();

                        return new Token(TokenTypes.COMMA, ',');

                    case ';':   // Retorna el token que representa un `punto y coma`.

                        Next();

                        return new Token(TokenTypes.SEMI, ';');

                    case '$':   // Retorna el token que representa una función.

                        Next();

                        return new Token(TokenTypes.FUNCTIONS, '$');

                    case '.':   // Retorna el token que representa un punto.

                        Next();

                        return new Token(TokenTypes.DOT, '.');
                }

                // Si no fue encontrado ningún token lanza una excepción.
                Error();
            }

            return new Token(TokenTypes.EOF, null);
        }
    }
}