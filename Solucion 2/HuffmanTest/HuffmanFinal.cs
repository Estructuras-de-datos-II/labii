using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace HuffmanTest
{
    public class HuffmanFinal
    {
        public class Comprimir
        {
            List<NodoHuffman> listaDeNodos;
            Monticulo<NodoHuffman> monticulo = new Monticulo<NodoHuffman>();
            List<NodoHuffman> listaDeNodos2 = null;
            MetodosHuffman MHF = new MetodosHuffman();
            public string mensajeComprimido = "";

            public Comprimir(string mensajeAcomprimir)
            {
                
                    listaDeNodos = MHF.C_obtenerFrecuencias(mensajeAcomprimir);
                    MHF.N_obtenerArbol(listaDeNodos, monticulo, ref listaDeNodos2);
                    MHF.N_definirCodigoBinario("", listaDeNodos2[0]);                    
                    //MHF.N_imprimir(listaDeNodos2[0]);                    

                    
                    string mensajeCodificado = null;
                    MHF.C_reescribirMensajeBinario(listaDeNodos, mensajeAcomprimir, ref mensajeCodificado);                    

                    List<string> listaNumerosB = new List<string>();
                    List<int> listaNumerosD = new List<int>();
                    MHF.C_convertirBinaDec(mensajeCodificado, ref listaNumerosB, ref listaNumerosD);

                    List<char> listaSimbolosAsc = new List<char>();
                    List<char> listaFrecuenciasAsc = new List<char>();
                    MHF.C_convertirDecaAsc(listaNumerosD, ref listaSimbolosAsc, listaDeNodos, ref listaFrecuenciasAsc);

                    string mensajeFinal = "";
                    mensajeComprimido = MHF.C_mensajeFinal(ref mensajeFinal, listaSimbolosAsc, listaDeNodos, listaFrecuenciasAsc);
                                                                                               
            }
        }
        public class Descomprimir
        {
            string mensajeEntrante;
            List<NodoHuffman> listaDeNodos = new List<NodoHuffman>();
            List<int> listaNumerosDecimales = new List<int>();
            Monticulo<NodoHuffman> monticulo = new Monticulo<NodoHuffman>();
            List<NodoHuffman> listaDeNodos2 = null;
            List<string> listaNumerosBinarios = new List<string>();
            string mensajeBinario;
            public string mensajeDescomprimido;
            public Descomprimir(string mensajeAdescomprimir)
            {
                mensajeEntrante = mensajeAdescomprimir;

                MetodosHuffman MHF = new MetodosHuffman();

                MHF.D_descomprimirMensaje(mensajeEntrante, ref listaDeNodos, ref listaNumerosDecimales, 0);

                MHF.N_obtenerArbol(listaDeNodos, monticulo, ref listaDeNodos2);
                MHF.N_definirCodigoBinario("", listaDeNodos2[0]);
                MHF.D_convertirDecaBin(listaNumerosDecimales, listaNumerosBinarios, ref mensajeBinario);
                MHF.D_rehacerMensajeOriginal(listaDeNodos, mensajeBinario, ref mensajeDescomprimido);
                
            }

        }
    }
}
