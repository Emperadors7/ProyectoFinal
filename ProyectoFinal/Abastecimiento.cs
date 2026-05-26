using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoFinal
{
    internal class Abastecimiento
    {
        private int id;
        private string nombreCliente;
        private int bombaId;
        private string tipo;
        private decimal cantidadPagada;
        private decimal litrosSolicitados;
        private decimal litrosDespachados;
        private DateTime fecha;
        private string estado;

        //----Propiedades Publicas----
        public int Id 
        { 
            get { return id; }
            set { id = value; }
        }

        public string NombreCliente
        {
            get { return nombreCliente; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("El nombre del cliente no puede estar vacío.");
                }
                nombreCliente = value;
            }
        }
        public int BombaId
        {
            get { return bombaId; }
            set { bombaId = value; }
        }

        public string Tipo
        {
            get { return tipo; }
            set
            {
                if (value != "prepago" && value != "tanque lleno")   
                    throw new ArgumentException("El tipo debe ser 'prepago' o 'tanque lleno'.");
                tipo = value;
            }
        }
        public decimal CantidadPagada
        {
            get { return cantidadPagada; }
            set
            {
                if (value < 0)
                {
                throw new ArgumentException("La cantidad pagada no puede ser negativa.");
                }
                cantidadPagada = value;
            }
        }
        public decimal LitrosSolicitados
        {
            get { return litrosSolicitados;  }
            set { litrosSolicitados = value; }
        }
        public decimal LitrosDespachados
        {
            get { return litrosDespachados; }
            set { litrosDespachados = value; }
        }

        public DateTime Fecha 
        { 
            get { return fecha; }
            set { fecha = value; }
        }

        public string Estado 
        { 
            get { return estado; }
            set { estado = value; }
        }

        //Contructror para prepago
        public Abastecimiento(int id, string nombreCliente, int bombaId, decimal cantidadPagada, PrecioCombustible precio)
        {
            this.id = id;
            NombreCliente = nombreCliente;
            BombaId = bombaId;
            Tipo = "prepago";
            CantidadPagada = cantidadPagada;
            LitrosSolicitados = precio.CalcularLitros(CantidadPagada);
            LitrosDespachados = 0;
            fecha = DateTime.Now;
            estado = "pendiente";
        }
        //Constructor para tanque lleno
        public Abastecimiento(int id, string nombreCliente, int bombaId )
        {
            this.id = id;
            NombreCliente = nombreCliente;
            BombaId = bombaId;
            Tipo = "tanque lleno";
            CantidadPagada = 0;
            LitrosSolicitados = 0;
            LitrosDespachados = 0;
            fecha = DateTime.Now;
            estado = "pendiente";
        }
        public Abastecimiento() { }

        public void RegistrarDespacho(decimal litrosRecibidos)
        {
            litrosDespachados = litrosRecibidos;
            ActualizarEstado();

            if (Tipo == "tanque lleno")
            {
                CantidadPagada = litrosDespachados;
            }
        }
        public void ActualizarIncompleto(decimal litrosRecibidos)
        {
            if (tipo == "prepago" && litrosRecibidos < LitrosSolicitados)
            {
                litrosDespachados = litrosRecibidos;
                ActualizarEstado();
            }
        }
        private void ActualizarEstado()
        {
            if (tipo == "prepago") 
                estado = litrosDespachados >= litrosSolicitados ? "completo" : "incompleto";
            else
            {
                estado = "completo";
            }
        }
        private decimal CalcularDiferencia()
        {
            return LitrosSolicitados - LitrosDespachados;
        }
        //Archivo JSON para guardar los abastecimientos
        public string ToJson()
        {
            return $"{{" +
                   $"\"id\": {id}, " +
                   $"\"cliente\": \"{nombreCliente}\", " +
                   $"\"bomba_id\": {bombaId}, " +
                   $"\"tipo\": \"{tipo}\", " +
                   $"\"monto_pagado\": {CantidadPagada}, " +
                   $"\"litros_solicitados\": {litrosSolicitados}, " +
                   $"\"litros_despachados\": {litrosDespachados}, " +
                   $"\"fecha\": \"{fecha:yyyy-MM-dd}\", " +
                   $"\"hora\": \"{fecha:HH:mm:ss}\", " +
                   $"\"estado\": \"{estado}\"" +
                   $"}}";
        }
    }
}
