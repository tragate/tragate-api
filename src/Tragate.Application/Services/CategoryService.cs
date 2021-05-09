using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Tragate.Application.ViewModels;
using Tragate.Common.Library.Dto;
using Tragate.Common.Library.Enum;
using Tragate.Domain.Commands;
using Tragate.Domain.Core.Bus;
using Tragate.Domain.Core.Notifications;
using Tragate.Domain.Interfaces;

namespace Tragate.Application
{
    public class CategoryService : ICategoryService
    {
        private readonly IMapper _mapper;
        private readonly IMediatorHandler _bus;
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(IMapper mapper,
            IMediatorHandler bus,
            ICategoryRepository categoryRepository){
            _bus = bus;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }

        public IQueryable<CategoryDto> GetCategories(StatusType status, int? parentId, string slug){
            return _categoryRepository.GetCategories(status, parentId, slug).ProjectTo<CategoryDto>();
        }

        public List<CategoryTreeDto> GetCategoryPathById(int id){
            return _categoryRepository.GetCategoryPathById(id);
        }

        public List<CategoryTreeDto> GetCategoryPathBySlug(string slug){
            return _categoryRepository.GetCategoryPathBySlug(slug);
        }

        public List<GroupCategoryDto> GetCategoryGroup(){
            return _categoryRepository.GetCategoryGroup();
        }

        public List<RootCategoryDto> GetSubCategoryGroup(int[] ids, int pageSize){
            return _categoryRepository.GetSubCategoryGroup(ids, pageSize);
        }

        public CategoryDto GetCategoryById(int id){
            return _mapper.Map<CategoryDto>(_categoryRepository.GetById(id));
        }

        public CategoryDto GetCategoryBySlug(string slug){
            var result = _mapper.Map<CategoryDto>(_categoryRepository.GetCategoryBySlug(slug));
            if (result == null){
                _bus.RaiseEvent(new DomainNotification("GetCategoryBySlug", "Category not found !"));
            }

            return result;
        }

        public void AddCategory(CategoryViewModel model){
            var insertCommand = _mapper.Map<AddNewCategoryCommand>(model);
            _bus.SendCommand(insertCommand);
        }

        public void UpdateCategory(CategoryViewModel model){
            var updateCommand = _mapper.Map<UpdateCategoryCommand>(model);
            _bus.SendCommand(updateCommand);
        }

        public void UploadImage(IFormFile files, int id){
            var uploadedFileCommand = new UploadCategoryImageCommand()
            {
                Id = id,
                UploadedFile = files
            };
            _bus.SendCommand(uploadedFileCommand);
        }

        public void Dispose(){
            GC.SuppressFinalize(this);
        }
    }
}