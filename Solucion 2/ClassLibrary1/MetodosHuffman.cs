using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;


namespace ClassLibrary1
{
    class MetodosHuffman
    {
        //metodo compresion
        public List<NodoHuffman> C_obtenerFrecuencias(string message)
        {
            List<NodoHuffman> listaDeNodos = new List<NodoHuffman>();
            List<NodoHuffman> listaDeNodos2 = new List<NodoHuffman>();
            Monticulo<NodoHuffman> monticulo = new Monticulo<NodoHuffman>();


            try
            {
                for (int i = 0; i < message.Length; i++)
                {
                    string read = message.Substring(i, 1);
                    if (listaDeNodos.Exists(x => x.identificador == read))
                        listaDeNodos[listaDeNodos.FindIndex(y => y.identificador == read)].aumentarFrecuencia();
                    else
                        listaDeNodos.Add(new NodoHuffman(read));
                }
                return listaDeNodos;
            }
            catch (Exception)
            {
                return null;
            }

        }
        //metodo neutro
        public void N_obtenerArbol(List<NodoHuffman> listaDeNodos, Monticulo<NodoHuffman> monticulo, ref List<NodoHuffman> listaDeNodos2)
        {
            List<NodoHuffman> listaTemp = listaDeNodos;
            for (int i = 0; i < listaTemp.Count; i++)
            {
                monticulo.insertar(listaTemp[i]);

            }
            int cont = monticulo.Count;
            while (cont > 1)
            {

                if (monticulo.Count > 0)
                {
                    NodoHuffman nodo3 = monticulo.extraerRaiz();
                    NodoHuffman nodo4 = monticulo.extraerRaiz();
                    listaDeNodos2 = monticulo.valores;
                    listaDeNodos2.Add(new NodoHuffman(nodo3, nodo4));
                }
                cont--;
            }
            listaDeNodos2 = monticulo.valores;
            Console.ReadKey();
        }
        //metodo neutro
        public void N_definirCodigoBinario(string codigoB, NodoHuffman nodos)
        {
            if (nodos == null)
                return;
            if (nodos.ramaIzquierda == null && nodos.ramaDerecha == null)
            {
                nodos.codigoB = codigoB;
                return;
            }
            N_definirCodigoBinario(codigoB + "0", nodos.ramaIzquierda);
            N_definirCodigoBinario(codigoB + "1", nodos.ramaDerecha);
        }
        //metodo neutro
        //public void N_imprimir(NodoHuffman listaDeNodos2)
        //{
        //    if (listaDeNodos2 == null)
        //        return;
        //    if (listaDeNodos2.ramaIzquierda == null && listaDeNodos2.ramaDerecha == null)
        //    {
        //        Console.WriteLine("identificador : {0} -  codigoB : {1}", listaDeNodos2.identificador, listaDeNodos2.codigoB);

        //        return;
        //    }
        //    N_imprimir(listaDeNodos2.ramaIzquierda);
        //    N_imprimir(listaDeNodos2.ramaDerecha);
        //}
        //-----------------------------------------------------------------------------------------------------------------------//
        //metodo de compresion
        public void C_reescribirMensajeBinario(List<NodoHuffman> listaDeNodos, string mensaje, ref string nuevoMensaje)
        {
            for (int i = 0; i < mensaje.Length; i++)
            {
                for (int j = 0; j < listaDeNodos.Count; j++)
                {
                    if (mensaje.Substring(i, 1) == listaDeNodos[j].identificador)
                    {
                        nuevoMensaje += listaDeNodos[j].codigoB;
                    }
                }
            }
        }
        //metodo de compresion 
        public void C_convertirBinaDec(string mensajeCodificado, ref List<string> listaNumerosB, ref List<int> listaNumerosD)
        {

            while (mensajeCodificado.Length > 0)
            {
                if (mensajeCodificado.Length >= 8)
                {
                    listaNumerosB.Add(mensajeCodificado.Substring(0, 8));
                    listaNumerosD.Add(Convert.ToInt32(mensajeCodificado.Substring(0, 8), 2));
                    mensajeCodificado = mensajeCodificado.Remove(0, 8);
                }
                else
                {

                    while (mensajeCodificado.Length < 8)
                    {
                        mensajeCodificado += 0.ToString();

                    }
                    listaNumerosB.Add(mensajeCodificado.Substring(0, 8));
                    listaNumerosD.Add(Convert.ToInt32(mensajeCodificado.Substring(0, 8), 2));
                    mensajeCodificado = mensajeCodificado.Remove(0, 8);
                }
            }
        }
        //metodo de compresion 
        public void C_convertirDecaAsc(List<int> listaNumerosD, ref List<char> listaSimbolsAsc, List<NodoHuffman> listaDeNodos, ref List<char> listaFrecuenciasAsc)
        {
            for (int i = 0; i < listaNumerosD.Count; i++)
            {
                listaSimbolsAsc.Add(Convert.ToChar(listaNumerosD[i]));
            }
            for (int i = 0; i < listaDeNodos.Count; i++)
            {
                listaFrecuenciasAsc.Add(Convert.ToChar(listaDeNodos[i].frecuencia));
            }
        }
        //metodo de compresion 
        public void C_mensajeFinal(ref string mensajeFinal, List<char> listaSimbolsAsc, List<NodoHuffman> listaDeNodos, List<char> listaFrecuenciasAsc)
        {
            for (int i = 0; i < listaDeNodos.Count; i++)
            {
                mensajeFinal += listaDeNodos[i].identificador + "[" + listaFrecuenciasAsc[i] + "]";
            }

            for (int i = 0; i < listaSimbolsAsc.Count; i++)
            {
                mensajeFinal += "[" + listaSimbolsAsc[i] + "]";
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------//
        //metodo de descompresion
        public void D_descomprimirMensaje(string mensajeComp, ref List<NodoHuffman> listaDeNodos, ref List<int> listaNumerosD, int iniciador)
        {
            int primerIterador = mensajeComp.Length;
            int segundoIterador = mensajeComp.Length;
            string aux = null;
            while (primerIterador > 0)
            {
                aux = null;
                if (mensajeComp.Substring(0, 1) == "[")
                {
                    primerIterador = -1;
                }
                else
                {
                    int j = 0;
                    NodoHuffman nuevoNodo = new NodoHuffman();
                    nuevoNodo.identificador = mensajeComp.Substring(0, 1);
                    mensajeComp = mensajeComp.Remove(0, 2);
                    for (int i = 0; i < mensajeComp.Length; i++)
                    {

                        if (mensajeComp.Substring(0, 1) != "]")
                        {
                            aux += mensajeComp.Substring(0, 1);
                            mensajeComp = mensajeComp.Remove(0, 1);
                            nuevoNodo.frecuencia = (byte)Convert.ToChar(aux);
                            j = 1;
                            i = 0;
                        }
                        else if (mensajeComp.Substring(0, 1) == "]" && mensajeComp.Substring(1, 1) != null && mensajeComp.Substring(1, 1) == "]")
                        {
                            aux += mensajeComp.Substring(0, 1);
                            nuevoNodo.frecuencia = (byte)Convert.ToChar(aux);
                            mensajeComp = mensajeComp.Remove(0, 1);
                            listaDeNodos.Add(nuevoNodo);
                            j = 0;
                            i = 0;
                        }
                        else
                        {
                            if (j == 1)
                            {
                                listaDeNodos.Add(nuevoNodo);
                            }
                            mensajeComp = mensajeComp.Remove(0, 1);
                            i = mensajeComp.Length + 1;
                        }
                    }
                }

            }
            int contadorSI = segundoIterador;
            aux = null;
            while (segundoIterador > 0)
            {
                aux = null;
                int j = 0;
                if (mensajeComp.Length > 0)
                {
                    mensajeComp = mensajeComp.Remove(0, 1);

                    for (int i = 0; i < contadorSI; i++)
                    {
                        if (mensajeComp.Substring(0, 1) != "]")
                        {
                            aux += mensajeComp.Substring(0, 1);
                            mensajeComp = mensajeComp.Remove(0, 1);
                            j = 1;
                            i = 0;
                        }
                        else if (mensajeComp.Substring(0, 1) == "]" && mensajeComp.Length > 1 && mensajeComp.Substring(1, 1) == "]")
                        {
                            aux += mensajeComp.Substring(0, 1);
                            listaNumerosD.Add((byte)Convert.ToChar(aux));
                            mensajeComp = mensajeComp.Remove(0, 1);
                            j = 0;
                            i = 0;
                        }
                        else
                        {
                            if (j == 1)
                            {
                                listaNumerosD.Add((byte)Convert.ToChar(aux));
                            }
                            mensajeComp = mensajeComp.Remove(0, 1);
                            i = contadorSI + 1;
                        }
                    }
                }
                else
                {
                    segundoIterador = -1;
                }
            }
        }

        //metodo descomprimir
        public void D_convertirDecaBin(List<int> listaNumerosD, List<string> listaNumerosB, ref string mensajeBinario)
        {
            for (int i = 0; i < listaNumerosD.Count; i++)
            {
                listaNumerosB.Add(Convert.ToString(Convert.ToInt64(listaNumerosD[i]), 2));
                if (listaNumerosB[i].Length != 8)
                {
                    string aux = Convert.ToString(Convert.ToInt64(listaNumerosD[i]));
                    aux = listaNumerosB[i];
                    listaNumerosB[i] = "";


                    while (aux.Length != 8)
                    {
                        aux = aux.Insert(0, "0");
                    }
                    listaNumerosB[i] = aux;
                }
            }
            D_rehacerMensajeBinario(ref mensajeBinario, listaNumerosB);
        }

        //metodo descomprimir
        public void D_rehacerMensajeBinario(ref string mensajeBinario, List<string> listaNumerosB)
        {
            for (int i = 0; i < listaNumerosB.Count; i++)
            {
                mensajeBinario += listaNumerosB[i];
            }
        }

        //metodo descomprimir

        public void D_rehacerMensajeOriginal(List<NodoHuffman> listaDeNodos, string mensajeB, ref string mensajeDesc)
        {
            bool seEncontro = false;
            int minCodigoPrefijo;
            minCodigoPrefijo = listaDeNodos[0].codigoB.Length;
            for (int i = 0; i < listaDeNodos.Count; i++)
            {
                if (listaDeNodos[i].codigoB.Length < minCodigoPrefijo)
                {
                    minCodigoPrefijo = listaDeNodos[i].codigoB.Length;
                }
            }
            List<NodoHuffman> listaApoyo = new List<NodoHuffman>();
            listaApoyo = listaDeNodos;
            for (int i = 0; i < mensajeB.Length; i++)
            {
                if (mensajeB.Length > minCodigoPrefijo)
                {
                    if (listaDeNodos.Count > 0)
                    {
                        for (int j = 1; j <= listaDeNodos.Count; j++)
                        {
                            if (mensajeB.Substring(0, i) == listaDeNodos[j - 1].codigoB)
                            {
                                mensajeDesc += listaDeNodos[j - 1].identificador;
                                mensajeB = mensajeB.Remove(0, i);
                                listaDeNodos[j - 1].frecuencia = listaDeNodos[j - 1].frecuencia - 1;
                                if (listaDeNodos[j - 1].frecuencia == 0)
                                {
                                    listaDeNodos.RemoveAt(j - 1);
                                }
                                seEncontro = true;
                                j = listaDeNodos.Count + 1;
                            }
                            else
                            {
                                seEncontro = false;
                            }

                        }
                    }
                    else
                    {
                        mensajeB = "";
                        i = mensajeB.Length + 1;
                    }
                }
                else
                {
                    mensajeB = "";
                    i = mensajeB.Length + 1;
                }
                if (seEncontro == true)
                {
                    i = 0;
                }
            }

        }
    }
}

