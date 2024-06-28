using System.ComponentModel.DataAnnotations;
using KursovaWorkDAL.Entity.Entities.Car;

namespace KursovaWorkDAL.Entity.Entities
{
    /// <summary>
    /// Represents an order.
    /// </summary>
    public class Order : BaseEntity
    {
        /// <summary>
        /// Order identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Car identifier.
        /// </summary>
        [Required]
        public int CarId { get; set; }

        /// <summary>
        /// User identifier.
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// Order price.
        /// </summary>
        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        /// <summary>
        /// Order date.
        /// </summary>
        [Required]
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// The car associated with the order.
        /// </summary>
        public virtual CarInfo Car { get; set; }

        /// <summary>
        /// The user associated with the order.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Configurator options associated with the order.
        /// </summary>
        public virtual ConfiguratorOptions? ConfiguratorOptions { get; set; }
    }
}
