using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Security.Cryptography;
using System.Net;

namespace Laboratorio2_KB_CL
{
    public class LZW
    {
        Dictionary<string, int> LZWDicc = new Dictionary<string, int>();
        Dictionary<int, List<byte>> LZWDescompresionDicc = new Dictionary<int, List<byte>>();
        List<byte> difCaracteres = new List<byte>();
        List<byte> bits = new List<byte>();
        List<int> NumerosAescribir = new List<int>();
        List<List<byte>> valoresDeDescompresion = new List<List<byte>>();
        int MaxValorTam = 0;
        int codigo = 1;
        string sobrante = "";


        private void reiniciarVariables()
        {
            LZWDicc.Clear();
            LZWDescompresionDicc.Clear();
            difCaracteres.Clear();
            bits.Clear();
            NumerosAescribir.Clear();
            valoresDeDescompresion.Clear();
            sobrante = string.Empty;
            MaxValorTam = 0;
            codigo = 1;
        }

        //--------------------------------compresion------------------------------------------
        private void llenarDiccionario(byte[] Text)
        {
            foreach (var character in Text)
            {
                if (!LZWDicc.ContainsKey(character.ToString()))
                {
                    LZWDicc.Add(character.ToString(), codigo);
                    codigo++;
                    difCaracteres.Add(character);
                }
            }
        }

        private void compresion(byte[] Text)
        {
            bits = Text.ToList();
            MaxValorTam = 0;
            while (bits.Count != 0)
            {
                int tamCodigo = 0;
                string subcadena = bits[tamCodigo].ToString();
                tamCodigo++;
                while (subcadena.Length != 0)
                {
                    if (bits.Count > tamCodigo)
                    {
                        if (!LZWDicc.ContainsKey(subcadena + bits[tamCodigo].ToString()))
                        {
                            NumerosAescribir.Add(LZWDicc[subcadena]);
                            subcadena += bits[tamCodigo].ToString();
                            agregarDiccionario(subcadena);
                            subcadena = string.Empty;
                            for (int i = 0; i < tamCodigo; i++)
                            {
                                bits.RemoveAt(0);
                            }
                        }
                        else
                        {
                            subcadena += bits[tamCodigo].ToString();
                            tamCodigo++;
                        }
                    }
                    else
                    {
                        NumerosAescribir.Add(LZWDicc[subcadena]);
                        agregarDiccionario(subcadena);
                        for (int i = 0; i < tamCodigo; i++)
                        {
                            bits.RemoveAt(0);
                        }
                        subcadena = string.Empty;
                    }
                }
            }
        }

        private void agregarDiccionario(string key)
        {
            if (!LZWDicc.ContainsKey(key))
            {
                LZWDicc.Add(key, codigo);
                codigo++;
            }
        }

        public string CompressText(string text)
        {
            var temp = Conversor.ConvertToBytes(text);//falta repetir esto varias veces por si es un texto muy grande
            llenarDiccionario(temp);
            compresion(temp);
            MaxValorTam = Convert.ToString(NumerosAescribir.Max(), 2).Length;
            List<byte> listaFinal = new List<byte>
            {
                Convert.ToByte(MaxValorTam),
                Convert.ToByte(difCaracteres.Count())
            };
            foreach (var valor in difCaracteres)
            {
                listaFinal.Add(valor);
            }
            string codigo = string.Empty;
            foreach (var num in NumerosAescribir)
            {
                string subcodigo = Convert.ToString(num, 2);
                while (subcodigo.Length != MaxValorTam)
                {
                    subcodigo = "0" + subcodigo;
                }
                codigo += subcodigo;
                if (codigo.Length >= 8)
                {
                    listaFinal.Add(Convert.ToByte(codigo.Substring(0, 8), 2));
                    codigo = codigo.Remove(0, 8);
                }
            }
            if (codigo.Length != 0)
            {
                while (codigo.Length != 8)
                {
                    codigo += "0";
                }
                listaFinal.Add(Convert.ToByte(codigo, 2));
            }
            reiniciarVariables();
            return Conversor.ConvertToString(listaFinal.ToArray());
        }

        //-------------------------descompresion

        private byte[] DiccionarioDeDescompresion(byte[] text)
        {
            for (int i = 0; i < text[1]; i++)
            {
                LZWDescompresionDicc.Add(codigo, new List<byte> { text[i + 2] });
                codigo++;
            }
            var textoComprimido = new byte[text.Length - (2 + text[1])];
            for (int i = 0; i < textoComprimido.Length; i++)
            {
                textoComprimido[i] = text[2 + text[1] + i];
            }
            return textoComprimido;
        }

        private List<int> descompresion(byte[] textoComprimido)
        {
            List<int> codigos = new List<int>();
            string numBinario = sobrante;
            valoresDeDescompresion.Add(new List<byte>());
            valoresDeDescompresion.Add(new List<byte>());
            valoresDeDescompresion.Add(new List<byte>());
            foreach (var valor in textoComprimido)
            {
                string binarioTemp = Convert.ToString(valor, 2);
                while (binarioTemp.Length < 8)
                {
                    binarioTemp = "0" + binarioTemp;
                }
                numBinario += binarioTemp;
                while (numBinario.Length >= MaxValorTam)
                {
                    var indice = Convert.ToInt32(numBinario.Substring(0, MaxValorTam), 2);
                    numBinario = numBinario.Remove(0, MaxValorTam);
                    if (indice != 0)
                    {
                        codigos.Add(indice);
                        valoresDeDescompresion[0] = valoresDeDescompresion[1];
                        if (LZWDescompresionDicc.ContainsKey(indice))
                        {
                            valoresDeDescompresion[1] = valoresDescompresion(LZWDescompresionDicc[indice]);
                            valoresDeDescompresion[2].Clear();
                            foreach (var val in valoresDeDescompresion[0])
                            {
                                valoresDeDescompresion[2].Add(val);
                            }
                            valoresDeDescompresion[2].Add(valoresDeDescompresion[1][0]);
                        }
                        else
                        {
                            valoresDeDescompresion[1] = valoresDeDescompresion[0];
                            valoresDeDescompresion[2].Clear();
                            foreach (var val in valoresDeDescompresion[0])
                            {
                                valoresDeDescompresion[2].Add(val);
                            }
                            valoresDeDescompresion[2].Add(valoresDeDescompresion[1][0]);
                            valoresDeDescompresion[1] = valoresDescompresion(valoresDeDescompresion[2]);
                        }
                        if (!verificar(valoresDeDescompresion[2]))
                        {
                            LZWDescompresionDicc.Add(codigo, new List<byte>(valoresDeDescompresion[2]));
                            codigo++;
                        }
                    }
                }
            }
            valoresDeDescompresion.Clear();
            sobrante = numBinario;
            return codigos;
        }

        private List<byte> valoresDescompresion(List<byte> valores)
        {
            List<byte> listaApoyo = new List<byte>();
            foreach (var valor in valores)
            {
                listaApoyo.Add(valor);
            }
            return listaApoyo;
        }

        private bool verificar(List<byte> stringAc)
        {
            foreach (var valor in LZWDescompresionDicc.Values)
            {
                if (stringAc.Count == valor.Count)
                {
                    if (compararBytes(stringAc, valor))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool compararBytes(List<byte> lista1, List<byte> lista2)
        {
            for (int i = 0; i < lista1.Count; i++)
            {
                if (lista1[i] != lista2[i])
                {
                    return false;
                }
            }
            return true;
        }

        public string descomprimirTexto(string text)
        {
            var temp = Conversor.ConvertToBytes(text);
            MaxValorTam = temp[0];
            temp = DiccionarioDeDescompresion(temp);
            var indicesDescompresion = descompresion(temp);
            var bytesAescribir = new List<byte>();
            foreach (var indice in indicesDescompresion)
            {
                foreach (var valor in LZWDescompresionDicc[indice])
                {
                    bytesAescribir.Add(valor);
                }
            }
            return Conversor.ConvertToString(bytesAescribir.ToArray());
        }
    }
}