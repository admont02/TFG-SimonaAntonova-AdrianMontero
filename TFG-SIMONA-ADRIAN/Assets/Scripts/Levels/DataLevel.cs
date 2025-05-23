using System;
using System.Collections.Generic;
using UnityEngine;
//para el editor de niveles
[System.Serializable]
public class MapaCompleto
{
    public string type;
    public MapaData mapa;
    public List<Stop> stops = new List<Stop>();
    public List<Prohibido> prohibidos = new List<Prohibido>();
    public List<Ceda> cedas = new List<Ceda>();
    public List<MaxVelocidad> maxVelocidad = new List<MaxVelocidad>();
    public List<IniLuz> iniLuces;
    public List<FrenteIzq> frenteIzq=new List<FrenteIzq>();
    public List<FrenteDcha> frenteDcha=new List<FrenteDcha>();
    public List<Frente> frente=new List<Frente>();
    public List<Cuadricula> cuadriculas= new List<Cuadricula>();

    public List<IA_Car> IACars = new List<IA_Car>();
    public TargetForPlayer targetJugador;
    public Jugador jugador;
    public List<Semaforo> semaforos = new List<Semaforo>();
    public List<string> objetivo;
    public List<int> correctOrder;
    public string[] levelDialogs;
    public string[] completedDialogs;
    public string[] wrongDialogs;
    public bool fog = false;
    public bool rain = false;
    public bool night = false;
    public bool deslumbramiento = false;
}
[System.Serializable]
public class MapaData
{
    public int numPiezas;
    public int filas;
    public int columnas;
    public List<RoundaboutPieza> Roundabout = new List<RoundaboutPieza>();
    public List<CrossroadPieza> Crossroad = new List<CrossroadPieza>();
    public List<TipoDePieza> Vertical = new List<TipoDePieza>();
    public List<TipoDePieza> VerticalContinua = new List<TipoDePieza>();
    public List<TipoDePieza> Horizontal = new List<TipoDePieza>();
    public List<TipoDePieza> HorizontalContinua = new List<TipoDePieza>();
    
    public List<TipoDePieza> Pavement = new List<TipoDePieza>();
    public List<TipoDePieza> Pavement_1 = new List<TipoDePieza>();
    public List<TipoDePieza> Pavement_2 = new List<TipoDePieza>();
    public List<TipoDePieza> Pavement_3 = new List<TipoDePieza>();
    public List<TipoDePieza> Grass = new List<TipoDePieza>();
    public List<TipoDePieza> Grass_2 = new List<TipoDePieza>();
    public List<TipoDePieza> Pavement_4 = new List<TipoDePieza>();
    public List<TipoDePieza> TurnLeft = new List<TipoDePieza>();
    public List<TipoDePieza> TurnLeftContinua = new List<TipoDePieza>();
    public List<TipoDePieza> TurnRight = new List<TipoDePieza>();
    public List<TipoDePieza> TurnRightContinua = new List<TipoDePieza>();
    public List<TipoDePieza> TurnLeft2 = new List<TipoDePieza>();
    public List<TipoDePieza> TurnLeft2Continua = new List<TipoDePieza>();
    public List<TipoDePieza> TurnRight2 = new List<TipoDePieza>();
    public List<TipoDePieza> TurnRight2Continua = new List<TipoDePieza>();
    public List<TipoDePieza> TunnelVertical = new List<TipoDePieza>();
    public List<TipoDePieza> TunnelHorizontal = new List<TipoDePieza>();

}

[System.Serializable]
public class TipoDePieza
{
    public int fil;
    public int col;
}
[System.Serializable]
public class CrossroadPieza : TipoDePieza
{
    public List<int> conexiones; // Lista de conexiones
}
[System.Serializable]
public class RoundaboutPieza : TipoDePieza
{
    public List<int> conexiones; // Lista de conexiones
}
public static class SceneData
{
    public static bool firstTime = true;
    public static string JsonFileName = "menu.json";
    // para poder probar el nivel sin venir desde el menu sustituir lo de arriba por esta:
    //public static string JsonFileName="nivel14.json";
    public static Vector3 lastCarPosition = Vector3.zero; // �ltima posici�n del coche en el men� o nivel
    public static bool hasLastPosition = false;
    public static Quaternion lastCarRotation = Quaternion.identity;

}


[System.Serializable]
public class Posicion
{
    public float x;
    public float y;
    public float z;
}

[System.Serializable]
public class IA_Car
{
    public Pieza pieza;
    public SubPosicion subPosicion;
    public string orientacion;
    public int branchTo;
    public string vehicle = "car";
    public bool emergency = false;
}

[System.Serializable]
public class Cuadricula
{
    public Pieza pieza;
    public SubPosicion subPosicion;
}
[System.Serializable]
public class Stop
{
    public Pieza pieza;
    public SubPosicion subPosicion;
    public string orientacion;
}
[System.Serializable]
public class FrenteIzq
{
    public Pieza pieza;
    public SubPosicion subPosicion;
    public string orientacion;
}
[System.Serializable]
public class FrenteDcha
{
    public Pieza pieza;
    public SubPosicion subPosicion;
    public string orientacion;
}
[System.Serializable]
public class Frente
{
    public Pieza pieza;
    public SubPosicion subPosicion;
    public string orientacion;
}
[System.Serializable]
public class MaxVelocidad
{
    public int velocidad;
    public Pieza pieza;
    public SubPosicion subPosicion;
    public string orientacion;
}
[System.Serializable]
public class Ceda
{
    public Pieza pieza;
    public SubPosicion subPosicion;
    public string orientacion;
}
[System.Serializable]
public class IniLuz
{
    public Pieza pieza;
    public SubPosicion subPosicion;
    public string orientacion;
}
[System.Serializable]
public class Prohibido
{
    public Pieza pieza;
    public SubPosicion subPosicion;
    public string orientacion;
}
[System.Serializable]
public class Node
{
    public int Id;

    public Node(int id)
    {
        Id = id;
    }
}

[System.Serializable]
public class Digrafo
{
    public int v;
    public int a = 0;
    public List<List<int>> ady;
    public Digrafo(int v_)
    {
        v = v_;
        ady = new List<List<int>>();
        for (int i = 0; i < v; i++)
        {
            ady.Add(new List<int>());

        }
    }


    ///**
    //* Crea un grafo dirigido a partir de los datos en el flujo de entrada (si puede).
    //* primer es el �ndice del primer v�rtice del grafo en el entrada.
    //*/
    //Digrafo(std::istream & flujo, int primer = 0) : _A(0)
    //{
    //    flujo >> _V;
    //    if (!flujo) return;
    //    _ady.resize(_V);
    //    int E, v, w;
    //    flujo >> E;
    //    while (E--)
    //    {
    //        flujo >> v >> w;
    //        ponArista(v - primer, w - primer);
    //    }
    //}
    public void ponArista(int a, int b)
    {
        if (a < 0 || a >= v || b < 0 || b >= v)
            //("Vertice inexistente");
            ++a;
        ady[a].Add(b);
    }
    // constante
    public bool hayArista(int a, int b)
    {
        return ady[a].Contains(b);
    }
    public List<int> getAdy(int a)
    {
        if (a < 0 || a >= v)
            return null;

        return ady[a];
    }
    // Digrafo inverso() const {
    //   Digrafo inv(_V);
    //   for (int v = 0; v<_V; ++v) {
    //      for (int w : _ady[v]) {
    //         inv.ponArista(w, v);
    //      }
    //   }
    //   return inv;
    //}
}

[System.Serializable]
public class Semaforo
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
public class PriorityQueue<T>
{
    private List<Tuple<T, float>> elements = new List<Tuple<T, float>>();

    public int Count
    {
        get { return elements.Count; }
    }

    public void Enqueue(T item, float priority)
    {
        elements.Add(Tuple.Create(item, priority));
    }

    public T Dequeue()
    {
        int bestIndex = 0;

        for (int i = 0; i < elements.Count; i++)
        {
            if (elements[i].Item2 < elements[bestIndex].Item2)
            {
                bestIndex = i;
            }
        }

        T bestItem = elements[bestIndex].Item1;
        elements.RemoveAt(bestIndex);
        return bestItem;
    }

    public bool Contains(T item)
    {
        foreach (var element in elements)
        {
            if (EqualityComparer<T>.Default.Equals(element.Item1, item))
            {
                return true;
            }
        }
        return false;
    }
}

[System.Serializable]
public class MapaNuevo
{
    public int numPiezas;
    public int filas;
    public int columnas;
    public List<PosicionMapa> Pavement;
    public List<PosicionMapa> Pavement_1;
    public List<PosicionMapa> Pavement_2;
    public List<PosicionMapa> Pavement_3;
    public List<PosicionMapa> Pavement_4;

    public List<PosicionMapa> Crossroad;
    public List<PosicionMapa> Roundabout;
    public List<PosicionMapa> Intersection;
    public List<PosicionMapa> Intersection2;
    public List<PosicionMapa> Intersection3;
    public List<PosicionMapa> Intersection4;

    public List<PosicionMapa> Vertical;
    public List<PosicionMapa> VerticalContinua;
    public List<PosicionMapa> Horizontal;
    public List<PosicionMapa> HorizontalContinua;

    public List<PosicionMapa> TurnRight;
    public List<PosicionMapa> TurnRightContinua;
    public List<PosicionMapa> TurnRight2;
    public List<PosicionMapa> TurnRight2Continua;
    public List<PosicionMapa> TurnLeft;
    public List<PosicionMapa> TurnLeftContinua;
    public List<PosicionMapa> TurnLeft2;
    public List<PosicionMapa> TurnLeft2Continua;
    public List<PosicionMapa> TunnelVertical;
    public List<PosicionMapa> TunnelHorizontal;
    public List<PosicionMapa> Grass;
    public List<PosicionMapa> Grass_2;




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
    public List<int> conexiones;
}

[System.Serializable]
public class TargetForPlayer
{
    public Pieza pieza;
    public SubPosicion subPosicion;
}
[System.Serializable]
public class IADestroyer
{
    public Pieza pieza;
    public SubPosicion subPosicion;
    public string orientacion;

}
[System.Serializable]
public class CocheIAEditorData
{
    public bool recto;
    public bool dcha;
    public bool izqda;
    public int index;

    public CocheIAEditorData()
    {
        recto = false;
        dcha = false;
        izqda = false;
        index = -1;
    }
}
[System.Serializable]
public class Nivel
{
    public int nivel;
    //public MapaConfig mapa;
    public MapaNuevo mapa;
    public Jugador jugador;
    public TargetForPlayer targetJugador;
    public List<IA_Car> IACars;

    public List<Cuadricula> cuadriculas;
    public List<Stop> stops;
    public List<MaxVelocidad> maxVelocidad;
    public List<FrenteIzq> frenteIzq;
    public List<FrenteDcha> frenteDcha;
    public List<Frente> frente;

    public List<Prohibido> prohibidos;
    public List<Ceda> cedas;
    public List<IniLuz> iniLuces;
    public List<Semaforo> semaforos;

    public List<ElementoMapa> elementosMapa;
    public string[] levelDialogs;
    public string[] completedDialogs;
    public string[] wrongDialogs;
    public bool fog = false;
    public bool rain = false;
    public bool night = false;
    public bool deslumbramiento = false;
    public string type;
    public bool isMenu = false;
    public List<string> objetivo;
    public List<int> correctOrder;
    public List<IADestroyer> IADestroyer;
}
