using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoFinal
{
    internal class Bomba
    {
        // ----Atributos de la bomba----
        private int id;
        private string nombre;
        private bool estaActivo;
        private decimal litrosDespachados;

        // ----Propiedades de la bomba----
        public int Id { get { return id; } }

        public string Nombre { get {  return nombre; } set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("El nombre no puede estar vacío.");
                }
                nombre = value;
            }
        }

        public bool EstaActivo { get { return estaActivo; } }

        public decimal LitrosDespachados { get { return litrosDespachados; } }

        public Bomba (int id, string nombre)
        {
            this.id = id;
            Nombre = nombre;
            estaActivo = false;
            litrosDespachados = 0;
        }

        public Bomba() { }

        //----Metodos publicos de la bomba----
        public async Task IniciarDespachoAsync()
        {
            if (estaActivo)
            {
                throw new InvalidOperationException($"La bomba {nombre} ya está en uso.");
            }
            estaActivo = true;
            ResetearLitros();
            await Task.Delay(100); //Pequeña pausa para no bloquear la interfaz
        }

        public async Task DetenerDespachoAsync()
        {
            if (!estaActivo)
            {
                throw new InvalidOperationException($"La bomba {nombre} no está en uso.");
            }
            estaActivo = false;
            await Task.Delay(100); 
        }

        public void RegistrarLitros(decimal litros)
        {
            if (litros < 0 ) 
                throw new ArgumentException("La cantidad de litros no puede ser negativa.");
            litrosDespachados += litros;
        }
        public async Task FinalizarSesionAsync()
        {
            await DetenerDespachoAsync();
            ResetearLitros();
        }
        public async Task ReiniciarBombaAsync()
        {
            estaActivo = false; // sin validación intencional
            ResetearLitros();
            await Task.Delay(100);
        }
        //----Metodos privados de la bomba----
        private void ResetearLitros()
        {
            litrosDespachados = 0;
        }

    }
}
