using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper.QueryableExtensions;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Tragate.Common.Library.Dto;
using Tragate.Domain.Interfaces;

namespace Tragate.Application
{
    public class ParameterService : IParameterService {
        private readonly IParameterRepository _parameterRepository;
        private readonly IDistributedCache _distributedCache;
        public ParameterService (IParameterRepository parameterRepository,
            IDistributedCache distributedCache) {
            _parameterRepository = parameterRepository;
            _distributedCache = distributedCache;
        }

        public List<ParameterDto> GetParameterByType (string type, int statusId) {
            string cacheKey = $"parameter:type:{type}:StatusId:{statusId}";
            var value = _distributedCache.GetString (cacheKey);
            if (value != null) {
                return JsonConvert.DeserializeObject<List<ParameterDto>> (value);
            }

            var result = _parameterRepository.GetParameterByType (type, statusId).ProjectTo<ParameterDto> ().ToList ();
            if (result.Any ()) {
                var cacheEntryOptions = new DistributedCacheEntryOptions ().SetAbsoluteExpiration (TimeSpan.FromHours (24));
                _distributedCache.SetString (cacheKey, JsonConvert.SerializeObject (result), cacheEntryOptions);
            }
            return result;
        }

        public void Dispose () {
            GC.SuppressFinalize (this);
        }
    }
}