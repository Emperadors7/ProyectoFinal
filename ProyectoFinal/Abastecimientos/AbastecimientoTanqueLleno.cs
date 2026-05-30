using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoFinal.Abastecimientos
{
    internal class AbastecimientoTanqueLleno : Abastecimiento
    {
        public decimal PrecioPorLitro { get; set; }
        public AbastecimientoTanqueLleno(int id, int clienteId, int bombaId, PrecioCombustible precio)
        {
            Id = id;
            ClienteId = clienteId;
            BombaId = bombaId;
            Fecha = DateTime.Now;
            Estado = "pendiente";
            LitrosDespachados = 0;
            CantidadPagada = 0;
            PrecioPorLitro = precio.PrecioPorLitro;
        }

        public AbastecimientoTanqueLleno() { }

        public override void RegistrarDespacho(decimal litrosRecibidos)
        {
            LitrosDespachados = litrosRecibidos;
            CantidadPagada = PrecioPorLitro * LitrosDespachados;
            ActualizarEstado();
        }

        public override void ActualizarEstado()
        {
            Estado = "completo";
        }
    }
}
