//                                  ┌∩┐(◣_◢)┌∩┐                                \\
//																				\\
// IMGUI.cs (19/04/2018)														\\
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
	/// <para>Interfaz de MDev.</para>
	/// </summary>
	public class IMGUI 
	{
		#region Variables Privadas
		/// <summary>
		/// <para>Instancia de <see cref="MDev"/> cacheada por rendimiento.</para>
		/// </summary>
		private MDev _mdev;
		/// <summary>
		/// <para>Datos de <see cref="MDev"/>.</para>
		/// </summary>
		private DatosAsset _asset;
		/// <summary>
		/// <para>Textura del fondo. (Solo es usada en caso de que el fondo no sea transparente)</para>
		/// </summary>
		private Texture2D _fondo;
		#endregion

		#region Propiedades
		/// <summary>
		/// <para>Estilo visual de la consola.</para>
		/// </summary>
		public GUIStyle Estilo { get; private set; }
		#endregion

		#region Constructor
		/// <summary>
		/// <para>Constructor de <see cref="IMGUI"/>.</para>
		/// </summary>
		/// <param name="i">Instancia de <see cref="MDev"/>.</param>
		public IMGUI(MDev i)
		{
			// Asignar la instancia de MDev y la configuracion visual
			this._mdev = i;
			this._asset = i.datos;

			// Crear el estilo visual de la consola
			this.Estilo = new GUIStyle();
			if (this._asset.fuente != null) this.Estilo.font = this._asset.fuente;
			this.Estilo.fontSize = 16;
			this.Estilo.richText = true;
			this.Estilo.normal.textColor = this._asset.colorTexto;
			this.Estilo.hover.textColor = this._asset.colorAutoCompletar;
			this.Estilo.active.textColor = this._asset.colorAutoCompletar;
			this.Estilo.onHover.textColor = this._asset.colorAutoCompletar;
			this.Estilo.onActive.textColor = this._asset.colorAutoCompletar;

			// Generar la textura de contencion
			this._fondo = new Texture2D(2, 2);
			this._fondo.SetPixels(0, 0, this._fondo.width, this._fondo.height, new Color[] { this._asset.colorBG, this._asset.colorBG, this._asset.colorBG, this._asset.colorBG });
			this._fondo.wrapMode = TextureWrapMode.Repeat;
			this._fondo.Apply();

			// Aplicar el fondo.
			this.Estilo.normal.background = this._fondo;
		}
		#endregion

		#region GUI
		/// <summary>
		/// <para>Interfaz de <see cref="MDev"/>.</para>
		/// </summary>
		internal void OnGUI()
		{
			// Mostrar el marcador inicial
			GUILayout.Label(this._mdev.Historial + this._mdev.MarcaLinea + this._mdev.Texto, this.Estilo);

			// Nodo Autocompletar y ejecutar
			if (this._mdev.AutoCompletar.Count > 0)
			{
				for (int n = 0; n < this._mdev.AutoCompletar.Count; n++)
				{
					GUI.SetNextControlName(this._mdev.AutoCompletar[n]);
					GUIStyle t = this.Estilo;
					if (n == this._mdev.AutoCompletarIndice) GUI.color = this._asset.colorAutoCompletar;
					else GUI.color = this._asset.colorTexto;
					if (GUILayout.Button(this._mdev.AutoCompletar[n], t))
					{
						this._mdev.CambiarTexto(this._mdev.AutoCompletar[n]);
						this._mdev.Preparar();
					}
				}
			}
			else GUI.color = this._asset.colorTexto;
		}
		#endregion
	}
}
