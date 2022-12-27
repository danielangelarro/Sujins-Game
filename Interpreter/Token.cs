using System.Collections.Generic;

namespace SujinsInterpreter
{
    /// <summary>
    /// Representa un fragmento del código, una de las piezas en que se separa el
    /// código para verificar de manera correcta su correcta estructura.
    /// </summary>
    public class Token
    {
        public TokenTypes? Type;
        public dynamic Value;

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="type">
        /// Representa el tipo de token actual.
        /// </param>
        /// <param name="value">
        /// Representa el valor que posee el token actual.
        /// </param>
        public Token(TokenTypes type, dynamic value)
        {
            this.Type = type;
            this.Value = value;
        }

        /// <summary>
        /// Constructor que permite clonar otro objeto de tipo Token.
        /// </summary>
        /// <param name="other">
        /// El token anterior que desea ser clonado.
        /// </param>
        public Token(Token other)
        {
            Type = other.Type;
            Value = other.Value;
        }
    }

    /// <summary>
    /// Reserva palabras que representan partes importantes en la sintaxis del código.
    /// </summary>
    public class ReservateKeywords
    {
        public Dictionary<string, TokenTypes> Keyword;  // Almacena las palabras reservadas para la sintaxis del código.
        public Dictionary<string, dynamic> Variables;   // Almacena las distintas variables declaradas en el código.

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <remarks>
        /// Inicializa los dccionarios y define las palabras reservadas.
        /// </remarks>
        public ReservateKeywords()
        {
            Keyword = new Dictionary<string, TokenTypes>();
            Variables = new Dictionary<string, dynamic>();

            #region Reserved Keywords

            Keyword.Add("Card", TokenTypes.CARD);
            Keyword.Add("int", TokenTypes.INTEGER);
            Keyword.Add("float", TokenTypes.FLOAT);
            Keyword.Add("bool", TokenTypes.BOOLEAN);
            Keyword.Add("string", TokenTypes.STRING);
            Keyword.Add("if", TokenTypes.IF);
            Keyword.Add("True", TokenTypes.TRUE);
            Keyword.Add("False", TokenTypes.FALSE);
            Keyword.Add("while", TokenTypes.WHILE);
            Keyword.Add("return", TokenTypes.RETURN);

            #endregion
        }
    }
}