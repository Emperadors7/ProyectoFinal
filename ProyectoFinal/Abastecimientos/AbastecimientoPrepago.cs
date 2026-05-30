using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoFinal.Abastecimientos
{
    internal class AbastecimientoPrepago : Abastecimiento
    {
        public decimal LitrosSolicitados { get; set; }

        public AbastecimientoPrepago(int id, int clienteId, int bombaId, decimal cantidadPagada, PrecioCombustible precio)
        {
            Id = id;
            ClienteId = clienteId;
            BombaId = bombaId;
            Fecha = DateTime.Now;
            Estado = "pendiente";
            CantidadPagada = cantidadPagada;
            LitrosSolicitados = precio.CalcularLitros(CantidadPagada);
            LitrosDespachados = 0;
        }
        public AbastecimientoPrepago() { }
        public override void RegistrarDespacho(decimal litrosRecibidos)
        {
            LitrosDespachados = litrosRecibidos;
            ActualizarEstado();
            if (Estado == "incompleto")
                CantidadPagada = LitrosDespachados * (CantidadPagada / LitrosSolicitados);
        }
        public override void ActualizarEstado()
        {
            Estado = LitrosDespachados >= LitrosSolicitados ? "completo" : "incompleto";
        }
    }
}
