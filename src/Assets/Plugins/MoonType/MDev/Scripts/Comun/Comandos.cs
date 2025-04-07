//                                  ┌∩┐(◣_◢)┌∩┐                                \\
//																				\\
// Comandos.cs (19/04/2018)														\\
// Autor:         N9+  (.\@ninpl) 												\\
// Fecha Mod:     07/04/2025													\\
// Ultima Mod:    Migracion a Unity Engine 6.0+									\\
//******************************************************************************\\

#region Librerias
using UnityEngine;
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
#endregion

namespace MoonType.MDev
{
	/// <summary>
	/// <para></para>
	/// </summary>
	public class Comandos 
	{
		#region Variables Privadas
		/// <summary>
		/// <para>Lista de comandos encontrados en la solucion.</para>
		/// </summary>
		private List<string> comandos = new List<string>();
		#endregion

		#region Propiedades
		/// <summary>
		/// <para>Metodos con comandos obtenidos.</para>
		/// </summary>
		public List<MethodInfo> Metodos { get; private set; }
		#endregion

		#region Constructor
		/// <summary>
		/// <para>Constructor de <see cref="Comandos"/>.</para>
		/// </summary>
		public Comandos()
		{
			// Inicializar los metodos y obtener todos los elementos monobehaviour de la escena
			this.Metodos = new List<MethodInfo>();
			MonoBehaviour[] escenaActiva = UnityEngine.Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);

			// Recorrer cada elemento monobehaviour de la escena
			foreach (MonoBehaviour mono in escenaActiva)
			{
				Type tipoMono = mono.GetType();

				// Recuperar los campos de la instancia mono
				MethodInfo[] metodosObtenidos = tipoMono.GetMethods(BindingFlags.Instance | BindingFlags.Public);

				// Busca todos los campos y encontrar los atributos
				for (int n = 0; n < metodosObtenidos.Length; n++)
				{
					CmdAttribute comando = Attribute.GetCustomAttribute(metodosObtenidos[n], typeof(CmdAttribute)) as CmdAttribute;

					// Si detectamos algun atributo, agregar datos.
					if (comando != null)
					{
						this.comandos.Add(comando.nombre);
						this.Metodos.Add(metodosObtenidos[n]);
					}
				}
			}
		}
		#endregion

		#region API
		/// <summary>
		/// <para>Filtra y devuelve todos los comandos que contienen el texto especificado.</para>
		/// </summary>
		/// <param name="texto">Texto a buscar dentro de los comandos disponibles.</param>
		/// <returns>Un array de comandos que contienen el texto proporcionado.</returns>
		public string[] ObtenerComandos(string texto) => this.comandos.Where(k => k.Contains(texto)).ToArray();
		#endregion
	}
}
