using FooRestaurantApi.DomainLayer.Entity;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace FooRestaurantApi.DomainLayer.Extensions
{
    public static class CustomExtensions
    {
        public static string GetHashedStringAsHex(this string inputString)
        {
            var inputStringAsutf8Bytes = Encoding.UTF8.GetBytes(inputString);
            byte[] inputStringHashedAsByteArr = SHA512Managed.HashData(inputStringAsutf8Bytes);
            var inputStringHashedAsHexaString = string.Concat(Array.ConvertAll(inputStringHashedAsByteArr, bytes => bytes.ToString("X2")));
            return inputStringHashedAsHexaString;

        }
        public static string GetDisplayNameFromAttr(this Enum enumValue)
        {

            Object attributeInstanceAsObj = enumValue.GetType().GetMember(enumValue.ToString()).First(m => m.DeclaringType == enumValue.GetType()).GetCustomAttributes(typeof(DisplayAttribute), false).First();
            DisplayAttribute attributeInstance = (DisplayAttribute)attributeInstanceAsObj;
            return attributeInstance.Name;
        }

        public static long? GetUserIdFromHttpContext(this IHttpContextAccessor httpContexAccessor)
        {


            long userId = 0;

            if (Int64.TryParse(httpContexAccessor.HttpContext.User.FindFirst(c => c.Type == nameof(UserEntity.Id))?.Value, out userId))
            {
                return userId;

            }
            else
            {

                return null;
            }
        }
    }
}
