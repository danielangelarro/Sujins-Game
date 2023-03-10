using System;
using System.Collections.Generic;

namespace SujinsCards
{
    /// <summary>
    /// Elementos que pueden tomar los diferentes tipos de monstruos.
    /// </summary>
    public enum TypeMonsterElement
    {
        Water,
        Fire,
        Rock,
        Flying,
        Dark
    }

    /// <summary>
    /// 
    /// Compara la efectividad de ataque entre 2 monstruos dependiendo de su elemento.
    /// 
    /// </summary>
    /// 
    /// <remarks>
    /// 
    /// Teniendo 2 monstruos distintos A y B la tabla se organiza de la siguiente manera:
    /// 
    /// [-1] -> si el elemento A es débil contra el elemento B.
    /// [ 0] -> si ambos monstruos tiene el mismo elemento o el ataque entre ellos es neutro.
    /// [ 1] -> si el elemento A es fuerte contra el elemento B.
    /// 
    /// Se organizan en una martiz representando filas y columnas y en las coordenadas se
    /// colocan los valores asociados.
    /// 
    ///     -------------------------------------------------
    ///     |         | Water | Fire | Rock | Flying | Dark |
    ///     -------------------------------------------------
    ///     | Water   |       |      |      |        |      |
    ///     -------------------------------------------------
    ///     | Fire    |       |      |      |        |      |
    ///     -------------------------------------------------
    ///     | Rock    |       |      |      |        |      |
    ///     -------------------------------------------------
    ///     | Flying  |       |      |      |        |      |
    ///     -------------------------------------------------
    ///     | Dark    |       |      |      |        |      |
    ///     -------------------------------------------------
    /// 
    /// </remarks>
    public class MonsterElements
    {
        // Tabla de comparacion de efectividad de los ataques.
        int[,] EffectiveAttack;

        Dictionary<TypeMonsterElement, int> ElementID;
        float IncrementValue;
        float DecrementValue;

        /// <summary>
        /// Constructor de la clase.
        /// </summary>
        /// <remarks>
        /// Se encarga de inicializar los distintos valores, asi como de organizar
        /// la relacion de fuerza y debilidad de los distintos elementos con respecto
        /// a los demas.
        /// </remarks>
        public MonsterElements()
        {
            EffectiveAttack = new int[,] {
                {  0,  1,  1,  0,  0},
                { -1,  0, -1,  0,  1},
                { -1,  1,  0,  1,  0},
                {  0,  0, -1,  0,  0},
                {  0, -1,  0,  0,  0}
            };

            ElementID = new Dictionary<TypeMonsterElement, int>();
            
            ElementID[TypeMonsterElement.Water]  = 1;
            ElementID[TypeMonsterElement.Fire]   = 2;
            ElementID[TypeMonsterElement.Rock]   = 3;
            ElementID[TypeMonsterElement.Flying] = 4;
            ElementID[TypeMonsterElement.Dark]   = 5;

            IncrementValue = 1.5f;
            DecrementValue = 0.7f;
        }

        /// <summary>
        /// Compara la efectividad del ataque de un mostruo contra otro
        /// revisando el tipo de cada uno
        /// </summary>
        /// <param value="m1">
        /// Elemento del primer monstruo a comparar.
        /// </param>
        /// <param value="m2">
        /// Elemento del 2do monstruop a comparar.
        /// </param>
        public float ComparerElements(TypeMonsterElement m1, TypeMonsterElement m2)
        {
            int value = EffectiveAttack[ElementID[m1], ElementID[m2]];

            if (value == 1)
                return IncrementValue;

            if (value == -1)
                return DecrementValue;
            
            return 1;
        }
    }
}