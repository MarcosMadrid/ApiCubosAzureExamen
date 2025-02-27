﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiCubosAzureExamen.Models
{
    [Table("USUARIOSCUBO")]
    public class Usuario
    {
        [Key]
        [Column("ID_USUARIO")]
        public int Id { get; set; }

        [Column("NOMBRE")]
        public string? Nombre { get; set; }

        [Column("EMAIL")]
        public string? Email { get; set; }

        [Column("PASS")]
        public string? Password { get; set; }

        [Column("imagen")]
        public string? Imagen { get; set; }
    }
}