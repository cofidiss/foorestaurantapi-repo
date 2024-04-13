using AutoMapper;
using FooRestaurantApi.DomainLayer.Entity;
using FooRestaurantApi.DomainLayer.Extensions;
using FooRestaurantApi.DomainLayer.Model.FoodCategory;
using FooRestaurantApi.DomainLayer.Model.Response;
using FooRestaurantApi.RepositoryLayer.RepositoryPattern.Interfaces;
using FooRestaurantApi.ServicesLayer.DTO.FoodCategory;
using FooRestaurantApi.ServicesLayer.DTO.Response;
using FooRestaurantApi.ServicesLayer.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FooRestaurantApi.ServicesLayer.Services.Implemantations
{
    public class FoodCategoryService : IFoodCategoryService
    {
        public IMapper _mapper { get; }

        public IFoodCategoryRepository _FoodCategoryRepository { get; }
        IHttpContextAccessor _httpContextAccessor { get; set; }
        long? _userId { get; set; }
        public FoodCategoryService(IMapper mapper, IFoodCategoryRepository FoodCategoryRepository, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;

            _FoodCategoryRepository = FoodCategoryRepository;
            _httpContextAccessor = httpContextAccessor;
            _userId = _httpContextAccessor.GetUserIdFromHttpContext();

        }

        public async Task<ApiResponseDto<string, long>> CreateFoodCategory(CreateFoodCategoryRequestDto createFoodCategoryRequestDto)
        {
            var createFoodCategoryRequestModel = _mapper.Map<CreateFoodCategoryRequestDto, CreateFoodCategoryRequestModel>(createFoodCategoryRequestDto);

            var apiResponseModel = await _FoodCategoryRepository.CreateFoodCategory(createFoodCategoryRequestModel);
            var apiResponseDto = _mapper.Map<ApiResponseModel<string, long>, ApiResponseDto<string, long>>(apiResponseModel);
            return apiResponseDto;

        }

        public async Task<ApiResponseDto<string, FoodCategoryDto>> GetFoodCategory(long id)
        {
            var apiResponseDto = new ApiResponseDto<string, FoodCategoryDto>();
            var FoodCategoryEntityList = await _FoodCategoryRepository.Filter(x => x.Id == id);
            var FoodCategoryDtoList = _mapper.Map<IEnumerable<FoodCategoryEntity>, IEnumerable<FoodCategoryDto>>(FoodCategoryEntityList);
            if (FoodCategoryDtoList.Count() == 0)
            {
                apiResponseDto.StatusCode = HttpStatusCode.NotFound;
                apiResponseDto.ErrorData = "Food category id {id} does not exits.";
                return apiResponseDto;
            }

            apiResponseDto.SuccessData = FoodCategoryDtoList.First();


            return apiResponseDto;
        }

        public async Task<ApiResponseDto<string, long>> UpdateFoodCategory(UpdateFoodCategoryRequestDto updateFoodCategoryRequestDto)
        {

            var updateFoodCategoryRequestModel = _mapper.Map<UpdateFoodCategoryRequestDto, UpdateFoodCategoryRequestModel>(updateFoodCategoryRequestDto);
            var apiResponseModel = await _FoodCategoryRepository.UpdateFoodCategory(updateFoodCategoryRequestModel);

            var apiResponseDto = _mapper.Map<ApiResponseModel<string, long>, ApiResponseDto<string, long>>(apiResponseModel);
            return apiResponseDto;
        }

        public async Task<ApiResponseDto<string, string>> DeleteFoodCategory(long id)
        {

            var apiResponseModel = await _FoodCategoryRepository.DeleteFoodCategory(id);


            var apiResponseDto = _mapper.Map<ApiResponseModel<string, string>, ApiResponseDto<string, string>>(apiResponseModel);
            return apiResponseDto;
        }

        public async Task<ApiResponseDto<string, IEnumerable<FoodCategoryLovAdminDto>>> GetFoodCategoryLovAdmin()
        {

            var apiresponseModel = await _FoodCategoryRepository.GetFoodCategoryLovAdmin();
            return _mapper.Map<ApiResponseModel<string, IEnumerable<FoodCategoryLovAdminModel>>, ApiResponseDto<string, IEnumerable<FoodCategoryLovAdminDto>>>(apiresponseModel);
        }






    }
}

