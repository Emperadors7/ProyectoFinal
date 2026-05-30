using ProyectoFinal.Abastecimientos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ProyectoFinal
{
    internal class Clientes
    {
        //---Atributos Privados---
        private int id;
        private string nombre;
        private string nit;
        private string telefono;
        private List<Abastecimiento> historial;

        //---Atributos Publicos---
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public string Nombre
        {
            get { return nombre; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El nombre no puede estar vacío.");
                nombre = value;
            }
        }
        public string NIT
        {
            get { return nit; }
            set { nit = value; }
        }
        public string Telefono
        {
            get { return telefono; }
            set { telefono = value; }
        }
        [JsonIgnore]
        public List<Abastecimiento> Historial
        {
            get { return historial; }
        }

        //---Contructor---
        public Clientes(int id, string nombre, string nit, string telefono)
        {
            this.id = id;
            Nombre = nombre;
            this.nit = nit;
            Telefono = telefono;
            historial = new List<Abastecimiento>();
        }
        public Clientes()
        {
            historial = new List<Abastecimiento>(); // Importante inicializarlo aquí también
        }

        //----Metodos Publicos---
        public void AgregarAbastecimientos(Abastecimiento abastecimiento)
        {
            if (abastecimiento == null)
                throw new ArgumentNullException("El abastecimiento no puede ser nulo");
            historial.Add(abastecimiento);
        }
        public List<Abastecimiento> ObtenerHistorial()
        {
            return historial;
        }
        public string GenerarResumen()
        {
            return $"Cliente: {nombre} | " +
                   $"NIT: {nit} | " +
                   $"Visitas: {ContarVisitas()} | " +
                   $"Total gastado: Q{CalcularTotalGastos():F2}";
        }

        //---Métodos Privados---
        private decimal CalcularTotalGastos()
        {
            decimal total = 0;
            foreach (var a in historial)
                total += a.CantidadPagada;
            return total;
        }
        private int ContarVisitas()
        {
            return historial.Count;
        }
    }
}
