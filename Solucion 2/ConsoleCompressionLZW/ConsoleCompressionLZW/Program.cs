using Compress;
using System;
using System.IO;

namespace ConsoleCompressionLZW
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Compresor y Descompresor LZW\n\n");
                Console.WriteLine("Escribe el path y el nombre del archivo a comprimir");
                string filePath = Console.ReadLine();

                byte[] data = File.ReadAllBytes(filePath);

                byte[] compressedData = LZWCompresscs.LzwCompress(data);
                string fileName = filePath.Substring(0, filePath.IndexOf('.'));
                createFileE(compressedData, fileName);

                byte[] descompressedData = LZWCompresscs.LzwDecompress(compressedData);
                createFileD(descompressedData, fileName);

                Console.WriteLine("Compresion Completada. Presione una tecla para continuar...");
                Console.ReadKey();
                Console.WriteLine("Desompresion Completada. Presione una tecla para terminar...");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine("Ubicacion o nombre del archivo incorrectos...");
                Console.ReadKey();
            }
        }

        static void createFileE(byte[] data, string pathName)
        {
            using (FileStream fs = File.Create(pathName + "-compress-LZW.txt"))
            {
                // Add some text to file    
                fs.Write(data, 0, data.Length);

            }
        }

        static void createFileD(byte[] data, string pathName)
        {
            using (FileStream fs = File.Create(pathName + "-descompress-LZW.txt"))
            {
                fs.Write(data, 0, data.Length);

            }
        }

    }
}
