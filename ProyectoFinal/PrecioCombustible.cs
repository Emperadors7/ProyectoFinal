using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoFinal
{
    internal class PrecioCombustible
    {
        private decimal precioPorLitro;
        private DateTime fechaActual;
        public decimal PrecioPorLitro
        {
            get { return precioPorLitro; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("El precio por litro debe ser mayor que cero.");
                }
                precioPorLitro = value;
            }
        }
        public DateTime FechaActual
        {
            get { return fechaActual; }
        }

        //Constructor Con precio inicial
        public PrecioCombustible(decimal precioInicial)
        {
            PrecioPorLitro = precioInicial;
            fechaActual = DateTime.Now;
        }

        public PrecioCombustible() { }

        public decimal CalcularLitros(decimal cantidadIngresada)
        {
            if (cantidadIngresada < 0)
            {
                throw new ArgumentException("La cantidad ingresada no puede ser negativa.");
            }
            return cantidadIngresada / PrecioPorLitro;
        }
        public decimal CalcularCosto(decimal litros)
        {
            if (litros < 0)
            {
                throw new ArgumentException("La cantidad de litros no puede ser negativa.");
            }
            return PrecioPorLitro * litros;
        }
    }   
}