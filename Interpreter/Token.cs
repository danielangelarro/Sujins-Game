using System.Collections.Generic;

namespace SujinsInterpreter
{
    public class Token
    {
        public TokenTypes? Type;
        public dynamic Value;

        public Token(TokenTypes type, dynamic value)
        {
            this.Type = type;
            this.Value = value;
        }

        public Token(Token other)
        {
            Type = other.Type;
            Value = other.Value;
        }

        public string Show()
        {
            return $"Token({Type}, {Value})";
        }
    }

    public class ReservateKeywords
    {
        public Dictionary<string, TokenTypes> Keyword;
        public Dictionary<string, dynamic> Variables;

        public ReservateKeywords()
        {
            Keyword = new Dictionary<string, TokenTypes>();
            Variables = new Dictionary<string, dynamic>();

            #region Reserved Keywords

            Keyword.Add("Card", TokenTypes.CARD);
            
            
            // Keyword.Add("description", TokenTypes.DESCRIPTION);
            // Keyword.Add("price", TokenTypes.PRICE);
            // Keyword.Add("image", TokenTypes.IMAGE);
            // Keyword.Add("position", TokenTypes.POSITION);

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