using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BTree<T> : IEnumerable<T> where T : IComparable
{
    private readonly int maxCantLlaves;
    private readonly int minCantLlaves;

    internal BTreeNode<T> ruta;

    public int contador { get; private set; }

    public BTree(int grado)
    {
        maxCantLlaves = grado - 1;
        if (grado < 3)
        {
            throw new Exception("Él arbol debe ser mínimo de grado 3");
        }

        this.minCantLlaves = maxCantLlaves / 2;
    }

    //Comprueba si tiene algún valor 
    public bool existeValor(T valor)
    {
        return encontrar(ruta, valor) != null;
    }

    private BTreeNode<T> encontrar(BTreeNode<T> node, T valor)
    {

        //si demuestra que es una hoja entonces insertara
        if (node.comprobarHoja)
        {
            for (var i = 0; i < node.contadorDeLlaves; i++)
            {

                if (valor.CompareTo(node.llaves[i]) == 0)
                {
                    return node;
                }
            }
        }
        else
        {
            //si no es una hoja, llevar hasta una
            for (var i = 0; i < node.contadorDeLlaves; i++)
            {
                if (valor.CompareTo(node.llaves[i]) == 0)
                {
                    return node;
                }

                //valor actual < valor nuevo , enviar hijo izquierdo
                if (valor.CompareTo(node.llaves[i]) < 0)
                {
                    return encontrar(node.hijo[i], valor);
                }
                //valor actual > valor nuevo, valor actual es el último                
                if (node.contadorDeLlaves == i + 1)
                {
                    return encontrar(node.hijo[i + 1], valor);
                }
            }

        }
        return null;
    }

    public void insertar(T valorNuevo)
    {
        if (ruta == null)
        {
            ruta = new BTreeNode<T>(maxCantLlaves, null) { llaves = { [0] = valorNuevo } };
            ruta.contadorDeLlaves++;
            contador++;
            return;
        }

        var hojaInsertar = encontrarHojaInsercion(ruta, valorNuevo);
        insertarSeparar(ref hojaInsertar, valorNuevo, null, null);
        contador++;
    }   

//encontrar la hoja para iniciar la inserción
private BTreeNode<T> encontrarHojaInsercion(BTreeNode<T> node, T valorNuevo)
    {
        //si es hoja se inserta
        if (node.comprobarHoja)
        {
            return node;
        }

        for (var i = 0; i < node.contadorDeLlaves; i++)
        {
            if (valorNuevo.CompareTo(node.llaves[i]) < 0)
            {
                return encontrarHojaInsercion(node.hijo[i], valorNuevo);
            }
            if (node.contadorDeLlaves == i + 1)
            {
                return encontrarHojaInsercion(node.hijo[i + 1], valorNuevo);
            }

        }

        return node;
    }

    //insertar y separar recursivamente hasta que sea necesario
    private void insertarSeparar(ref BTreeNode<T> node, T valorNuevo, BTreeNode<T> nuevoValorIzquierda, BTreeNode<T> nuevoValorDerecha)
    {
        //agregar nuevo elemento al nodo 
        if (node == null)
        {
            node = new BTreeNode<T>(maxCantLlaves, null);
            ruta = node;
        }

        //el nuevo valor cabe, por lo que se inserta respetando orden
        if (node.contadorDeLlaves != maxCantLlaves)
        {
            insertarNodoNoLleno(ref node, valorNuevo, nuevoValorIzquierda, nuevoValorDerecha);
            return;
        }
        //si el nodo esta lleno separar y poner nuevo valor medio en la raiz

        //dividir valores actuales en los nodos y definir los nodos de la izquierda y la derecha
        var izquierda = new BTreeNode<T>(maxCantLlaves, null);
        var derecha = new BTreeNode<T>(maxCantLlaves, null);

        var indiceMedioActual = node.obtenerIndiceMedio();

        var nodoActual = izquierda;
        var indiceNodoActual = 0;

        var nuevoMedio = default(T);
        var nuevoMedianoTF = false;
        var valorNuevoInsertado = false;

        var contadorInsertado = 0;

        //insertar de manera ordenada y definir valor medio
        for (var i = 0; i < node.contadorDeLlaves; i++)
        {
            if (!nuevoMedianoTF && contadorInsertado == indiceMedioActual)
            {
                nuevoMedianoTF = true;

                if (!valorNuevoInsertado && valorNuevo.CompareTo(node.llaves[i]) < 0)
                {
                    nuevoMedio = valorNuevo;
                    valorNuevoInsertado = true;

                    if (nuevoValorIzquierda != null)
                    {
                        definirHijo(nodoActual, nodoActual.contadorDeLlaves, nuevoValorIzquierda);
                    }

                    nodoActual = derecha;
                    indiceNodoActual = 0;

                    if (nuevoValorDerecha != null)
                    {
                        definirHijo(nodoActual, 0, nuevoValorDerecha);
                    }

                    i--;
                    contadorInsertado++;
                    continue;
                }
                nuevoMedio = node.llaves[i];
                nodoActual = derecha;
                indiceNodoActual = 0;

                continue;

            }

            if (valorNuevoInsertado || node.llaves[i].CompareTo(valorNuevo) < 0)
            {
                nodoActual.llaves[indiceNodoActual] = node.llaves[i];
                nodoActual.contadorDeLlaves++;

                if (nodoActual.hijo[indiceNodoActual] == null)
                {
                    definirHijo(nodoActual, indiceNodoActual, node.hijo[i]);
                }

                definirHijo(nodoActual, indiceNodoActual + 1, node.hijo[i + 1]);

            }
            else
            {
                nodoActual.llaves[indiceNodoActual] = valorNuevo;
                nodoActual.contadorDeLlaves++;

                definirHijo(nodoActual, indiceNodoActual, nuevoValorIzquierda);
                definirHijo(nodoActual, indiceNodoActual + 1, nuevoValorDerecha);

                i--;
                valorNuevoInsertado = true;
            }

            indiceNodoActual++;
            contadorInsertado++;
        }

        if (!valorNuevoInsertado)
        {
            nodoActual.llaves[indiceNodoActual] = valorNuevo;
            nodoActual.contadorDeLlaves++;

            definirHijo(nodoActual, indiceNodoActual, nuevoValorIzquierda);
            definirHijo(nodoActual, indiceNodoActual + 1, nuevoValorDerecha);
        }
        var raiz = node.raiz;
        insertarSeparar(ref raiz, nuevoMedio, izquierda, derecha);

    }

    private void insertarNodoNoLleno(ref BTreeNode<T> node, T valorNuevo, BTreeNode<T> nuevoValorIzquierda, BTreeNode<T> nuevoValorDerecha)
    {
        var bInsertado = false;

        for (var i = 0; i < node.contadorDeLlaves; i++)
        {
            if (valorNuevo.CompareTo(node.llaves[i]) >= 0)
            {
                continue;
            }
            insertarEn(node.llaves, i, valorNuevo);
            node.contadorDeLlaves++;

            definirHijo(node, i, nuevoValorIzquierda);
            insertarHijo(node, i + 1, nuevoValorDerecha);

            bInsertado = true;
            break;
        }

        if (bInsertado)
        {
            return;
        }

        node.llaves[node.contadorDeLlaves] = valorNuevo;
        node.contadorDeLlaves++;

        definirHijo(node, node.contadorDeLlaves - 1, nuevoValorIzquierda);
        definirHijo(node, node.contadorDeLlaves, nuevoValorDerecha);
    }

    public void eliminar(T valor)
    {
        var node = encontrarNodoEliminar(ruta, valor);

        if (node == null)
        {
            throw new Exception("No existe el valor");
        }

        for (var i = 0; i < node.contadorDeLlaves; i++)
        {
            if (valor.CompareTo(node.llaves[i]) != 0)
            {
                continue;
            }
            if (node.comprobarHoja)
            {
                eliminarEn(node.llaves, i);
                node.contadorDeLlaves--;
                balancear(node);
            }
            else
            {
                var nodoMaximo = encontrarNodoMaximo(node.hijo[i]);
                node.llaves[i] = nodoMaximo.llaves[nodoMaximo.contadorDeLlaves - 1];
                eliminarEn(nodoMaximo.llaves, nodoMaximo.contadorDeLlaves - 1);
                nodoMaximo.contadorDeLlaves--;
                balancear(nodoMaximo);
            }
            contador--;
            return;
        }
    }

    private BTreeNode<T> encontrarNodoMinimo(BTreeNode<T> node)
    {
        return node.comprobarHoja ? node : encontrarNodoMinimo(node.hijo[0]);
    }

    private BTreeNode<T> encontrarNodoMaximo(BTreeNode<T> node)
    {
        return node.comprobarHoja ? node : encontrarNodoMaximo(node.hijo[node.contadorDeLlaves]);
    }

    private void balancear(BTreeNode<T> node)
    {
        if (node == ruta || node.contadorDeLlaves >= minCantLlaves)
        {
            return;
        }

        var hermanoDerecho = obtenerHermanoDerecho(node);

        if (hermanoDerecho != null
            && hermanoDerecho.contadorDeLlaves > minCantLlaves)
        {
            rotarIzquierda(node, hermanoDerecho);
            return;
        }

        var hermanoIzquierdo = obtenerHermanoIzquierdo(node);

        if (hermanoIzquierdo != null
            && hermanoIzquierdo.contadorDeLlaves > minCantLlaves)
        {
            rotarDerecha(hermanoIzquierdo, node);
            return;
        }

        if (hermanoDerecho != null)
        {
            juntarHermanos(node, hermanoDerecho);
        }
        else
        {
            juntarHermanos(hermanoIzquierdo, node);
        }
    }

    //ordena hermanos 
    private void juntarHermanos(BTreeNode<T> hermanoIzquierdo, BTreeNode<T> hermanoDerecho)
    {
        var indiceSeparador = obtenerIndiceSeparador(hermanoIzquierdo);
        var raiz = hermanoIzquierdo.raiz;

        var nuevoNodo = new BTreeNode<T>(maxCantLlaves, hermanoIzquierdo.raiz);
        var nuevoIndice = 0;

        for (var i = 0; i < hermanoIzquierdo.contadorDeLlaves; i++)
        {
            nuevoNodo.llaves[nuevoIndice] = hermanoIzquierdo.llaves[i];

            if (hermanoIzquierdo.hijo[i] != null)
            {
                definirHijo(nuevoNodo, nuevoIndice, hermanoIzquierdo.hijo[i]);
            }

            if (hermanoIzquierdo.hijo[i + 1] != null)
            {
                definirHijo(nuevoNodo, nuevoIndice + 1, hermanoIzquierdo.hijo[i + 1]);
            }

            nuevoIndice++;
        }

        if (hermanoIzquierdo.contadorDeLlaves == 0 && hermanoIzquierdo.hijo[0] != null)
        {
            definirHijo(nuevoNodo, nuevoIndice, hermanoIzquierdo.hijo[0]);
        }

        nuevoNodo.llaves[nuevoIndice] = raiz.llaves[indiceSeparador];
        nuevoIndice++;

        for (var i = 0; i < hermanoDerecho.contadorDeLlaves; i++)
        {
            nuevoNodo.llaves[nuevoIndice] = hermanoDerecho.llaves[i];

            if (hermanoDerecho.hijo[i] != null)
            {
                definirHijo(nuevoNodo, nuevoIndice, hermanoDerecho.hijo[i]);
            }

            if (hermanoDerecho.hijo[i + 1] != null)
            {
                definirHijo(nuevoNodo, nuevoIndice + 1, hermanoDerecho.hijo[i + 1]);
            }

            nuevoIndice++;
        }

        if (hermanoDerecho.contadorDeLlaves == 0 && hermanoDerecho.hijo[0] != null)
        {
            definirHijo(nuevoNodo, nuevoIndice, hermanoDerecho.hijo[0]);
        }
        nuevoNodo.contadorDeLlaves = nuevoIndice;
        definirHijo(raiz, indiceSeparador, nuevoNodo);
        eliminarEn(raiz.llaves, indiceSeparador);
        raiz.contadorDeLlaves--;

        eliminarHijo(raiz, indiceSeparador + 1);


        if (raiz.contadorDeLlaves == 0
            && raiz == ruta)
        {
            ruta = nuevoNodo;
            ruta.raiz = null;

            if (ruta.contadorDeLlaves == 0)
            {
                ruta = null;
            }

            return;
        }

        if (raiz.contadorDeLlaves < minCantLlaves)
        {
            balancear(raiz);
        }
    }

    private void rotarDerecha(BTreeNode<T> hermanoIzquierdo, BTreeNode<T> hermanoDerecho)
    {
        var raizindice = obtenerIndiceSeparador(hermanoIzquierdo);

        insertarEn(hermanoDerecho.llaves, 0, hermanoDerecho.raiz.llaves[raizindice]);
        hermanoDerecho.contadorDeLlaves++;

        insertarHijo(hermanoDerecho, 0, hermanoIzquierdo.hijo[hermanoIzquierdo.contadorDeLlaves]);

        hermanoDerecho.raiz.llaves[raizindice] = hermanoIzquierdo.llaves[hermanoIzquierdo.contadorDeLlaves - 1];

        eliminarEn(hermanoIzquierdo.llaves, hermanoIzquierdo.contadorDeLlaves - 1);
        hermanoIzquierdo.contadorDeLlaves--;

        eliminarHijo(hermanoIzquierdo, hermanoIzquierdo.contadorDeLlaves + 1);
    }
    private void rotarIzquierda(BTreeNode<T> hermanoIzquierdo, BTreeNode<T> hermanoDerecho)
    {
        var raizindice = obtenerIndiceSeparador(hermanoIzquierdo);
        hermanoIzquierdo.llaves[hermanoIzquierdo.contadorDeLlaves] = hermanoIzquierdo.raiz.llaves[raizindice];
        hermanoIzquierdo.contadorDeLlaves++;

        definirHijo(hermanoIzquierdo, hermanoIzquierdo.contadorDeLlaves, hermanoDerecho.hijo[0]);

        hermanoIzquierdo.raiz.llaves[raizindice] = hermanoDerecho.llaves[0];

        eliminarEn(hermanoDerecho.llaves, 0);
        hermanoDerecho.contadorDeLlaves--;

        eliminarHijo(hermanoDerecho, 0);

    }

    private BTreeNode<T> encontrarNodoEliminar(BTreeNode<T> node, T valor)
    {
        if (node.comprobarHoja)
        {
            for (var i = 0; i < node.contadorDeLlaves; i++)
            {
                if (valor.CompareTo(node.llaves[i]) == 0)
                {
                    return node;
                }
            }
        }
        else
        {
            for (var i = 0; i < node.contadorDeLlaves; i++)
            {
                if (valor.CompareTo(node.llaves[i]) == 0)
                {
                    return node;
                }

                if (valor.CompareTo(node.llaves[i]) < 0)
                {
                    return encontrarNodoEliminar(node.hijo[i], valor);
                }
                if (node.contadorDeLlaves == i + 1)
                {
                    return encontrarNodoEliminar(node.hijo[i + 1], valor);
                }
            }
        }
        return null;
    }

    private int obtenerIndiceSeparador(BTreeNode<T> node)
    {
        var raiz = node.raiz;

        if (node.indice == 0)
        {
            return 0;
        }

        if (node.indice == raiz.contadorDeLlaves)
        {
            return node.indice - 1;
        }

        return node.indice;

    }

    private BTreeNode<T> obtenerHermanoDerecho(BTreeNode<T> node)
    {
        var raiz = node.raiz;

        return node.indice == raiz.contadorDeLlaves ? null : raiz.hijo[node.indice + 1];
    }

    private BTreeNode<T> obtenerHermanoIzquierdo(BTreeNode<T> node)
    {
        return node.indice == 0 ? null : node.raiz.hijo[node.indice - 1];
    }

    private void definirHijo(BTreeNode<T> raiz, int hijoIndice, BTreeNode<T> hijo)
    {
        raiz.hijo[hijoIndice] = hijo;

        if (hijo == null)
        {
            return;
        }

        hijo.raiz = raiz;
        hijo.indice = hijoIndice;

    }

    private void insertarHijo(BTreeNode<T> raiz, int hijoIndice, BTreeNode<T> hijo)
    {
        insertarEn(raiz.hijo, hijoIndice, hijo);

        if (hijo != null)
        {
            hijo.raiz = raiz;
        }

        for (var i = hijoIndice; i <= raiz.contadorDeLlaves; i++)
        {
            if (raiz.hijo[i] != null)
            {
                raiz.hijo[i].indice = i;
            }
        }
    }

    private void eliminarHijo(BTreeNode<T> raiz, int hijoIndice)
    {
        eliminarEn(raiz.hijo, hijoIndice);

        for (var i = hijoIndice; i <= raiz.contadorDeLlaves; i++)
        {
            if (raiz.hijo[i] != null)
            {
                raiz.hijo[i].indice = i;
            }
        }
    }

    private void insertarEn<TS>(TS[] array, int indice, TS valorNuevo)
    {
        Array.Copy(array, indice, array, indice + 1, array.Length - indice - 1);
        array[indice] = valorNuevo;
    }

    private void eliminarEn<TS>(TS[] array, int indice)
    {
        Array.Copy(array, indice + 1, array, indice, array.Length - indice - 1);
    }

    public void resetear()
    {        
        
        
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public IEnumerator<T> GetEnumerator()
    {
        return new BTreeEnumerator<T>(ruta);
    }

}

public abstract class BNode<T> where T : IComparable
{
    internal int indice;

    internal T[] llaves { get; set; }
    internal int contadorDeLlaves;

    internal abstract BNode<T> obtenerRaiz();
    internal abstract BNode<T>[] obtenerHijo();

    internal BNode(int maxCantLlaves)
    {
        llaves = new T[maxCantLlaves];
    }

    internal int obtenerIndiceMedio()
    {
        return (contadorDeLlaves / 2);
    }
}

public class BTreeNode<T> : BNode<T> where T : IComparable
{

    internal BTreeNode<T> raiz { get; set; }
    internal BTreeNode<T>[] hijo { get; set; }

    internal bool comprobarHoja => hijo[0] == null;

    internal BTreeNode(int maxCantLlaves, BTreeNode<T> raiz)
        : base(maxCantLlaves)
    {

        raiz = raiz;
        hijo = new BTreeNode<T>[maxCantLlaves + 1];

    }

    internal override BNode<T> obtenerRaiz()
    {
        return raiz;
    }

    internal override BNode<T>[] obtenerHijo()
    {
        return hijo;
    }
}

public class BTreeEnumerator<T> : IEnumerator<T> where T : IComparable
{
    private  BTreeNode<T> ruta;
    public Stack<BTreeNode<T>> progreso;

    public BTreeNode<T> current;
    private int indice;

    internal BTreeEnumerator(BTreeNode<T> ruta)
    {
        this.ruta = ruta;
    }

    public bool MoveNext()
    {
        if (ruta == null)
        {
            return false;
        }

        if (progreso == null)
        {
            current = ruta;
            progreso = new Stack<BTreeNode<T>>(ruta.hijo.Take(ruta.contadorDeLlaves + 1).Where(x => x != null));
            return current.contadorDeLlaves > 0;
        }

        if (current != null && indice + 1 < current.contadorDeLlaves)
        {
            indice++;
            return true;
        }

        if (progreso.Count > 0)
        {
            indice = 0;

            current = progreso.Pop();

            foreach (var hijo in current.hijo.Take(current.contadorDeLlaves + 1).Where(x => x != null))
            {
                progreso.Push(hijo);
            }

            return true;
        }

        return false;
    }

    public void Reset()
    {
        progreso = null;
        current = null;
        indice = 0;
    }

    object IEnumerator.Current => Current;

    public T Current
    {
        get
        {
            return current.llaves[indice];
        }
    }

    public void Dispose()
    {
        progreso = null;
    }
}