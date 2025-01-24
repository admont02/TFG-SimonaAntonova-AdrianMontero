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
public class Cuadricula
{
    public Posicion posicion;
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
    public Posicion posicionCoche;
    public Posicion targetJugador;
    public List<CocheIA> cochesIA;
    public List<Cuadricula> cuadriculas;
    public List<SemaforoConfig> semaforos;
    public List<ElementoMapa> elementosMapa;
    public string[] levelDialogs;
    public string[] completedDialogs;
    public bool fog = false;
    public string type;
    public bool isMenu=false;
    public List<string> objetivo;
}
