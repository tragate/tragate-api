using MediatR;
using Tragate.Domain.Events;
using Tragate.Domain.Interfaces;

namespace Tragate.Domain.EventHandlers {
    public class CategoryEventHandler : INotificationHandler<CategoryImageUploadedEvent> {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryEventHandler (ICategoryRepository categoryRepository) {
            _categoryRepository = categoryRepository;
        }

        public void Handle (CategoryImageUploadedEvent message) {
            var entity = _categoryRepository.GetById (message.Id);
            entity.ImagePath = message.ImagePath;
            _categoryRepository.Update (entity);
            _categoryRepository.SaveChanges ();
        }
    }
}