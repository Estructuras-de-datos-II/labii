using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary1
{
    class NodoHuffman : IComparable
    {
        public string identificador;
        public int frecuencia;
        public string codigoB;
        public NodoHuffman nodoPadre;
        public NodoHuffman ramaIzquierda;
        public NodoHuffman ramaDerecha;
        public bool esHoja;

        public NodoHuffman()
        {

        }
        public NodoHuffman(string value)
        {
            identificador = value;
            frecuencia = 1;

            ramaDerecha = ramaIzquierda = nodoPadre = null;

            codigoB = "";
            esHoja = true;
        }


        public NodoHuffman(NodoHuffman nodo1, NodoHuffman nodo2)
        {

            codigoB = "";
            esHoja = false;
            nodoPadre = null;


            if (nodo1.frecuencia > nodo2.frecuencia)
            {
                ramaDerecha = nodo1;
                ramaIzquierda = nodo2;
                ramaDerecha.nodoPadre = ramaIzquierda.nodoPadre = this;
                identificador = nodo2.identificador + nodo1.identificador;
                frecuencia = nodo1.frecuencia + nodo2.frecuencia;
            }
            else if (nodo1.frecuencia <= nodo2.frecuencia)
            {
                ramaDerecha = nodo2;
                ramaIzquierda = nodo1;
                ramaIzquierda.nodoPadre = ramaDerecha.nodoPadre = this;
                identificador = nodo2.identificador + nodo1.identificador;
                frecuencia = nodo1.frecuencia + nodo2.frecuencia;
            }
        }


        public int CompareTo(object? obj)
        {
            NodoHuffman value = (NodoHuffman)obj;
            return frecuencia.CompareTo(value.frecuencia);

        }


        public void aumentarFrecuencia()
        {
            frecuencia++;
        }
    }
}
