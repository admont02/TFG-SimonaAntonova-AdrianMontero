using System.Collections.Generic;

[System.Serializable]
public class Posicion
{
    public float x;
    public float y;
    public float z;
}

[System.Serializable]
public class CocheIA
{
    public List<Posicion> posiciones;
    public Posicion posicionInicial;
    public Posicion rotacionInicial;

}
[System.Serializable]
public class IA_Car
{
    public List<Posicion> posiciones;
    public Pieza pieza;
    public SubPosicion subPosicion;
    public string orientacion;

}
[System.Serializable]
public class Player
{
    public Posicion posicionInicial;
    public Posicion rotacionInicial;
}
[System.Serializable]
public class Cuadricula
{
    public Pieza pieza;
    public SubPosicion subPosicion;
}
[System.Serializable]
public class SemaforoConfig
{
    public bool doble;
    public Posicion posicion;
    public Posicion rotacion;
    public float greenSeconds;
    public float amberSeconds;
    public float redSeconds;
    public string initialLight;
}
[System.Serializable]
public class SemaforoNuevoConfig
{
    public bool doble;
    public Pieza pieza;
    public SubPosicion subPosicion;
    public string orientacion;
    public float greenSeconds;
    public float amberSeconds;
    public float redSeconds;
    public string initialLight;
}
[System.Serializable]
public class ElementoMapa
{
    public string tipo;
    public Posicion posicion;
}
[System.Serializable]
public enum LevelType
{
    Desconocido,
    Conduccion,
    Prioridad,
    Luces
}

[System.Serializable]
public class MapaNuevo
{
   
    public List<PosicionMapa> City_Crossroad;
    public List<PosicionMapa> City_Vertical_Road;
    public List<PosicionMapa> City_Horizontal_Road;
    public List<PosicionMapa> City_Crossroad_Crosswalk;

    public List<PosicionMapa> Roundabout_Front_Left_Trees;
    public List<PosicionMapa> Roundabout_Front_Right_Trees;
    public List<PosicionMapa> Roundabout_Left_Trees;
    public List<PosicionMapa> Roundabout_Right_Trees;



    //public List<PosicionMapa> rotondas;
}
[System.Serializable]
public class SubPosicion
{
    public int fil;
    public int col;
}

[System.Serializable]
public class Pieza
{
    public int index;
}

[System.Serializable]
public class Jugador
{
    public Pieza pieza;
    public SubPosicion subPosicion;
    public string orientacion;
}
[System.Serializable]
public class PosicionMapa
{
    public int id;
    public int fil;
    public int col;
}

[System.Serializable]
public class TargetForPlayer
{
    public Pieza pieza;
    public SubPosicion subPosicion;
}
[System.Serializable]
public class MapaConfig
{
    public string nombre;
    public Posicion posicion;
}
[System.Serializable]
public class Nivel
{
    public int nivel;
    public MapaConfig mapa;
    public MapaNuevo mapaNuevo;
    public Player jugador;
    public Jugador jugadorNuevo;
    public TargetForPlayer targetJugador;
    public List<CocheIA> cochesIA;
    public List<IA_Car> IACars;

    public List<Cuadricula> cuadriculas;
    public List<SemaforoConfig> semaforos;
    public List<SemaforoNuevoConfig> semaforosNuevos;

    public List<ElementoMapa> elementosMapa;
    public string[] levelDialogs;
    public string[] completedDialogs;
    public bool fog = false;
    public string type;
    public bool isMenu = false;
    public List<string> objetivo;
}
