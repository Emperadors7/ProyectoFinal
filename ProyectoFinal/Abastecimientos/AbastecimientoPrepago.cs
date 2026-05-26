using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoFinal.Abastecimientos
{
    internal class AbastecimientoPrepago : Abastecimiento
    {
        public decimal LitrosSolicitados { get; set; }

        public AbastecimientoPrepago(int id, string nombreCliente, int bombaId, decimal cantidadPagada, PrecioCombustible precio)
        {
            Id = id;
            NombreCliente = nombreCliente;
            BombaId = bombaId;
            Fecha = DateTime.Now;
            Estado = "pendiente";

            CantidadPagada = cantidadPagada;
            LitrosSolicitados = precio.CalcularLitros(CantidadPagada);
            LitrosDespachados = 0;
        }

        public override void ActualizarEstado()
        {
            Estado = LitrosDespachados >= LitrosSolicitados ? "completo" : "incompleto";
        }
    }
}
