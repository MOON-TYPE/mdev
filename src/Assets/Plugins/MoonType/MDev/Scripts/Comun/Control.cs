//                                  ┌∩┐(◣_◢)┌∩┐                                \\
//																				\\
// Control.cs (19/04/2018)														\\
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
	/// <para>Control de las entradas del usuario. Permite gestionar las acciones del
	/// usuario y transmitir la informacion a <see cref="MDev"/>.</para>
	/// </summary>
	public class Control
	{
		#region Variables Privadas
		/// <summary>
		/// <para>Instancia de <see cref="MDev"/> cacheada por rendimiento.</para>
		/// </summary>
		private MDev _mdev;
		#endregion

		#region Constructor
		/// <summary>
		/// <para>Constructor de <see cref="Control"/>.</para>
		/// </summary>
		/// <param name="i">Instancia de <see cref="MDev"/>.</param>
		public Control(MDev i) => this._mdev = i;
		#endregion

		#region Actualizador
		/// <summary>
		/// <para>Actualizador de <see cref="Control"/>.</para>
		/// </summary>
		public void Actualizar()
		{
			// Procesar tecla de visibilidad
			if (Input.GetKeyDown(this._mdev.datos.tecla))
			{
				this._mdev.Visibilidad();
				return;
			}

			// Comprobar estado, si es visible procesar las entradas
			if (!this._mdev.Mostrar) return;
			
			// Procesar Teclas
			if (Input.GetKeyDown(KeyCode.Backspace)) { this._mdev.EventoBorrar(); return; }
			if (Input.GetKeyDown(KeyCode.Tab)) { this._mdev.EventoTabulacion(); return; }
			if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) { this._mdev.EventoAceptar(); return; }

			if (Input.GetKeyDown(KeyCode.DownArrow)) this._mdev.EventoFlechaAbajo();
			else if (Input.GetKeyDown(KeyCode.UpArrow)) this._mdev.EventoFlechaArriba();
			else this._mdev.ActualizarTexto(Input.inputString);
		}
		#endregion
	}
}
