using AutoMapper;
using FooRestaurantApi.DomainLayer.Model.TaskTakip;
using FooRestaurantApi.RepositoryLayer.RepositoryPattern.Interfaces;

using FooRestaurantApi.ServicesLayer.DTO.Response;
using FooRestaurantApi.ServicesLayer.DTO.User;
using FooRestaurantApi.ServicesLayer.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FooRestaurantApi.ServicesLayer.Services.Implemantations
{
    public class TaskTakipService : ITaskTakipService
    {
        public IMapper _mapper { get; }
        IHttpContextAccessor _httpContextAccessor { get; }
        string _cartSessionKey { get; set; }
        ITaskTakipRepository _taskTakipRepository { get; }

        IConfiguration _configuration { get; }
        public TaskTakipService(IMapper mapper, IHttpContextAccessor httpContextAccessor, ITaskTakipRepository taskTakipRepository, IConfiguration configuration)
        {
            _mapper = mapper;

            _httpContextAccessor = httpContextAccessor;
            _taskTakipRepository = taskTakipRepository;
            _configuration = configuration;
            _cartSessionKey = _configuration.GetSection("cartSessionKey").Value;
        }

        public TaskTakipMainModel GetTaskResult(int weekIdStart, int weekIsEnd)
        {
          return  _taskTakipRepository.GetTaskResult( weekIdStart,  weekIsEnd);


        }
    }
}
