﻿using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Drawing;

namespace PRO_1DATOS
{
    public static class Recursos
    {
        // Diccionario para almacenar las imágenes con sus nombres
        private static Dictionary<string, Image> imagenes;

        static Recursos()
        {
            // Inicializamos el diccionario
            imagenes = new Dictionary<string, Image>();

            // Cargar todas las imágenes al inicializar la clase
            CargarImagenes();
        }

        private static void CargarImagenes()
        {
            imagenes["moto"] = Properties.Resources.moto;
            imagenes["bot"] = Properties.Resources.bot;
            imagenes["combustible"] = Properties.Resources.combustible;
            imagenes["crecimiento_estela"] = Properties.Resources.crecimiento_estela;
            imagenes["bomba"] = Properties.Resources.bomba;
            imagenes["escudo"] = Properties.Resources.escudo;
            imagenes["hiper_velocidad"] = Properties.Resources.hiper_velocidad;
            imagenes["gasolina"] = Properties.Resources.gasolina;
        }



        // Método para obtener una imagen por su nombre
        public static Image ObtenerImagen(string nombre)
        {
            if (imagenes.TryGetValue(nombre, out var imagen))
            {
                return imagen;
            }
            else
            {
                // En lugar de arrojar una excepción, puedes retornar null o una imagen por defecto.
                return Properties.Resources.moto; // Puedes cambiar a una imagen por defecto adecuada.
            }
        }
    }
}