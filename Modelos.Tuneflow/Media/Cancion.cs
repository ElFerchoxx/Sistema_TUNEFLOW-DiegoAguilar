﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modelos.Tuneflow.Playlist;
using Modelos.Tuneflow.Usuario.Produccion;

namespace Modelos.Tuneflow.Media
{
    public class Cancion
    {
        [Key] public int Id { get; set; }
        public string Titulo { get; set; }
        public int Duracion { get; set; } // Duración en segundos
        public string Genero { get; set; } // Pop, Rock, Hip-Hop, etc.
        [ForeignKey(nameof(Artista))] public int ArtistaId { get; set; }
        [ForeignKey(nameof(Album))] public int? AlbumId { get; set; } // Puede ser nulo si la canción no pertenece a un álbum específico
        public string RutaArchivo { get; set; } // Ruta del archivo de la canción en el servidor o almacenamiento en la nube
        public bool ContenidoExplicito { get; set; } // Indica si la canción tiene contenido explícito (palabras groseras, temas sensibles, etc.)
        public string RutaImagen { get; set; }
        public Artista? Artista { get; set; } // Relación con el artista que creó la canción
        public Album? Album { get; set; } // Relación con el álbum al que pertenece la canción (opcional)


        public string TiempoEnMinutos(int tiempo)
        {
            int minutos = tiempo / 60;
            int segundos = tiempo % 60; // mejor usar operador módulo

            return $"{minutos}:{segundos:D2}"; // D2 para que segundos tengan siempre dos dígitos
        }

    }
}
