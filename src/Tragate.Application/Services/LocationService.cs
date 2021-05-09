using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper.QueryableExtensions;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Tragate.Common.Library.Dto;
using Tragate.Domain.Interfaces;

namespace Tragate.Application {
    public class LocationService : ILocationService {
        private readonly IDistributedCache _distributedCache;
        private readonly ILocationRepository _locationRepository;
        public LocationService (ILocationRepository locationRepository,
            IDistributedCache distributedCache) {
            _distributedCache = distributedCache;
            _locationRepository = locationRepository;
        }

        public List<LocationDto> GetLocationByParentId (int? id, int statusId) {
            //string cacheKey = $"location:Id:{id}:StatusId:{statusId}";
            //var value = _distributedCache.GetString (cacheKey);
            //if (value != null) {
            //    return JsonConvert.DeserializeObject<List<LocationDto>> (value);
            //}

            var result = _locationRepository.GetLocationByParentId (id, statusId).ProjectTo<LocationDto> ().ToList ();
            //if (result.Any ()) {
            //    var cacheEntryOptions = new DistributedCacheEntryOptions ().SetAbsoluteExpiration (TimeSpan.FromHours (24));
            //    _distributedCache.SetString (cacheKey, JsonConvert.SerializeObject (result), cacheEntryOptions);
            //}
            return result;
        }

        public void Dispose () {
            GC.SuppressFinalize (this);
        }
    }
}