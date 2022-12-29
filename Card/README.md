# 🎴 Modelo de las cartas

## ***`Interface ICard:`***
Como se comentó anteriormente en el juego existen dos tipos de cartas, sin embargo ambos objetos poseen propiedades en comunes, para ello se creó la clase padre ICard que engloba el concepto de una carta sin importar el tipo que sea, las restantes clases que definen un tipo de carta en específico heredarían de esta clase.

Propiedades de un objeto carta:
- **Name:** con el nombre de la carta.
- **Description:** una descripción de la carta.
- **Price:** precio de la carta.
- **Image:** dirección de la imagen.
- **IsActive:** representa si una carta está en el campo o no

### `MoveCard():`
Modifica el valor de la propiedad **IsActive** en dependencia si se lanza una carta o se retira del campo.

## ***`Clase MonsterCard:`***
Define el objeto carta monstruo. Y como se explicó anteriormente hereda de la clase ***`ICard`***.

Propiedades específicas de una carta monstruo:
- **HealtPoins:** representa los puntos de vida de un monstruo.
- **MaxHealtPoins:** máxima cantidad de vida que puede llegar a tener un monstruo.
- **Defense:** puntos de defensa de un monstruo.
- **Attack:** representa los puntos de ataque.
- **Type:** objeto de la clase **`TypeMonsterElement`** que define el elemento o tipo de monstruo(fire, Dark  etc…).

El constructor de esta clase recibe como parámetros los valores necesarios para inicializar cada una de las propiedades, lo cual hace una vez llamado.

### `IsDead():`
Devuelve un booleano indicando si el monstruo sigue vivo o no(su vida es mayor que 0).

### `GetInfo():`
Devuelve una lista de string con las propiedades del monstruo.

### `UpdateHealtPoints():`
Modifica la vida del monstruo en base al valor pasado(se le suma a la vida actual), si la vida resultante es menor que 0 se iguala a 0.

### `UpdateAttack():`
Modificar los puntos de ataque del monstruo según el valor pasado(se le suma al valor actual).

### `UpdateDeffense():`
Modifica los puntos de defensa del monstruo según el valor pasado(se le suma al valor actual).

### `Clone():`
Devuelve una nueva instancia de la clase con los valores de las propiedades iguales a los de la clase actual.

## ***`Clase MonsterElemet:`***
Esta clase contienen los distintos tipos de monstruos del juego y varios métodos que determinan como influye el ataque de un monstruo hacia otro en dependencia del tipo que sean.
Lo primero q vemos es un enumerable con los distintos tipos de monstruos(Water, Fire, Rock, Flying y Dark). L a clase contiene las distintas propiedades:

- **EffectiveAttack:** un array bidimensional de enteros que representa la efectividad de los ataques. A cada tipo de monstruo le corresponde un índice en el array. En la matriz solo aparecen los valores de -1, 0 y 1 indicando que si un monstruo es fuerte con respecto a otro entonces esa tiene el valor de 1, si es débil es -1 y 0 en caso de ser neutro.
- **ElementID:** el cual asocia a cada tipo de monstruo su índice en el array.
- **IncrementValue** y **DecrementValue** que representan en cuanto se va a aumentar o disminuir el ataque.

El constructor de la clase no recibe parámetros y lo que hace es inicializar estas propiedades. **IncrementValue** y **DecrementValue** se inicializan en 1.5 y 0.7 respectivamente.

### `ComparerElements():`
El método lo que hace es buscar el valor de la casilla que posee **EffectiveAttack** en lo respectivos índicies que poseen ambos objetos según **ElementID** y si es uno retorna **IncrementValue**, si es -1 **DecrementValue** y si es 0 devuelve 0.

## ***`Clase MagicsCard:`***
En esta clase se definen las cartas mágicas, por lo q hereda de ***`ICard`***. Las cartas mágicas solo poseen dos nuevas propiedades:
- **position:** que representa la posición de la carta sobre la cual va a actuar su efecto.
- **Action:** para almacenar el código del efecto.

El constructor de la clase recibe como parámetro los valores necesarios para inicializar cada una de las propiedades e inicializa las mismas.
La clase cuenta con varios métodos como:

### `Clone():`
Devuelve una nueva instancia de la clase igualando cada propiedad de esta nueva instancia con la de la clase actual.
### `GetInfo():`
Devuelve una lista de string con las propiedades de la carta.
Además se le hizo un overraide al método tu string el cual va a devolver ahora un string con la forma en la que se va a imprimir en consola una carta mágica.

## ***`Clase Deck:`***
En esta clase englobamos el concepto de un mazo de cartas. Como se aprecia en el código la clase hereda de la intefaz ***`IEnumerable`***.
La primera propiedad de la clase es una lista para almacenar tales cartas. La segunda es el entero **length** para saber en que índice de la lista vamos y el y el booleano **Empty** que toma valor _true_ cuando **length** es 0.

### `Add():`
Recibe como parámetro un objeto de tipo carta y lo adiciona al mazo, en este caso la lista de cartas

### `Remove():`
recibe como parámetro una carta y la elimina del mazo.

### `RemoveAt():`
Hace lo mismo que Remove solo que este recibe un entero que representa la carta que se va a eliminar.

### `GetEnumerator():`
Retorna cartas aleatorias que representan ser escogida de un maso de cartas removidas previamente.