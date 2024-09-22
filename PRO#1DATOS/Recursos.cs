using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Drawing;

namespace PRO_1DATOS
{
    public static class Recursos
    {
        private static Dictionary<string, Image> imagenes;

        static Recursos()
        {
            imagenes = new Dictionary<string, Image>();

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



        public static Image ObtenerImagen(string nombre)
        {
            if (imagenes.TryGetValue(nombre, out var imagen))
            {
                return imagen;
            }
            else
            {
                return Properties.Resources.moto; 
            }
        }
    }
}