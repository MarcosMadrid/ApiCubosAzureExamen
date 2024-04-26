using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiCubosAzureExamen.Models
{
    [Table("COMPRACUBOS")]
    public class Pedido
    {
        [Key]
        [Column("id_pedido")]
        public int Id { get; set; }

        [Column("id_cubo")]
        public int IdCubo { get; set; }

        [Column("id_usuario")]
        public int IdUsuario { get; set; }

        [Column("fechapedido")]
        public DateTime FecahPedido { get; set; }
    }
}