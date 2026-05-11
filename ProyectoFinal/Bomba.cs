using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoFinal
{
    internal class Bomba
    {
        // Atributos de la bomba
        private int id;
        private string nombre;
        private bool estaDisponible;
        private decimal litrosSurtidos;

        //propiedades
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

        public bool EstaDisponible { get { return estaDisponible; } }

        public decimal LitrosSurtidos { get { return litrosSurtidos; } }

        public Bomba (int id, string nombre)
        {
            this.id = id;
            Nombre = nombre;
            estaDisponible = false;
            litrosSurtidos = 0;
        }

        public Bomba() { }

        public void IniciarSurtido()
        {
            if (estaDisponible)
            {
                throw new InvalidOperationException("La bomba ya está en uso.");
            }
            estaDisponible = true;
            litrosSurtidos = 0;
        }

        public void DetenerSurtido()
        {
            if (!estaDisponible)
            {
                throw new InvalidOperationException("La bomba no está en uso.");
            }
            estaDisponible = false;
        }

        public void RegistrarLitros(decimal litros)
        {
            if (litros < 0 ) 
                throw new ArgumentException("La cantidad de litros no puede ser negativa.");
            litrosSurtidos += litros;
        }
        private void ResetearLitros()
        {
            litrosSurtidos = 0;
        }
        public void ReiniciarBomba()
        {
            DetenerSurtido();
            ResetearLitros();
        }
    }
}
