using System.ComponentModel.DataAnnotations;

namespace KursovaWorkDAL.Entity.Entities
{
    /// <summary>
    /// Class representing the configurator options.
    /// </summary>
    public class ConfiguratorOptions : BaseEntity
    {
        /// <summary>
        /// Configurator options identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The identifier of the order to which the configurator options belong.
        /// </summary>
        [Required]
        public int OrderId { get; set; }

        /// <summary>
        /// The color of the car.
        /// </summary>
        [StringLength(50)]
        public string? Color { get; set; }

        /// <summary>
        /// The transmission type of the car.
        /// </summary>
        [StringLength(50)]
        public string? Transmission { get; set; }

        /// <summary>
        /// The fuel type of the car.
        /// </summary>
        [StringLength(50)]
        public string? FuelType { get; set; }

        /// <summary>
        /// Relationship with the order.
        /// </summary>
        public virtual Order? Order { get; set; }
    }
}
