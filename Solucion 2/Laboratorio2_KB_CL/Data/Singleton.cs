using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laboratorio2_KB_CL.Models;

namespace Laboratorio2_KB_CL.Data
{
    public sealed class Singleton
    {
        private readonly static Singleton _instance = new Singleton();
        public string mpdescomprimir;
        public string textoparacomprimir;
        public string mensajecomp;
        public string mensajedescomp;
        public HuffmanFinal.Comprimir comp;

        private Singleton()
        {
            comp = new HuffmanFinal.Comprimir(textoparacomprimir);
        }

        public static Singleton Instance
        {
            get
            {
                return _instance;
            }
        }
    }
}
