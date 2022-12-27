# **Sujins-Game**: Juego de cartas coleccionables 🎴

```
███████╗██╗   ██╗     ██╗██╗███╗   ██╗███████╗     ██████╗  █████╗ ███╗   ███╗███████╗
██╔════╝██║   ██║     ██║██║████╗  ██║██╔════╝    ██╔════╝ ██╔══██╗████╗ ████║██╔════╝
███████╗██║   ██║     ██║██║██╔██╗ ██║███████╗    ██║  ███╗███████║██╔████╔██║█████╗  
╚════██║██║   ██║██   ██║██║██║╚██╗██║╚════██║    ██║   ██║██╔══██║██║╚██╔╝██║██╔══╝  
███████║╚██████╔╝╚█████╔╝██║██║ ╚████║███████║    ╚██████╔╝██║  ██║██║ ╚═╝ ██║███████╗
╚══════╝ ╚═════╝  ╚════╝ ╚═╝╚═╝  ╚═══╝╚══════╝     ╚═════╝ ╚═╝  ╚═╝╚═╝     ╚═╝╚══════╝
```

## 💻 Lógica del Juego:

**Sujins-Game** es un juego de cartas en el cual uno o dos jugadores se enfrentarán a un duelo letal.\
El juego cuenta con dos modos: uno para _dos jugadores_ y otro para _uno solo_. Si desea jugar usted solo deberá enfrentarse a un bot desarrollado para no tener piedad y hacer todo lo posible por aniquilarlo (le advertimos que esta IA es muy orgullosa y nunca ha perdido una partida 😉).\
Antes de comenzar a jugar verá que se le mostrará la opción de `Tienda`, pues le comentamos que en este juego cuenta con dos tipos de cartas (_mágicas_ y _monstruos_), usted tendrá acceso a un número limitado de _cartas monstruos_ para escoger antes de cada partida y otras cartas a las cuales no podrá acceder y tendrá que pagar si desea usarlas, estas son las cartas que se encuentran en la `Tienda`. Note que al iniciar el programa cuenta con un dinero inicial el cual podrá gastarlo aquí, para aumentar este dinero deberá derrotar a la IA(tarea casi imposible pero es libre de intentarlo, nosotros no lo desanimamos).\
También cuenta con la opción de crear sus propia carta, pero se explicará más adelante.\
Una vez seleccionado el modo de juego el jugador o los jugadores proceden a escoger 3 _cartas monstruos_ a su gusto y que estén disponibles claro está. Y así damos comienzo a una partida.

> En cada partida las acciones que puede ejecutar son: 
> - lanzar un monstruo al campo
> - realizar un ataque
> - utilizar una de las tres cartas mágicas.
> 
> Ojo solo puede ejecutar una acción por turno. Gana el jugador  que primero acabe con la vida de todos los monstruos del contrario.

## 📜 Índice de contenidos:

- [🎴 Modelo de las cartas](Card/README.md)
- [Manual del interprete](Interpreter/README.md)
- [🏗️ Descripcion de la arquitectura del juego.](Game/README.md)