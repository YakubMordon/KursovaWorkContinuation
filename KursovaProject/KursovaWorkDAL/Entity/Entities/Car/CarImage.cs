using System.ComponentModel.DataAnnotations;

namespace KursovaWorkDAL.Entity.Entities.Car
{
    /// <summary>
    /// Represents a car image.
    /// </summary>
    public class CarImage : BaseEntity
    {
        /// <summary>
        /// The identifier for the car image.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The identifier for the car to which the image belongs.
        /// </summary>
        [Required]
        public int CarId { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        /// <summary>
        /// The URL of the car image.
        /// </summary>
        [Required]
        public string ImageUrl { get; set; }

        /// <summary>
        /// The car object to which the image belongs.
        /// </summary>
        public virtual CarInfo Car { get; set; }

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    }
}
