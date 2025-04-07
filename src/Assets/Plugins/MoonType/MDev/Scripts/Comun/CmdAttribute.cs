//                                  ┌∩┐(◣_◢)┌∩┐                                \\
//																				\\
// CmdAttribute.cs (19/04/2018)													\\
// Autor:         N9+  (.\@ninpl) 												\\
// Fecha Mod:     07/04/2025													\\
// Ultima Mod:    Migracion a Unity Engine 6.0+									\\
//******************************************************************************\\

#region Librerias
using System;
#endregion

namespace MoonType.MDev
{
	/// <summary>
	/// <para>Clase para representar el atributo (Cmd) que se utilizara como bandera de señalizacion
	/// para los comandos usados por <see cref="MDev"/>.</para>
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	public class CmdAttribute : Attribute 
	{
		#region Variables Publicas
		/// <summary>
		/// <para>Nombre del comando.</para>
		/// </summary>
		public string nombre;
		/// <summary>
		/// <para>Descripcion del comando.</para>
		/// </summary>
		public string descripcion;
		#endregion

		#region Constructores
		/// <summary>
		/// <para>Crea un <see cref="CmdAttribute"/>.</para>
		/// </summary>
		/// <param name="nom">Nombre del comando.</param>
		public CmdAttribute(string nom) => this.nombre = nom;

		/// <summary>
		/// <para>Crea un <see cref="CmdAttribute"/>.</para>
		/// </summary>
		/// <param name="nom">Nombre del comando.</param>
		/// <param name="desc">Descripcion del comando.</param>
		public CmdAttribute(string nom, string desc)
		{
			this.nombre = nom;
			this.descripcion = desc;
		}
		#endregion
	}
}
