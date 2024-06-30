using KursovaWork.Domain.Entities.Car;
using KursovaWork.Infrastructure.Services.DB.Fakers;
using Serilog;

namespace KursovaWork.Infrastructure.Services.DB;

/// <summary>
/// Static class for initialization of database.
/// </summary>
public static class DbInitializer
{
    /// <summary>
    /// Initializes Database.
    /// </summary>
    public static void Initialize(CarSaleContext context)
    {
        context.Database.EnsureCreated();

        Log.Information("Successfully checked if the database is created.");

        if (context.Cars.Any())
        {
            Log.Information("Data already exists in the database.");
            return;
        }

        Log.Information("No data is existing in the database.");

        var carInfos = new List<CarInfo>
        {
            new CarInfo
            {
                Make = "Volkswagen",
                Model = "Arteon",
                Year = 2022,
                Price = 1260000,
                Description = "Luxurious sedan with advanced features",
                Detail = new CarDetail
                {
                    Color = "Black",
                    Transmission = "Automatic",
                    FuelType = "Gasoline"
                },
                Images = new List<CarImage>()
                {
                    new CarImage()
                    {
                        ImageUrl = "https://cdn.motor1.com/images/mgl/yK3PG/s3/2020-vw-arteon-r-line-edition.jpg"
                    },
                    new CarImage()
                    {
                        ImageUrl = "https://i.ytimg.com/vi/oAIervGLG9Q/maxresdefault.jpg"
                    },
                    new CarImage()
                    {
                        ImageUrl = "https://cdn.motor1.com/images/mgl/JPV9A/s3/2020-vw-arteon-r-line-edition.webp"
                    }
                }
            },
            new CarInfo
            {
                Make = "Porsche",
                Model = "Taycan",
                Year = 2023,
                Price = 3360000,
                Description = "Electric sports car with spectacular performance",
                Detail = new CarDetail
                {
                    Color = "White",
                    Transmission = "Automatic",
                    FuelType = "Electric"
                },
                Images = new List<CarImage>()
                {
                    new CarImage()
                    {
                        ImageUrl = "https://cdn.motor1.com/images/mgl/8ww1J/s3/2021-porsche-taycan-turbo-s.jpg"
                    },
                    new CarImage()
                    {
                        ImageUrl = "https://cdn.motor1.com/images/mgl/QEmQB/s3/2020-porsche-taycan.jpg"
                    },
                    new CarImage()
                    {
                        ImageUrl = "https://gaadiwaadi.com/wp-content/uploads/2020/04/Porche-taycan2-1280x720.jpg"
                    }
                }
            },
            new CarInfo
            {
                Make = "Audi",
                Model = "Q8",
                Year = 2021,
                Price = 2100000,
                Description = "Sleek and spacious SUV for ultimate comfort",
                Detail = new CarDetail
                {
                    Color = "White",
                    Transmission = "Automatic",
                    FuelType = "Diesel"
                },
                Images = new List<CarImage>()
                {
                    new CarImage()
                    {
                        ImageUrl = "https://uzr.com.ua/wp-content/uploads/2018/08/Audi-Q8-8.jpg"
                    },
                    new CarImage()
                    {
                        ImageUrl = "https://cdn.car-recalls.eu/wp-content/uploads/2020/07/Audi-Q8-2020-automatic-gearbox-oil-leak.jpg"
                    },
                    new CarImage()
                    {
                        ImageUrl = "https://pictures.dealer.com/a/audisanjuan1001eexpressway83aoa/0618/0112a064facce92a80125844c7371be2x.jpg"
                    }
                }
            },
            new CarInfo
            {
                Make = "Volkswagen",
                Model = "Golf",
                Year = 2022,
                Price = 784000,
                Description = "Versatile hatchback with high fuel efficiency",
                Detail = new CarDetail
                {
                    Color = "Blue",
                    Transmission = "Manual",
                    FuelType = "Gasoline"
                },
                Images = new List<CarImage>()
                {
                    new CarImage()
                    {
                        ImageUrl = "https://i.ytimg.com/vi/JtIuMVXBYlY/maxresdefault.jpg"
                    },
                    new CarImage()
                    {
                        ImageUrl = "https://a.d-cd.net/To5JJbC-UBCTAHadHDFZ7Zu4PEM-1920.jpg"
                    },
                    new CarImage()
                    {
                        ImageUrl = "https://autoua.net/media/uploads2/volkswagen/volkswagen-golf_r-2022-1280-20.jpg"
                    }
                }
            },
            new CarInfo
            {
                Make = "Lamborghini",
                Model = "Huracan",
                Year = 2023,
                Price = 8400000,
                Description = "Incredible supercar with iconic design",
                Detail = new CarDetail
                {
                    Color = "Red",
                    Transmission = "Automatic",
                    FuelType = "Gasoline"
                },
                Images = new List<CarImage>()
                {
                    new CarImage()
                    {
                        ImageUrl = "https://i.ytimg.com/vi/7IQCZxi7Mwk/maxresdefault.jpg"
                    },
                    new CarImage()
                    {
                        ImageUrl = "https://renty.ae/cdn-cgi/image/format=auto,fit=contain/https://renty.ae/uploads/car/photos/l/red_lamborghini-evo-spyder_2021_7856_abe1ff80b1f4cd65bf3347c209736e48.jpg"
                    },
                    new CarImage()
                    {
                        ImageUrl = "https://www.topgear.com/sites/default/files/cars-car/image/2019/01/tom_5734.jpg?w=1280&h=720"
                    }
                }
            },
            new CarInfo
            {
                Make = "Audi",
                Model = "A4",
                Year = 2022,
                Price = 1120000,
                Description = "Elegant and sporty sedan for daily drives",
                Detail = new CarDetail
                {
                    Color = "Gray",
                    Transmission = "Automatic",
                    FuelType = "Gasoline"
                },
                Images = new List<CarImage>()
                {
                    new CarImage()
                    {
                        ImageUrl = "https://img.automoto.ua/thumb/1280-720/d/3/2/f/1/b/audi-a4-2022.jpg?url=aHR0cHM6Ly9hdXRvbW90by51YS91cGxvYWRzL2ZpbGUvM2IvYzkvYmEvZGYvQTQuanBn"
                    },
                    new CarImage()
                    {
                        ImageUrl = "https://i.ytimg.com/vi/y09RWxyx288/maxresdefault.jpg?sqp=-oaymwEmCIAKENAF8quKqQMa8AEB-AH-CYAC0AWKAgwIABABGFsgWyhbMA8=&rs=AOn4CLALycU5RW9apLZElHc2_e7H8y0yMg"
                    },
                    new CarImage()
                    {
                        ImageUrl = "https://www.autopediame.com/storage/images/Audi/A4%202023/a4%20exterior.jpg"
                    }
                }
            },
            new CarInfo
            {
                Make = "Lamborghini",
                Model = "Aventador",
                Year = 2023,
                Price = 14000000,
                Description = "Legendary hypercar with breathtaking performance",
                Detail = new CarDetail
                {
                    Color = "Yellow",
                    Transmission = "Automatic",
                    FuelType = "Gasoline"
                },
                Images = new List<CarImage>()
                {
                    new CarImage()
                    {
                        ImageUrl = "https://i.ytimg.com/vi/2RKG2HqRy8U/maxresdefault.jpg"
                    },
                    new CarImage()
                    {
                        ImageUrl = "https://i.ytimg.com/vi/RWNpWW8MZUM/maxresdefault.jpg"
                    },
                    new CarImage()
                    {
                        ImageUrl = "https://i0.wp.com/namastecar.com/wp-content/uploads/2022/06/Lamborghini-Aventador-LP-780-4-Ultimae-Roadster-2022-video-review-specs-details-in-Hindi-1.jpeg?ssl=1"
                    }
                }
            }
        };

        var users = new UserFaker()
            .Generate(10);

        Log.Information("Adding each car.");

        context.AddRange(carInfos);

        Log.Information("Adding each user.");

        context.AddRange(users);

        context.SaveChanges();

        Log.Information("Data was successfully added.");
    }
}