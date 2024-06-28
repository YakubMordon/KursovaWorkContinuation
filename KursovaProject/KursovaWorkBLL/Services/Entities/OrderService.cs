using KursovaWorkBLL.Contracts;
using KursovaWorkBLL.Services.AdditionalServices;
using KursovaWorkDAL.Entity.Entities;
using KursovaWorkDAL.Entity.Entities.Car;
using KursovaWorkDAL.Repositories.Contracts;
using Microsoft.Extensions.Logging;

namespace KursovaWorkBLL.Services.Entities
{
    /// <summary>
    /// Implementation of the IOrderService interface for business logic related to orders
    /// </summary>
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<OrderService> _logger;
        private readonly IDRetriever _idRetriever;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderService"/> class.
        /// </summary>
        /// <param name="orderRepository">Repository for orders.</param>
        /// <param name="logger">Logger object for logging events.</param>
        /// <param name="idRetriever">Service to retrieve user identifier.</param>
        public OrderService(IOrderRepository orderRepository, ILogger<OrderService> logger, IDRetriever idRetriever)
        {
            _orderRepository = orderRepository;
            _logger = logger;
            _idRetriever = idRetriever;
        }

        public void AddOrder(Order order)
        {
            _orderRepository.Add(order);
            _logger.LogInformation("Order successfully added");
        }

        public int AddOrderLoggedIn(CarInfo _curCar, ConfiguratorOptions? configurator)
        {
            _logger.LogInformation("Retrieving user identifier");
            int loggedInUserId = _idRetriever.GetLoggedInUserId();

            Order order = new Order()
            {
                CarId = _curCar.Id,
                UserId = loggedInUserId,
                Price = _curCar.Price,
                OrderDate = DateTime.Now
            };

            _logger.LogInformation("Creating car order");

            if (configurator != null)
            {
                order.ConfiguratorOptions = configurator;
                _logger.LogInformation("Car selected in configurator");
            }

            _orderRepository.Add(order);
            _logger.LogInformation("Order successfully added");

            return order.Id;
        }

        public void DeleteOrder(Order order)
        {
            _orderRepository.Delete(order);
            _logger.LogInformation("Order successfully deleted");
        }

        public IEnumerable<Order> FindAll(int id)
        {
            _logger.LogInformation("Retrieving orders associated with user");
            return _orderRepository.FindAll(id);
        }

        public IEnumerable<Order> FindAllLoggedIn()
        {
            _logger.LogInformation("Retrieving identifier of logged-in user");
            int loggedInId = _idRetriever.GetLoggedInUserId();

            _logger.LogInformation("Retrieving orders associated with logged-in user");
            return _orderRepository.FindAll(loggedInId);
        }

        public IEnumerable<Order> GetAll()
        {
            _logger.LogInformation("Retrieving all orders");
            return _orderRepository.GetAll();
        }

        public Order GetOrderById(int id)
        {
            _logger.LogInformation("Retrieving order by its identifier");
            return _orderRepository.GetById(id);
        }

        public void UpdateOrder(Order order)
        {
            _orderRepository.Update(order);
            _logger.LogInformation("Order successfully updated");
        }
    }
}
