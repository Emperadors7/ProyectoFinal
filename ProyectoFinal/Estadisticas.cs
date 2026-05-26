using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoFinal
{
    internal class Estadisticas
    {
        //---Atributos Privados---
        private List<Abastecimiento> abastecimientos;
        //---Constructor---
        public Estadisticas(List<Abastecimiento> abastecimientos)
        {
            this.abastecimientos = abastecimientos;
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
            return FiltrarPorTipo("Prepago");
        }
        public List<Abastecimiento> InformeTanqueLleno()
        {
            return FiltrarPorTipo("Tanque Lleno");
        }
        public int BombaMasUtilizada()
        {
            return ObtenerBombaExtremo(true);
        }
        public int BombaMenosUtilizada()
        {
            return ObtenerBombaExtremo(false);
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
        //Filtrar por tipo
        private List<Abastecimiento> FiltrarPorTipo(string tipo)
        {
            List<Abastecimiento> resultado = new List<Abastecimiento>();
            foreach (var a in abastecimientos)
            {
                if (a.Tipo == tipo)
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
