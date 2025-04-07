//                                  ┌∩┐(◣_◢)┌∩┐                                \\
//																				\\
// MDev.cs (19/04/2018)															\\
// Autor:         N9+  (.\@ninpl) 												\\
// Fecha Mod:     07/04/2025													\\
// Ultima Mod:    Migracion a Unity Engine 6.0+									\\
//******************************************************************************\\

#region Librerias
using UnityEngine;
using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
#endregion

namespace MoonType.MDev
{
	/// <summary>
	/// <para>Sistema central de la consola. Genera una IDC (Ingame Developer Console), con la que
	/// puedes llamar a comandos predefinidos anteriormente y llamarlos en tiempo de ejecucion.</para>
	/// </summary>
	public class MDev : MonoBehaviour 
	{
		#region Constantes
		/// <summary>
		/// <para>Ruta predeterminada del archivo de datos.</para>
		/// </summary>
		public const string RUTA_DATOS_PREDETERMINADOS = "Datos/MoonType";
		#endregion
		
		#region Globales
		/// <summary>
		/// <para>Instancia de <see cref="MDev"/>. Obtiene acceso a la API publica desde cualquier lado.</para>
		/// </summary>
		public static MDev I;
		#endregion

		#region Variables Publicas
		/// <summary>
		/// <para>Datos de la configuracion de <see cref="MDev"/>. Contiene las directrices basicas de
		/// colores, marcadores y ajustes iniciales de la consola.</para>
		/// </summary>
		public DatosAsset datos;
		#endregion

		#region Variables Privadas
		/// <summary>
		/// <para>Comandos funcionales que el sistema a recogido para cargarlos dentro de <see cref="MDev"/>.</para>
		/// </summary>
		private Comandos _comandos;
		/// <summary>
		/// <para>Control que gestiona las entradas de unity engine.</para>
		/// </summary>
		private Control _control;
		/// <summary>
		/// <para>Interfaz grafica de <see cref="MDev"/>.</para>
		/// </summary>
		private IMGUI _interfaz;
		#endregion

		#region Propiedades
		/// <summary>
		/// <para>Muestra/Oculta <see cref="MDev"/>.</para>
		/// </summary>
		public bool Mostrar { get; private set; }
		
		/// <summary>
		/// <para>Texto de entrada de la consola.</para>
		/// </summary>
		public string Texto { get; private set; }
		
		/// <summary>
		/// <para>Historial de la consola.</para>
		/// </summary>
		public string Historial { get; private set; }
		
		/// <summary>
		/// <para>Lista euristica de los posibles comandos para completar tomando en cuenta
		/// el texto de entrada de la consola.</para>
		/// </summary>
		public List<string> AutoCompletar { get; private set; }
		
		/// <summary>
		/// <para>Indice del elemento seleccionado en el campo de autocompletar.</para>
		/// </summary>
		public int AutoCompletarIndice { get; private set; }
		
		/// <summary>
		/// <para>Linea inicial de <see cref="MDev"/>. Son caracteres que se ajustan en los datos y se muestran
		/// por delante de cada linea.</para>
		/// </summary>
		public string MarcaLinea => (this.datos.nombre + this.datos.direccion + this.datos.marcador + " ");
		#endregion

		#region Inicializadores
		/// <summary>
		/// <para>Inicializador de <see cref="MDev"/>.</para>
		/// </summary>
		private void Awake()
		{
			// Obtener la instancia
			I = this;

			// Inicializar los componentes necesarios
			if (this.datos == null) this.datos = Resources.Load<DatosAsset>(RUTA_DATOS_PREDETERMINADOS);
			this.AutoCompletarIndice = 0;
			this.AutoCompletar = new List<string>();
			this._comandos = new Comandos();
			this._control = new Control(this);
			this._interfaz = new IMGUI(this);
		}
		#endregion

		#region Actualizadores
		/// <summary>
		/// <para>Actualizador de <see cref="MDev"/>.</para>
		/// </summary>
		private void Update() => this._control.Actualizar();
		#endregion
		
		#region API
		/// <summary>
		/// <para>[COMANDO] Este metodo muestra los comandos disponibles en consola.</para>
		/// </summary>
		/// <returns></returns>
		[Obsolete("Este metodo solo es para los comandos.")]
		[Cmd("ayuda", "Muestra todos los comandos disponibles.")]
		public string ComandoAyuda()
		{
			string resultado = "Lista de comandos disponibles:";
			foreach (var met in MDev.I._comandos.Metodos)
			{
				foreach (var atri in met.GetCustomAttributes(true))
				{
					if (atri is CmdAttribute)
					{
						CmdAttribute com = (CmdAttribute)atri;
						resultado += "\n      " + com.nombre + " --> " + com.descripcion;
					}
				}
			}
			return resultado;
		}

		/// <summary>
		/// <para>[COMANDO] Este metodo oculta la consola.</para>
		/// </summary>
		/// <returns></returns>
		[Obsolete("Este metodo solo es para los comandos.")]
		[Cmd("ocultar", "Oculta MDev.")]
		public void ComandoOcultar() => this.Mostrar = false;

		/// <summary>
		/// <para>[COMANDO] Este metodo limpia el historial de la consola.</para>
		/// </summary>
		/// <returns></returns>
		[Obsolete("Este metodo solo es para los comandos.")]
		[Cmd("limpiar", "Limpia el historial.")]
		public void ComandoLimpiar() => StartCoroutine(this.LimpiarHistorialAsync());

		/// <summary>
		/// <para>Prepara la ejecucion del comando.</para>
		/// </summary>
		public void Preparar()
		{
			string resultado = EjecutarComando(Texto);
			this.Historial += this.MarcaLinea + this.Texto + "\n" + (!string.IsNullOrEmpty(resultado) ? (resultado + "\n") : "");
			this.Texto = "";
		}

		/// <summary>
		/// <para>Actualiza el texto para que no ocurran fallos en la ejecucion del comando.</para>
		/// </summary>
		/// <param name="texto">Texto que se actualizara.</param>
		public void ActualizarTexto(string texto)
		{
			this.Texto += texto;
			this.Texto = this.Texto.Replace("\b", "");
		}
		#endregion

		#region Metodos Privados
		/// <summary>
		/// <para>Limpia el historial de la consola asincronamente.</para>
		/// </summary>
		/// <returns></returns>
		private IEnumerator LimpiarHistorialAsync()
		{
			yield return new WaitForEndOfFrame();
			this.Historial = "";
		}
		
		/// <summary>
		/// <para>Ejecuta un comando dado como parametro realizando las comprobaciones correspondientes.</para>
		/// </summary>
		/// <param name="c">Comando que se ejecutara.</param>
		/// <returns></returns>
		private string EjecutarComando(string c)
		{
			// Preparar la ejecucion
			this.AutoCompletar.Clear();
			bool registrado = false;
			string resultado = null;
			string validado = Regex.Match(c, @"\(([^)]*)\)").Groups[1].Value;
			List<string> args = new List<string>();
			string comando;

			// Validar el comando
			if (!string.IsNullOrEmpty(validado))
			{
				args = validado.Split(new char[] { ',' }).ToList();
				comando = c.Replace(validado, "").Replace("(", "").Replace(")", "").Replace(";", "");
			}
			else comando = c.Replace("(", "").Replace(")", "").Replace(";", "");

			// Recorrer los metodos
			foreach (var met in this._comandos.Metodos)
			{
				// Devuelve los 3 atributos.
				foreach (object atri in met.GetCustomAttributes(true))
				{
					if (atri is CmdAttribute)
					{
						CmdAttribute com = (CmdAttribute)atri;
						if (com.nombre == comando)
						{
							if (registrado) Debug.LogError("Multiples comandos se definen con: " + comando);

							Type tipo = (met.DeclaringType);
							ParameterInfo[] parametros = met.GetParameters();
							List<object> listaArgumentos = new List<object>();

							// Argumentos de lanzamiento si hay alguno
							if (args.Count != 0)
							{
								if (parametros.Length != args.Count)
								{
									resultado = $"El comando {comando} necesita {parametros.Length} parametros, solo paso {args.Count}";
									Debug.Log(resultado);
									return resultado;
								}
								else
								{
									// Lanzar argumentos para ingresar tipos de objetos
									for (int n = 0; n < parametros.Length; n++)
									{
										try
										{
											var a = Convert.ChangeType(args[n], parametros[n].ParameterType);
											listaArgumentos.Add(a);
										}
										catch
										{
											resultado = $"No se pudo convertir {args[n]} al tipo {parametros[n].ParameterType}";
											Debug.LogError(resultado);
											return resultado;
										}
									}
								}
							}
							if (tipo.IsSubclassOf(typeof(UnityEngine.Object)))
							{
								var objetos = this.EncontrarObjetosPorTipo(tipo);
    
								if (objetos != null)
								{
									foreach (var ic in objetos) resultado = (string)met.Invoke(ic, listaArgumentos.ToArray());
								}
							}
							else
							{
								var ic = Activator.CreateInstance(tipo);
								resultado = (string)met.Invoke(ic, listaArgumentos.ToArray());
							}
							registrado = true;
							break;
						}
					}
				}		
			}

			// Comprobar errores
			if (!string.IsNullOrEmpty(resultado)) return resultado;
			if (registrado) return null;
			return "Comando no encontrado! - Escribe \"ayuda\" para la lista de comandos disponibles!";
		}
		
		/// <summary>
		/// <para>Encuentra objetos por tipo.</para>
		/// </summary>
		/// <param name="tipo">Tipo de objeto.</param>
		/// <param name="modo">Modo de ordenacion.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="MissingMethodException"></exception>
		private IEnumerable<UnityEngine.Object> EncontrarObjetosPorTipo(Type tipo, FindObjectsSortMode modo = FindObjectsSortMode.None)
		{
			if (!tipo.IsSubclassOf(typeof(UnityEngine.Object)))
				throw new ArgumentException("El tipo debe ser una subclase de UnityEngine.Object");

			var m = typeof(UnityEngine.Object)
				.GetMethod("FindObjectsByType", new Type[] { typeof(FindObjectsSortMode) });

			if (m == null)
				throw new MissingMethodException("No se pudo encontrar FindObjectsByType");

			var generico = m.MakeGenericMethod(tipo);
			var resultado = generico.Invoke(null, new object[] { modo });

			return resultado as UnityEngine.Object[];
		}
		
		/// <summary>
		/// <para>Cambia el estado de visibilidad de <see cref="MDev"/>.</para>
		/// </summary>
		internal void Visibilidad() => this.Mostrar = !this.Mostrar;

		/// <summary>
		/// <para>Cambia el texto actual con el texto dado como parametro.</para>
		/// </summary>
		/// <param name="texto">Texto que se cambiara.</param>
		internal void CambiarTexto(string texto) => this.Texto = texto;
		#endregion

		#region GUI
		/// <summary>
		/// <para>Interfaz de <see cref="MDev"/>.</para>
		/// </summary>
		private void OnGUI()
		{
			if (!this.Mostrar) return;
			this._interfaz.OnGUI();
		}
		#endregion

		#region Eventos
		/// <summary>
		/// <para>Evento llamado cuando se presiona la tecla return/Borrar/Delete.</para>
		/// </summary>
		internal void EventoBorrar()
		{
			if (this.Texto.Length >= 1) this.Texto = this.Texto.Substring(0, this.Texto.Length - 1);
		}

		/// <summary>
		/// <para>Evento llamado cuando se presiona la tecla Enter.</para>
		/// </summary>
		internal void EventoAceptar()
		{
			if (this.AutoCompletar.Count > 0)
			{
				this.Texto = this.AutoCompletar[this.AutoCompletarIndice];
				this.AutoCompletar.Clear();
			}
			else this.Preparar();
		}

		/// <summary>
		/// <para>Evento llamado cuando se presiona el tabulador.</para>
		/// </summary>
		internal void EventoTabulacion()
		{
			if (this.AutoCompletar.Count != 0) { this.EventoAceptar(); return; }
			this.AutoCompletarIndice = 0;
			this.AutoCompletar.Clear();
			this.AutoCompletar.AddRange(this._comandos.ObtenerComandos(this.Texto));
		}

		/// <summary>
		/// <para>Evento llamado cuando se presiona la tecla flecha arriba.</para>
		/// </summary>
		internal void EventoFlechaArriba()
		{
			if (this.AutoCompletar.Count > 0) this.AutoCompletarIndice = (int)Mathf.Repeat(this.AutoCompletarIndice - 1, this.AutoCompletar.Count);
		}

		/// <summary>
		/// <para>Evento llamado cuando se presiona la tecla flecha abajo.</para>
		/// </summary>
		internal void EventoFlechaAbajo()
		{
			if (this.AutoCompletar.Count > 0) this.AutoCompletarIndice = (int)Mathf.Repeat(this.AutoCompletarIndice + 1, this.AutoCompletar.Count);
		}
		#endregion
	}
}
