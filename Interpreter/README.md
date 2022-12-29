# INTERPRETER OF DY.X

Nuestro interprete es letal. Sirve no solo para crear cartas, sino para escribir cualquier tipo de programa
lineal con solo cambiar un poco las cosas. Nos orgullecemos en decir que nuestro interprete ha logrado simular algoritmos que solo utilizan condicionales y ciclos. Sí, leyeron bien, ¡nuestro intérprete implementa estas 2
cosas!

Este intérprete se encarga de procesar el código correspondientes a las cartas mágicas, desde corregir que
están bien escritos, hasta crearlas a partir de códigos que usted mismo puede escribir.

## Aspectos generales

>⚠️ Debe respetar la sintaxis (manera en que se escriben las cosas). 
> - ❌ Al final de cada expresión debe colocar un **`;`**.
> - ❌ Siempre que se habra un paréntesis o una llave esta debe ser cerrada luego.
> - ❌ Las palabras predefinidas deben ser escritas de manera correcta.
> - ❌ Debe evitar crear ciclos que puedan ejecutarse de manera infinita

## Tipos de datos

Nuestro interprete cuenta con 3 4 tipos de datos distintos:
- **bool:** permite referirse a expresiones de verdad. _True_ y _False_ son variables internas que pueden usarse.
```
bool a, b, c;
```

- **int:** permite guardar valores de números enteros, o sea, sin coma.
```
int a, b, c;
```

- **float:** permite guardar valores de números flotantes, o sea, con coma.
```
float a, b, c;
```

- **string:** permite guardar cadenas de textos. sirve para expresar el nombre, la descripción y la 
dirección de la imagen, así como puede ser utilizado en caso de que sea necesario trabajar con el 
elemento del monstruo.
```
string a, b, c;
```

## Variables predefinidasa propias de las cartas

A la hora de codificar las cartas debe tener en cuenta variables que le ayudarán a especificar de manera más
legible las acciones que realiza. Esta sintaxis debe ser respetada ya que un mal uso de ella
impedirá que el código se ejecute.

> **[< ESTO ES UN COMENTARIO >]** O sea, un fragmento de código que sirve para explicar algo,
> que nada que esté escrito en su interior dañará la ejecución del programa.

```
Card 'Nombre' [< Aquí va el nombre de la carta >]
{
    description = 'descripción';    [< Aquí va la descripción de la acción que realiza la carta >]
    price = 23;     [< Aquí va el precio inicial de la carta en el mercado interno del juego >]
    position = 2;   [< Posición de la carta monstruo en el campo a la que afecta >]
    image = 'image.png';   [< Aquí va la dirección de donde se encuentra la imagen correspondiente >]
};
```

## Ciclos y Condicionales

Si necesita hacer uso de alguna condición para permitir realizar una acción
puede hacerlo a través de los condicionales. Su funcionamiento es sencillo de 
comprender: si se cumple la condición que desea se ejecuta las acciones que contiene
en su interior.

```
if ( condición a cumplir ) {
    acción a realizar
};
```

De igual manera funcionan los ciclos, solo que estos repetirán la acción mientras la
condición se siga cumpliendo aún cuando se hayan ejecutado otras acciones que se 
encuentren en su interior.

```
while ( condición a cumplir ) {
    acción a realizar
};
```

## Métodos predefinidos

Para poder interactuar sobre las cartas monstruos lo hará con el uso de métodos o funciones
predefinidas por el intérprete. Estos tienen una manera de escritura muy particular:

```
$Function(1, 2);
```
1) Representa si la acción se va a ejecutar sobre un monstruo mío o del contrario. 
(1 para monstruos míos y 2 para monstruos del contrario).
2) Representa el valor de cambio que afectara al parámetro que desea ser modificado
de la carta monstruo.

Los siguientes métodos son los que pueden ser utilizados:
- `IncrementHP(int, int)`
- `DecrementHP(int, int)`
- `IncrementATK(int, int)`
- `DecrementATK(int, int)`
- `IncrementDEF(int, int)`
- `DecrementDEF(int, int)`
- `GetType(int, string)`

# Otros datos

## Gramática utilizada

```
getCards : block*
block : CARD cadene L_KEYS statement_list (SEMI statement_list)* R_KEYS
statement_list : statement
               | statement SEMI statement_list
statement : L_PARENT statement R_PARENT
          | declarations
          | assignment
          | conditionals
          | cicles
          | funtions
          | empty
functions : INTERNAL L_PARENT factor (COMMA factor)* R_PARENT
declarations : type_data ID (COMMA ID)* SEMI
assignment : variable ASSIGN compounds
cicles : WHILE L_PARENT compounds R_PARENT L_KEYS statement_list R_KEYS
conditionals : IF L_PARENT compounds R_PARENT L_KEYS statement_list R_KEYS
compounds : comp ((AND | OR) comp)*
          | L_PARENT compounds R_PARENT
comp : expr ((SAME | DIFFERENT | LESS | GREATER | LESS_EQUAL | GREATER_EQUAL | NOX) expr)*
expr : term ((PLUS | MINUS) term)*
term : factor ((MUL | DIV | MOD) factor)*
factor : PLUS factor
       | MINUS factor
       | INTEGER 
       | FLOAT 
       | STRING
       | TRUE
       | FALSE
       | L_PARENT expr R_PARENT
type_data : INTEGER 
          | FLOAT 
          | BOOL 
          | STRING
globals_variables : states
                  | models
variable : ID
empty : 
```

## Operadores

- _PLUS (+)_
- _MINUS (-)_
- _MULT (*)_
- _MOD (%)_
- _FLOAT_DIV (/)_
- _INTEGER_DIV (//)_
- _ASSIGN (=)_
- _SAME (==)_
- _DIFFERENT (!=)_
- _LESS (<)_
- _GREATER (>)_
- _NOX (!)_
- _AND (&&)_
- _OR (||)_

## Palabras reservadas
- **int**
- **float**
- **string**
- **True**
- **False**
- **main**
- **if**
- **while**
- **return**

## Ejemplo de algunos códigos

```
Card 'Nombre' {
    description = 'Incrementa en 50 el hp de la tercera carta';
    price = 23;
    position = 2;
    image = 'Card10.png';

    $IncrementHP(1, 50);
};

Card 'Carta2' {
    description = 'Incrementa en 120 el hp de la segunda carta';
    price = 23;
    position = 1;
    image = 'Card11.png';

    $IncrementHP(1, 120);
};
```