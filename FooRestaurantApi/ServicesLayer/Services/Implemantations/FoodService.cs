using AutoMapper;
using FooRestaurantApi.DomainLayer.Entity;
using FooRestaurantApi.DomainLayer.Extensions;
using FooRestaurantApi.DomainLayer.Model.Enums;
using FooRestaurantApi.DomainLayer.Model.Food;
using FooRestaurantApi.DomainLayer.Model.Response;
using FooRestaurantApi.RepositoryLayer.RepositoryPattern.Interfaces;

using FooRestaurantApi.ServicesLayer.DTO.Food;
using FooRestaurantApi.ServicesLayer.DTO.Response;
using FooRestaurantApi.ServicesLayer.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FooRestaurantApi.ServicesLayer.Services.Implemantations
{
    public class FoodService : IFoodService
    {
        public IMapper _mapper { get; }
        public IFoodRepository _foodRepository { get; }
        IHttpContextAccessor _httpContextAccessor { get; set; }
        long? _userId { get; set; }
        public FoodService(IMapper mapper, IFoodRepository foodRepository, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _foodRepository = foodRepository;
            _httpContextAccessor = httpContextAccessor;

            _userId = _httpContextAccessor.GetUserIdFromHttpContext();
        }

        public async Task<ApiResponseDto<string, long>> CreateFood(CreateFoodRequestDto createFoodRequestDto)
        {
            var createFoodRequestModel = _mapper.Map<CreateFoodRequestDto, CreateFoodRequestModel>(createFoodRequestDto);

            var apiresponseModel = await _foodRepository.CreateFood(createFoodRequestModel);
            return _mapper.Map<ApiResponseModel<string, long>, ApiResponseDto<string, long>>(apiresponseModel);
        }

        public async Task<ApiResponseDto<string, IEnumerable<FoodDto>>> GetFoodAdmin(long id)
        {
            var apiResponseDto = new ApiResponseDto<string, IEnumerable<FoodDto>>();
            var FoodEntityList = await _foodRepository.Filter(x => x.Id == id);
            var FoodDtoList = _mapper.Map<IEnumerable<FoodEntity>, IEnumerable<FoodDto>>(FoodEntityList);

            if (FoodDtoList.Count() == 0)
            {
                apiResponseDto.StatusCode = HttpStatusCode.NotFound;
                apiResponseDto.ErrorData = $"food with id {id} does not exits.";
                return apiResponseDto;
            }

            apiResponseDto.SuccessData = FoodDtoList;
            return apiResponseDto;
        }

        public async Task<ApiResponseDto<string, long>> UpdateFood(UpdateFoodRequestDto updateFoodRequestDto)
        {

            var updateFoodRequestModel = _mapper.Map<UpdateFoodRequestDto, UpdateFoodRequestModel>(updateFoodRequestDto);

            var apiresponseModel = await _foodRepository.UpdateFood(updateFoodRequestModel);

            return _mapper.Map<ApiResponseModel<string, long>, ApiResponseDto<string, long>>(apiresponseModel);

        }

        public async Task<ApiResponseDto<string, string>> DeleteFood(long id)
        {
            var apiresponseModel = await _foodRepository.DeleteFood(id);
            return _mapper.Map<ApiResponseModel<string, string>, ApiResponseDto<string, string>>(apiresponseModel);

        }

        public async Task<ApiResponseDto<string, IEnumerable<FoodsForTheCategoryAdminDto>>> GetFoodByWeekDayAdmin(int id)
        {
            var apiResponseModel = await _foodRepository.GetFoodByWeekDayAdmin(id);
            var apiResponseDto = _mapper.Map<ApiResponseModel<string, IEnumerable<FoodsForTheCategoryAdminModel>>, ApiResponseDto<string, IEnumerable<FoodsForTheCategoryAdminDto>>>(apiResponseModel);
            return apiResponseDto;

        }

        public async Task<ApiResponseDto<string, IEnumerable<FoodsForTheCategoryDto>>> GetFoodByWeekDay(int id)
        {
            var apiResponseModel = await _foodRepository.GetFoodByWeekDay(id);
            var apiResponseDto = _mapper.Map<ApiResponseModel<string, IEnumerable<FoodsForTheCategoryModel>>, ApiResponseDto<string, IEnumerable<FoodsForTheCategoryDto>>>(apiResponseModel);
            return apiResponseDto;

        }

        public ApiResponseDto<string, IEnumerable<DayLovAdminDto>> GetDayLovAdmin()
        {
            var apiResponseDto = new ApiResponseDto<string, IEnumerable<DayLovAdminDto>>();

            var dayLovAdminDtoList = new List<DayLovAdminDto>();

            var enumValues = Enum.GetValues(typeof(DayEnum));
            foreach (DayEnum enumValue in enumValues)
            {
                DayLovAdminDto dayLovAdminDto = new DayLovAdminDto();
                dayLovAdminDto.Id = (int)enumValue;
                dayLovAdminDto.Name = ((DayEnum)enumValue).GetDisplayNameFromAttr();
                dayLovAdminDtoList.Add(dayLovAdminDto);

            }

            apiResponseDto.SuccessData = dayLovAdminDtoList;
            return apiResponseDto;

        }

        public ApiResponseDto<string, IEnumerable<DayLovDto>> GetDayLov()
        {
            var apiResponseDto = new ApiResponseDto<string, IEnumerable<DayLovDto>>();

            var dayLovDtoList = new List<DayLovDto>();

            var enumValues = Enum.GetValues(typeof(DayEnum));
            foreach (DayEnum enumValue in enumValues)
            {
                DayLovDto dayLovDto = new DayLovDto();
                dayLovDto.Id = (int)enumValue;
                dayLovDto.Name = ((DayEnum)enumValue).GetDisplayNameFromAttr();
                dayLovDtoList.Add(dayLovDto);

            }

            apiResponseDto.SuccessData = dayLovDtoList;
            return apiResponseDto;
        }


    }
}
