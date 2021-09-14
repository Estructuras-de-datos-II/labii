using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace HuffmanTest
{
    class Monticulo<T> where T : IComparable
    {
        public List<T> valores = new List<T>();

        public void insertar(T o)
        {
            valores.Add(o);

            int i = valores.Count - 1;
            while (i > 0)
            {
                int j = (i + 1) / 2 - 1;

                T v = valores[j];
                if (v.CompareTo(valores[i]) < 0 || v.CompareTo(valores[i]) == 0)
                {
                    break;
                }

                T tmp = valores[i];
                valores[i] = valores[j];
                valores[j] = tmp;

                i = j;
            }
        }

        public T extraerRaiz()
        {
            if (valores.Count < 0)
            {
                throw new ArgumentOutOfRangeException();
            }


            T min = valores[0];
            valores[0] = valores[valores.Count - 1];
            valores.RemoveAt(valores.Count - 1);
            this.reOrdenar(0);
            return min;
        }

        public int Count
        {
            get { return valores.Count; }
        }

        private void reOrdenar(int i)
        {
            int menor;
            int l = 2 * (i + 1) - 1;
            int r = 2 * (i + 1) - 1 + 1;

            if (l < valores.Count && (valores[l].CompareTo(valores[i]) < 0))
            {
                menor = l;
            }
            else
            {
                menor = i;
            }

            if (r < valores.Count && (valores[r].CompareTo(valores[menor]) < 0))
            {
                menor = r;
            }

            if (menor != i)
            {
                T tmp = valores[i];
                valores[i] = valores[menor];
                valores[menor] = tmp;
                this.reOrdenar(menor);
            }
        }
    }
}