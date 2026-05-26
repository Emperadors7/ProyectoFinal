using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using ProyectoFinal.Abastecimientos;

namespace ProyectoFinal
{
    internal class PanelCentral
    {
        // ----Atributos privados----
        private List<Bomba> bombas;
        private List<Abastecimiento> abastecimientos; 
        private PrecioCombustible precio;
        private Estadisticas estadisticas;
        private const string rutaArchivo = "abastecimientos.json";
        private const string rutaPrecio = "precio_dia.json";
        private int contadorId;

        // ----Propiedades públicas----
        public List<Bomba> Bombas { get { return bombas; } }
        public List<Abastecimiento> Abastecimientos { get { return abastecimientos; } }
        public PrecioCombustible Precio { get { return precio; } }

        // ----Constructor----
        public PanelCentral()
        {
            abastecimientos = new List<Abastecimiento>();
            bombas = new List<Bomba>();
            contadorId = 1;

            for (int i = 1; i <= 4; i++)
                bombas.Add(new Bomba(i, $"Bomba {i}"));

            CargarPrecio();

            CargarAbastecimientos();

            estadisticas = new Estadisticas(abastecimientos);
        }

        // ----Métodos públicos----

        // Iniciar abastecimiento prepago
        public async Task IniciarPrepago(string nombreCliente, int bombaId, decimal monto)
        {
            Bomba bomba = BuscarBomba(bombaId);

            if (bomba == null)
                throw new Exception($"No existe la bomba {bombaId}.");

            await bomba.IniciarDespachoAsync();

            Abastecimiento nuevo = new AbastecimientoPrepago(contadorId++, nombreCliente, bombaId, monto, precio);
            abastecimientos.Add(nuevo);
            GuardarAbastecimientos();
        }

        // Iniciar abastecimiento tanque lleno
        public async Task IniciarTanqueLleno(string nombreCliente, int bombaId)
        {
            Bomba bomba = BuscarBomba(bombaId);

            if (bomba == null)
                throw new Exception($"No existe la bomba {bombaId}.");

            await bomba.IniciarDespachoAsync();

            Abastecimiento nuevo = new AbastecimientoTanqueLleno(contadorId++, nombreCliente, bombaId);
            abastecimientos.Add(nuevo);
            GuardarAbastecimientos();
        }
        public async Task RecibirRespuestaArduino(string jsonRecibido)
        {
            try
            {
                RespuestaArduino respuesta = JsonSerializer.Deserialize<RespuestaArduino>(jsonRecibido);

                Bomba bomba = BuscarBomba(respuesta.BombaId);
                if (bomba == null) return;

                bomba.RegistrarLitros(respuesta.LitrosDespachados);

                Abastecimiento actual = BuscarUltimoAbastecimiento(respuesta.BombaId);
                if (actual != null)
                {
                    actual.RegistrarDespacho(respuesta.LitrosDespachados);

                    if (actual is AbastecimientoTanqueLleno)
                    {
                        actual.CantidadPagada = precio.CalcularCosto(respuesta.LitrosDespachados);
                    }

                    GuardarAbastecimientos();
                }

                await bomba.FinalizarSesionAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al procesar respuesta Arduino: {ex.Message}");
            }
        }

        // Actualizar precio del día
        public void ActualizarPrecio(decimal nuevoPrecio)
        {
            precio = new PrecioCombustible(nuevoPrecio);
            GuardarPrecio();
        }

        // Métodos de estadísticas
        public List<Abastecimiento> ObtenerCierreDiario(DateTime fecha)
        {
            return estadisticas.CierreCajaDiario(fecha);
        }

        public decimal ObtenerTotalDia(DateTime fecha)
        {
            return estadisticas.TotalRecaudado(fecha);
        }

        public List<Abastecimiento> ObtenerInformePrepagos()
        {
            return estadisticas.InformePrepagos();
        }

        public List<Abastecimiento> ObtenerInformeTanqueLleno()
        {
            return estadisticas.InformeTanqueLleno();
        }

        public int ObtenerBombaMasUsada()
        {
            return estadisticas.BombaMasUtilizada();
        }

        public int ObtenerBombaMenosUsada()
        {
            return estadisticas.BombaMenosUtilizada();
        }

        // ----Métodos privados----

        // Busca una bomba por ID
        private Bomba BuscarBomba(int id)
        {
            foreach (var b in bombas)
            {
                if (b.Id == id)
                    return b;
            }
            return null;
        }

        private Abastecimiento BuscarUltimoAbastecimiento(int bombaId)
        {
            Abastecimiento ultimo = null;
            foreach (var a in abastecimientos)
            {
                if (a.BombaId == bombaId && a.Estado == "pendiente")
                    ultimo = a;
            }
            return ultimo;
        }
        private void GuardarAbastecimientos()
        {
            try
            {
                string json = JsonSerializer.Serialize(abastecimientos);
                File.WriteAllText(rutaArchivo, json);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al guardar abastecimientos: {ex.Message}");
            }
        }

        private void CargarAbastecimientos()
        {
            try
            {
                if (File.Exists(rutaArchivo))
                {
                    string json = File.ReadAllText(rutaArchivo);
                    List<Abastecimiento> cargados = JsonSerializer.Deserialize<List<Abastecimiento>>(json);

                    if (cargados != null)
                    {
                        abastecimientos = cargados;
                        contadorId = abastecimientos.Count + 1;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al cargar abastecimientos: {ex.Message}");
            }
        }

        // Guarda el precio del día en archivo JSON
        private void GuardarPrecio()
        {
            try
            {
                string json = JsonSerializer.Serialize(precio);
                File.WriteAllText(rutaPrecio, json);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al guardar precio: {ex.Message}");
            }
        }

        // Carga el precio del día desde archivo JSON
        private void CargarPrecio()
        {
            try
            {
                if (File.Exists(rutaPrecio))
                {
                    string json = File.ReadAllText(rutaPrecio);
                    precio = JsonSerializer.Deserialize<PrecioCombustible>(json);
                }
                else
                {
                    precio = new PrecioCombustible(10);
                }
            }
            catch (Exception ex)
            {
                precio = new PrecioCombustible(10);
            }
        }
    }
    internal class RespuestaArduino
    {
        public int BombaId { get; set; }
        public decimal LitrosDespachados { get; set; }
        public string Estado { get; set; }
    }
}