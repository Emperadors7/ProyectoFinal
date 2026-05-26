using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoFinal.Abastecimientos
{
    internal class AbastecimientoTanqueLleno : Abastecimiento
    {
        public AbastecimientoTanqueLleno(int id, string nombreCliente, int bombaId)
        {
            Id = id;
            NombreCliente = nombreCliente;
            BombaId = bombaId;
            Fecha = DateTime.Now;
            Estado = "pendiente";
            LitrosDespachados = 0;
        }

        public override void ActualizarEstado()
        {
            Estado = "completo";
        }
    }
}
