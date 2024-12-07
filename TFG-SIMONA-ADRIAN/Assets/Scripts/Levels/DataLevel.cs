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
}

[System.Serializable]
public class ElementoMapa
{
    public string tipo;
    public Posicion posicion;
}

[System.Serializable]
public class Nivel
{
    public int nivel;
    public Posicion posicionCoche;
    public List<CocheIA> cochesIA;
    public List<ElementoMapa> elementosMapa;
}
