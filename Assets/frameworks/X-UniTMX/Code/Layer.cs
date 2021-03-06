﻿/*!
 * X-UniTMX: A tiled map editor file importer for Unity3d
 * https://bitbucket.org/Chaoseiro/x-unitmx
 * 
 * Copyright 2013 Guilherme "Chaoseiro" Maia
 * Released under the MIT license
 * Check LICENSE.MIT for more details.
 */
using System;
using System.Collections.Generic;
using System.Xml;
using System.Globalization;

namespace X_UniTMX
{
	/// <summary>
	/// An abstract base for a layer in a map.
	/// </summary>
	public abstract class Layer
	{
		/// <summary>
		/// Gets the name of the layer.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets the width (in tiles) of the layer.
		/// </summary>
		public int Width { get; private set; }

		/// <summary>
		/// Gets the height (in tiles) of the layer.
		/// </summary>
		public int Height { get; private set; }

		/// <summary>
		/// Gets or sets the depth of the layer.
		/// </summary>
		/// <remarks>
		/// This value is passed to SpriteBatch when drawing tiles in this layer and therefore
		/// is only useful if SpriteBatch.Begin is called with SpriteSortMode.FrontToBack or
		/// SpriteSortMode.BackToFront.
		/// 
		/// By default, the Map will set up the layers with LayerDepth values that work for
		/// SpriteSortMode.BackToFront to enable alpha blending to work properly.
		/// </remarks>
		public float LayerDepth { get; set; }

		/// <summary>
		/// Gets or sets the whether the layer is visible.
		/// </summary>
		public bool Visible { get; set; }

		/// <summary>
		/// Gets or sets the opacity of the layer.
		/// </summary>
		public float Opacity { get; set; }

		/// <summary>
		/// Gets the list of properties for the layer.
		/// </summary>
		public PropertyCollection Properties { get; private set; }

		internal Layer(string name, int width, int height, float layerDepth, bool visible, float opacity, PropertyCollection properties)
		{
			this.Name = name;
			this.Width = width;
			this.Height = height;
			this.LayerDepth = layerDepth;
			this.Visible = visible;
			this.Opacity = opacity;
			this.Properties = properties;
		}

		public Layer(XmlNode node)
        {
            //string Type = node.Name;
            Name = node.Attributes["name"].Value;
            Width = int.Parse(node.Attributes["width"].Value, CultureInfo.InvariantCulture);
            Height = int.Parse(node.Attributes["height"].Value, CultureInfo.InvariantCulture);

            if (node.Attributes["opacity"] != null)
            {
                Opacity = float.Parse(node.Attributes["opacity"].Value, CultureInfo.InvariantCulture);
            }

            if (node.Attributes["visible"] != null)
            {
                Visible = int.Parse(node.Attributes["visible"].Value, CultureInfo.InvariantCulture) == 1;
            }

            XmlNode propertiesNode = node["properties"];
            if (propertiesNode != null)
            {
                Properties = new PropertyCollection(propertiesNode);
            }
        }

		/// <summary>
		/// Gets a string property
		/// </summary>
		/// <param name="property">Name of the property inside Tiled</param>
		/// <returns>The value of the property, String.Empty if property not found</returns>
		public string GetPropertyAsString(string property)
		{
			string str = string.Empty;
			Property p = null;
			if (Properties == null)
				return str;
			if (Properties.TryGetValue(property.ToLowerInvariant(), out p))
				str = p.RawValue;

			return str;
		}
		/// <summary>
		/// Gets a boolean property
		/// </summary>
		/// <param name="property">Name of the property inside Tiled</param>
		/// <returns>The value of the property</returns>
		public bool GetPropertyAsBoolean(string property)
		{
			bool b = false;
			string str = string.Empty;
			Property p = null;
			if (Properties == null)
				return b;
			if (Properties.TryGetValue(property.ToLowerInvariant(), out p))
				str = p.RawValue;

			Boolean.TryParse(str, out b);

			return b;
		}
		/// <summary>
		/// Gets an integer property
		/// </summary>
		/// <param name="property">Name of the property inside Tiled</param>
		/// <returns>The value of the property</returns>
		public int GetPropertyAsInt(string property)
		{
			int b = 0;
			string str = string.Empty;
			Property p = null;
			if (Properties == null)
				return b;
			if (Properties.TryGetValue(property.ToLowerInvariant(), out p))
				str = p.RawValue;

			Int32.TryParse(str, out b);

			return b;
		}
		/// <summary>
		/// Gets a float property
		/// </summary>
		/// <param name="property">Name of the property inside Tiled</param>
		/// <returns>The value of the property</returns>
		public float GetPropertyAsFloat(string property)
		{
			float b = 0;
			string str = string.Empty;
			Property p = null;
			if (Properties == null)
				return b;
			if (Properties.TryGetValue(property.ToLowerInvariant(), out p))
				str = p.RawValue;

			float.TryParse(str, out b);

			return b;
		}
	}
}
