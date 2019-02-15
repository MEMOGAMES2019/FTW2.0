using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script que calcula el mejor camino entre dos puntos mediante el algoritmo A*.
/// </summary>
class Node
{
    public int gCost = 0, hCost = 0, fCost = 0, x = 0, y = 0, terreno = 0;
    public Node parent = null;
}

public struct Posicion
{
    public int x, y;
    public Posicion(int _x, int _y) { x = _x; y = _y; }
}

namespace AStar
{

    public class AStarSolver
    {
        private Posicion arr, abj, der, izq;                                            //Los cuatro movimientos posibles de cada jugador
        private Posicion pDestino;
        private readonly int ancho;
        private readonly int alto;
        private int[,] mapa;

        public AStarSolver(int _ancho, int _alto)
        {
            arr.x = 0; arr.y = -1;
            abj.x = 0; abj.y = 1;
            der.x = 1; der.y = 0;
            izq.x = -1; izq.y = 0;
            ancho = _ancho; alto = _alto;

        }

        public void ActualizaMapa(int[,] m)                                        //Método que actualiza la matriz con el estado del mapa
        {
            mapa = m;
        }

        public LinkedList<Posicion> Solve(int x, int y, Posicion pActivo)           //Método que resuelve el mapa con el algoritmo AStar
        {
            Dictionary<Posicion, Node> seen, close;
            LinkedList<Posicion> sol;

            sol = new LinkedList<Posicion>();
            seen = new Dictionary<Posicion, Node>();
            close = new Dictionary<Posicion, Node>();

            pDestino.x = x; pDestino.y = y;
            Posicion pAux = new Posicion();
            Posicion p2 = new Posicion();

            Node aux = CreaNodo(pActivo.x, pActivo.y, null);
            Node ch = null, compara;
            pAux.x = aux.x; pAux.y = aux.y;
            seen.Add(pAux, aux);

            while (seen.Count > 0 && (aux.x != pDestino.x || aux.y != pDestino.y))
            {
                pAux.x = aux.x; pAux.y = aux.y;
                seen.Remove(pAux);
                for (int i = 0; i < 4; i++)
                {
                    ch = null;
                    compara = null;
                    switch (i)                                                                                          //Se compara para cada uno de los posibles movimientos.
                    {
                        case 0:
                            p2.x = aux.x + arr.x; p2.y = aux.y + arr.y;
                            if (aux.y - 1 >= 0 && !close.ContainsKey(p2))
                            {
                                ch = CreaNodo(aux.x + arr.x, aux.y + arr.y, aux); pAux.x = ch.x; pAux.y = ch.y;
                            }
                            break;
                        case 1:
                            p2.x = aux.x + abj.x; p2.y = aux.y + abj.y;
                            if (aux.y + 1 < alto && !close.ContainsKey(p2))
                            {
                                ch = CreaNodo(aux.x + abj.x, aux.y + abj.y, aux); pAux.x = ch.x; pAux.y = ch.y;
                            }
                            break;
                        case 2:
                            p2.x = aux.x + der.x; p2.y = aux.y + der.y;
                            if (aux.x + 1 < ancho && !close.ContainsKey(p2))
                            {
                                ch = CreaNodo(aux.x + der.x, aux.y + der.y, aux); pAux.x = ch.x; pAux.y = ch.y;
                            }
                            break;
                        case 3:
                            p2.x = aux.x + izq.x; p2.y = aux.y + izq.y;
                            if (aux.x - 1 >= 0 && !close.ContainsKey(p2))
                            {
                                ch = CreaNodo(aux.x + izq.x, aux.y + izq.y, aux); pAux.x = ch.x; pAux.y = ch.y;
                            }
                            break;
                    }
                    if (ch != null && !seen.ContainsKey(pAux) && !close.ContainsKey(pAux) && ch.terreno < 2) seen.Add(pAux, ch);
                    else if (ch != null && !close.ContainsKey(pAux) && ch.terreno < 2)
                    {
                        seen.TryGetValue(pAux, out compara);
                        if (compara.gCost < ch.gCost)
                        {
                            ch.gCost = compara.gCost;
                            ch.fCost = ch.gCost + ch.hCost;
                            ch.parent = compara.parent;
                        }
                        seen.Remove(pAux);
                        seen.Add(pAux, ch);
                    }
                }

                pAux.x = aux.x; pAux.y = aux.y;
                if (!close.ContainsKey(pAux))
                    close.Add(pAux, aux);

                int nextNode = 999999;
                foreach (Node n in seen.Values)
                {
                    if (n.fCost < nextNode) { aux = n; nextNode = n.fCost; }
                    else if (n.fCost == nextNode && n.gCost < aux.gCost) { aux = n; }
                }
            }
            // Metemos la solucion a la pila leyendo los padres si es que hay solucion

            if (aux.x == pDestino.x && aux.y == pDestino.y)
            {
                while (aux.parent != null)
                {
                    pAux = new Posicion(aux.x, aux.y);
                    sol.AddLast(pAux);
                    aux = aux.parent;
                }
            }

            return sol;
        }

        Node CreaNodo(int x, int y, Node padre)
        {

            Node aux = new Node
            {
                x = x,
                y = y
            };
            aux.terreno = mapa[aux.y, aux.x];
            if (padre != null) { aux.gCost = padre.gCost + 1 + aux.terreno; aux.parent = padre; }
            else
            {
                aux.gCost = 0;
                aux.terreno -= 2;
            }
            aux.hCost = Mathf.Abs(x - pDestino.x) + Mathf.Abs(y - pDestino.y);
            aux.fCost = aux.gCost + aux.hCost;

            return aux;

        }
    }
}