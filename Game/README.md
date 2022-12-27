# 🏗️ Descripcion de la arquitectura del juego.

> A continuacion se ofrece una descripción con las clases que influyen en la lógica del juego:  

## ***`Clase Boveda:`***

**`Bóveda`** es una clase estática que nos va a permitir cargar nuestro banco de cartas.  La clase cuenta con cuatro propiedades:
- **_MarketMonsterDeck:_** es una lista de cartas de monstruos que va a contener las cartas que van a estar en la tienda.
- **_PublicMonsterDeck:_** lista de cartas monstruos donde van a estar las cartas a las que los jugadores van a tener acceso.
- **_PublicMagickDeck:_** es un objeto de la clase Deck donde se van a guardar las cartas magicas.
- **_Coins:_** entero que representa el dinero inicial y el cual se irá actualizando según transcurra el juego.


El primer método de esta clase es `load()`, pero antes de explicar el funcionamiento de este es necesario explicar los métodos `LoadMonsterCard()` y `LoadMagicCards()`. \

### `LoadMonsterCard():`
Éste método es la encargada de cargar las _cartas monstruos_, para ello se carga el archivo _Monstruo.data_ que está en la dirección guardada en la variable **Database** de la clase **`Config`**, este documento contiene las cartas separadas por salto de línea cada una y sus propiedades por un guion(-). Una vez cargado el documento se le aplica un Split por salto de línea y posteriormente al array resultante un Split por guión, las propiedades que aparecen en el documento son el nombre de la carta, la descripción y la imagen el resto como el ataque y la defensa se inicializan con números random entre valores especificados. \
Las primeras cartas(según el valor de la variable CantCardpPublics de la clase config) van para las cartas de acceso público y las restantes para la `Tienda`. Si el archivo fue encontrado y todas las cartas que se cargaron se devuelven en un string vacío, sino se devuelve un string donde se especifica que no fue encontrado el archivo.

### `LoadMagicCards():`
Este método lo primero que hace es cargar el archivo _Magics.data_, en caso de no existir retorna un string especificando este error. Una vez cargado el documento se le pasa al intérprete el cual se encarga de verificar que el código de cada carta es correcto. Una vez hecho esto se llama al método `GetCards` del intérprete el cual retorna cada carta para adicionarla a la lista de cartas del juego, una vez realizado todo esto se devuelve un string vacío.

### `load():`
Llama a los dos anteriores métodos y si los dos se ejecutaron bien entonces devuelve un string vacío.

### `SetCoins():`
Aumenta o disminuye el dinero en base al parámetro pasado.


## ***`Clase Config:`***
En esta clase se almacenan variables que tienen que ver con el estado del juego y los documentos. La case cuenta con 
- **Database:** guarda la dirección de los documentos que contienen las cartas.
- **Mediabase:** guarda la direcciónde las imágenes de cada carta.
- **CantCardsPublic:** almacena la cantidad de cartas entre las cuales los jugadores van a poder escoger.
- **CantMonsterForPlayer:** cantidad de cartas monstruos que puede almacenar un jugador en cada partida.
- **CantMagicsForPlayer:** cantidad de cartas mágicas que podrá poseer un jugador en cada partida.
- **CantMagicsForPlayerAtHand:** indica la cantidad de cartas mágicas que se mostraran por turno a cada jugador.
- **status:** para saber el estado en que se está ejecutando el proyecto, modo _Debug_ o _Development_.

El constructor de esta clase solo recibe un string para especificar el estado el cual es guardado en la variable status.

## **`Clase InfoCard:`**
Esta clase nos permitirá saber la información de cada carta. Para ello se declara una propiedad de tipo string por cada propiedad que tienen las cartas como el nombre la descripción etc…
La clase cuenta con los siguientes métodos:
- **ClearInfo:** Se encarga de inicializar con un guion cada una de las propiedades.
- **UpdateInfoMonster:** iguala cada propiedad de la clase a las de la _carta monstruo_.
- **UpadteInfoMagic:** iguala las propiedades **Name** y **Description** a las de la carta.
- **UpdateInfo:** llama a alguno de los métodos anteriores según el tipo de carta que sea.

## **`Clase GameStatus:`**
Clase creada con el objetivo de guardar el estado del juego.
Cuenta con las siguientes propiedades:
- **MonsterPS1:** lista de cartas de cartas monstruos para guardar las cartas del jugador 1.
- **MonsterPS2:** lista de cartas de cartas monstruos para guardar las cartas del jugador 2(en el modo de _single player_ este sería el jugador virtual).
- **MagicPS1:** guarda las cartas mágicas del jugador 1.
- **MagicPS2:** guarda las cartas mágicas del jugador 2(en el modo de _single player_ este sería el jugador virtual).
- **TableOfInfo:** Guarda la información de una carta seleccionada.
- **ChatGame:** Guarda la descripción de la jugada realizada. 
- **Turn:** indica a que jugador le corresponde jugar.

### `Clone():`
Devuelve una copia de la misma, es decir, crea un nuevo objeto **`GameState`** y le asigna a las propiedades de este los valores actuales de las propiedades de la clase.

### `IsLosser():`
Recibe el número del jugador y lo que hace es verificar si este tiene algún monstruo con vida, sino devuelve true. Indicando que perdió la partida.

### `GetStatusofMoster():`
Acumula en una variable la vida restante de cada monstruo del jugador después de simular un ataque con cada monstruo del jugador contrario para saber el estado en que esta un jugador hasta ese turno.

## **`Clase Game:`**
Esta es la clase donde se desarrolla la lógica del juego. Lo primero que vemos en esta clase es un enumerable de tipo **`TypeAction`** que representa las distintas acciones que puede pasar durante un turno como lanzar un monstruo, mostrar la información de este étc… La clase cuenta con varias propiedades:
- **Status:** objeto de tipo GameState que se encargara del estado del juego según valla transcurriendo la partida.
- **Modo:** string que representa el modo de juego con el que se ingreso a la partida(player vs player o single player).
- **SelectObjetive** toma valor de true cuando seleccionamos un monstruo (utilizado para saber que se escogió un monstruo para atacar y otro sobre el que se va a atacar).
- **FinishAction:** toma valor de true cuando el jugador realiza una acción en el turno (pues solo puede hacer una acción durante un turno).
- **MonsterAttack:** y **MonsterDeffense**: dos objetos de tipos MonsterCard que como su nombre indica representan los monstruos con los cuales se va a realizar el ataque. El primero es el que ataca y el segundo el que recibe el ataque.

**`Game`** es una clase que hereda de **`Config`**. Por tanto el constructor emplea el de Config pero siempre en modo _Debug_ para indicar que se va a desarrollar una partida. Además el constructor recibe como parámetro un string indicando el modo de juego de la partida el cual es guardado en la propiedad **Modo** e inicializa la propiedad **Status**. Esta clase se encuentra organizada por regiones. 

- **_Auxiliar Methods_**

    ### `GetMagicsCardsHand():` 
    Selecciona tres cartas mágicas de las guardadas en Boveda y devuelve una lista con ellas, pues son las cartas que va a usar un jugador en cada turno(recordemos que cada jugador va a tener 3 diferentes cartas mágicas por turno de las cuales solo puede usar una).

    ### `IsGameOver():`
    Indicando si algún jugador perdió y por tanto la partida debe culminar. Para ello lo que hace es llamar al método `IsLoser()` de **`GameStatus`** con cada jugador.

- **_Select Card_** 
    Métodos que influyen en el proceso de escoger las cartas, en la cual se encuentran tres variables privadas:
    - **MonsterCardID1:** índice del monstruo seleccionado por el primer jugador.
    - **MonsterCardID2:** índice del monstruo seleccionado por el segundo jugador.
    - **CantOfCard:** cantidad de monstruos a los que los jugadores tienen acceso.
    
    ### `GetMosnterSelect():`
    Devuelve el monstruo de guardado en lista de monstruos públicos de la clase **`Boveda`** con el índice de la variable **MonsterCardID1** o **MonsterCardID2** en dependencia del jugador que sea.
    
    ### `ShowMonster():`
    Devuelve un string con la dirección de la imagen del monstruo con índice **MonsterCardID1** o **MonsterCardID2** en dependencia del jugador.

    ### `NextCard():`
    Avanza a la siguiente carta, es decir aumenta en 1 el índice de **MonsterCardID1** o **MonsterCardID2** en dependencia del player, si llegan a la ultima carta se reinician en 0.

    ### `PrevCard():`
    Hace lo mismo que NextCard() solo que en sentido contrario, es decir vuelve a la carta anterior.

    ### `SelectCard():`
    Recibe como parámetro un entero que representa el jugador y lo que hace es seleccionar la carta con índice en **MonsterCardID1** o **MonsterCardID2**. La adiciona a la lista de monstruos de cada jugador (propiedad de la clase **`GameStatus`**).
    
    ### `RemoveCard():`
    Es similar al método anterior solo que en vez de seleccionar la carta la elimina de las cartas seleccionadas.

    ### `GetMonsterSelect():`
    Recibe un entero como parámetro representando el jugador y devuelve un objeto de tipo **`MonsterCard`** que seria la carta en el índice de **MonsterCardID1** o **MonsterCarID2**.

    ### `ShowMonster():`
    Recibe un entero que representa el jugador y devuelve un string con la dirección de la imagen de la carta en el índice especificado en **MonsterCardID1** o **MonsterCardID2**.

- **_Game Logic:_**
Aquí se encuentran los métodos que influyen directamente en la lógica del juego.

    ### `NextTurn():`
    Salta al siguiente turno pero antes reinicia las propiedades **FinishAction** y **SelectObjetive** para indicar que el siguiente jugador no ha hecho nada todavía.

    ### `MoveMonsterToCamp():` 
    Recibe como parámetro un objeto de tipo GameState y dos enteros uno para representar el jugador y otro para el índice de la carta. El método lo que hace es lanzar hacia el campo el monstruo del jugador, es decir vuelve true la propiedad **IsActive** de la carta.

    ### `SelectMonsterCard():`
    este es el método utilizado a la hora de realizar un ataque. Recibe como parámetro un objeto de tipo **`GameState`** y dos enteros uno para representar el jugador y otro para el índice de la carta. Cuando se va a ejecutar un ataque este método se llama dos veces, la primera es para escoger el monstruo con el cual se va a realizar el ataque verificando que no se haya escogido ningún monstruo anteriormente, una vez escogido la propiedad **SelectObjetive** toma valor true para validar que ya se seleccionó y se guarda en la propiedad **MonsterAttack**. La segunda vez que se llama se verifica q ya **SelectObjetive** este en _true_ y  se guarda en **MonsterDeffense** el monstruo seleccionado, un vez seleccionado ambos monstruos se llama al método `ActionsManager` de esta misma clase que recibe el estado del juego el numero del jugador y la acción que se va a realizar, este a su ves llama a la función `ActionsMonsterAttack` que recibe el estado del juego y el índice del monstruo atacado, lo que hace es restarle a la vida de este la diferencia entre el ataque del otro y los puntos de defensa de este, hecho esto se verifica la vida resultante y que el monstruo siga con vida sino se retira del campo y **FinshAction** toma valor _true_ indicando que ya se hizo una acción.

    ### `MagicUsage():` 
    Ejecuta una carta mágica. Para ello toma la carta del jugador especificado en el índice pasado como parámetro y llama al intérprete con esta carta y el monstruo sobre el que actúa y este es el que se encarga de ejecutar el código de la misma. Una vez realizado esto **FinishAction** toma valor true para indicar que ya se realizo una acción durante el turno.

    ### `ShowInfoCard():`
    Actualiza la información del objeto **TableInfo** del objeto **Status**, llamando a la función `UpdateInfo()` de la clase **TableInfo**, con la carta especificada al llamar al método.

    ### `GetMagicsCard():`
    Actualiza las tres cartas mágicas de ese jugador en el turno(recordemos que en cada turno a los jugadores se les mostrará tres cartas mágicas diferentes).

- **_Ejecute Action:_**
La mayoría de los métodos de esta región ya han sido explicados anteriormente pues son llamados desde el interior de otro, excepto el método `Action()` que recibe como parámetro la acción que se va a ejecutar en el turno el numero del jugador, el índice de la carta y un string especificando el tipo de carta. Este método es un void que se encarga de ejecutar la acción pasada como parámetro llamando a los distintos métodos de la clase encargados de ejecutarlas.

- **_Bot:_**
    Se encarga de ejecutar las acciones que intervienen en las decisiones uqe toma el jugador virtual a la hora de ejecutar una jugada.

    ### `BotLoadMonsterCards():`
    Se encarga de escoger los monstruos con los que jugará el jugador virtual.

    ### `BotPlay():`
    Ejecuta las acciones que puede ejecutar el bot en un turno (_Mover una carta monstruo al campo_, _usar una carta mágica_, _atacar a un monstruo_) en dependencia de si puede hacerlas en ese momento o no.

    ### `GetMonsterCard():`
    Escoge la carta del jugador 1 que tenga menor poder de defensa y la mayor carta del jugador virtual que tenga mayor poder de ataque con el objetivo de ejecutar un ataque efectivo.
