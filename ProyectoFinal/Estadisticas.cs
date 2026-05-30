using System;
using System.Collections.Generic;
using System.Text;
using ProyectoFinal.Abastecimientos;

namespace ProyectoFinal
{
    internal class Estadisticas
    {
        //---Atributos Privados---
        private List<Abastecimiento> abastecimientos;
        private List<Clientes> clientes;
        //---Constructor---
        public Estadisticas(List<Abastecimiento> abastecimientos, List<Clientes> clientes)
        {
            this.abastecimientos = abastecimientos;
            this.clientes = clientes;
        }
        //---Metodos Publicos---
        public List<Abastecimiento> CierreCajaDiario(DateTime fecha)
        {
            return FiltrarPorFecha(fecha);
        }
        public decimal TotalRecaudado(DateTime fecha)
        {
            List<Abastecimiento> delDia = FiltrarPorFecha(fecha);
            decimal total = 0;
            foreach (var a in delDia)
                total += a.CantidadPagada;
            return total;
        }
        public List<Abastecimiento> InformePrepagos()
        {
            List<Abastecimiento> resultado = new List<Abastecimiento>();
            foreach (var a in abastecimientos)
            {
                if (a is AbastecimientoPrepago)
                {
                    resultado.Add(a);
                }
            }
            return resultado;
        }
        public List<Abastecimiento> InformeTanqueLleno()
        {
            List<Abastecimiento> resultado = new List<Abastecimiento>();
            foreach (var a in abastecimientos)
            {
                if (a is AbastecimientoTanqueLleno)
                {
                    resultado.Add(a);
                }
            }
            return resultado;
        }
        public int BombaMasUtilizada()
        {
            return ObtenerBombaExtremo(true);
        }
        public int BombaMenosUtilizada()
        {
            return ObtenerBombaExtremo(false);
        }
        public List<Abastecimiento> HistorialPorCliente(string nit)
        {
            foreach (var c in clientes)
            {
                if (c.NIT == nit)
                    return c.ObtenerHistorial();
            }
            return new List<Abastecimiento>();
        }

        //---Metodos Privados---
        //Filtrar por fecha
        private List<Abastecimiento> FiltrarPorFecha(DateTime fecha)
        {
            List<Abastecimiento> resultado = new List<Abastecimiento>();
            foreach (var a in abastecimientos)
            {
                if (a.Fecha.Date == fecha.Date)
                    resultado.Add(a);
            }
            return resultado;
        }
        private int ObtenerBombaExtremo(bool mayor)
        {
            Dictionary<int, int> conteo = new Dictionary<int, int>();
            foreach (var a in abastecimientos)
            {
                if (conteo.ContainsKey(a.BombaId))
                    conteo[a.BombaId]++;
                else
                    conteo[a.BombaId] = 1;
            }
            int bombaId = -1;
            int extremo = mayor ? int.MinValue : int.MaxValue;

            foreach (var par in conteo)
            {
                if (mayor && par.Value > extremo)
                {
                    extremo = par.Value;
                    bombaId = par.Key;
                }
                else if (!mayor && par.Value < extremo)
                {
                    extremo = par.Value;
                    bombaId = par.Key;
                }
            }
            return bombaId;
        }
    }
    
}
