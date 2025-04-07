//                                  ┌∩┐(◣_◢)┌∩┐                                \\
//																				\\
// DatosAsset.cs (19/04/2018)													\\
// Autor:         N9+  (.\@ninpl) 												\\
// Fecha Mod:     07/04/2025													\\
// Ultima Mod:    Migracion a Unity Engine 6.0+									\\
//******************************************************************************\\

#region Librerias
using UnityEngine;
#endregion

namespace MoonType.MDev
{
	/// <summary>
	/// <para>Datos de configuracion para la consola <see cref="MDev"/>.</para>
	/// </summary>
	[CreateAssetMenu(fileName ="CmdAsset",menuName ="MDev/Asset")]
	public class DatosAsset : ScriptableObject
	{
		#region Variables Publicas
		/// <summary>
		/// <para>Marcador inicial de la consola. ('>' o '~')</para>
		/// </summary>
		public string marcador;
		/// <summary>
		/// <para>Direccion inicial o actual de la consola. (/assets/objetos)</para>
		/// </summary>
		public string direccion;
		/// <summary>
		/// <para>Nombre de usuario. ('Terminal' o ' →')</para>
		/// </summary>
		public string nombre;
		/// <summary>
		/// <para>Fuente de la consola..</para>
		/// </summary>
		public Font fuente;
		/// <summary>
		/// <para>Color del texto de la consola.</para>
		/// </summary>
		public Color colorTexto;
		/// <summary>
		/// <para>Color de fondo de la consola.</para>
		/// </summary>
		public Color colorBG;
		/// <summary>
		/// <para>Color del texto autocompletado de la consola.</para>
		/// </summary>
		public Color colorAutoCompletar;
		/// <summary>
		/// <para>Tecla para abrir/cerrar <see cref="MDev"/>.</para>
		/// </summary>
		public KeyCode tecla;
		#endregion
	}
}
