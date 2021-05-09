using System;
using System.Collections.Generic;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Tragate.Common.Library;
using Tragate.Domain.Core.Infrastructure;
using Tragate.Domain.Events;
using Tragate.Domain.Interfaces;

namespace Tragate.Domain.EventHandlers
{
    public class UserEventHandler :
        INotificationHandler<UserRegisteredEvent>,
        INotificationHandler<UserForgotPasswordEvent>,
        INotificationHandler<UserImageUploadedEvent>,
        INotificationHandler<AnonymUserCreatedEvent>
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IEmailService _emailService;
        private readonly ConfigSettings _settings;
        private readonly IUserRepository _userRepository;

        public UserEventHandler(IDistributedCache distributedCache,
            IEmailService emailService,
            IOptions<ConfigSettings> settings,
            IUserRepository userRepository){
            _settings = settings.Value;
            _emailService = emailService;
            _userRepository = userRepository;
            _distributedCache = distributedCache;
        }

        public void Handle(UserRegisteredEvent message){
            var token = Guid.NewGuid().ToString();
            var callbackUrl = $"{_settings.WebSite}/account/verify/{token}";
            _distributedCache.SetString(token, message.UserId.ToString(), new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24)
            });

            string body = _emailService.GetEmailTemplate(_settings.S3.EmailPath, "activation.html");
            body = body.Replace("@username", message.FullName)
                .Replace("@email", message.Email)
                .Replace("@callbackUrl", callbackUrl)
                .Replace("@website", _settings.WebSite);
            _emailService.SendActivationEmail(new List<string> {message.Email},
                $"Verify Your Account - {DateTime.Now:dd/MM/yyyy HH:mm:ss}", body);
        }

        public void Handle(UserForgotPasswordEvent message){
            string token = Guid.NewGuid().ToString();
            var user = _userRepository.GetUserById(message.UserId);
            string callbackUrl = $"{_settings.WebSite}/account/reset-password/{token}";
            _distributedCache.SetString(token, message.UserId.ToString(), new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24)
            });

            string body = _emailService.GetEmailTemplate(_settings.S3.EmailPath, "reset-password.html");
            body = body.Replace("@username", user.FullName)
                .Replace("@email", user.Email)
                .Replace("@callbackUrl", callbackUrl)
                .Replace("@website", _settings.WebSite);
            _emailService.SendPasswordResetEmail(new List<string> {message.Email},
                $"Reset Your Password - {DateTime.Now:dd/MM/yyyy HH:mm:ss}", body);
        }

        public void Handle(AnonymUserCreatedEvent message){
            var user = _userRepository.GetById(message.UserId);
            var token = Guid.NewGuid().ToString();
            var callbackUrl = $"{_settings.WebSite}/account/completeSignup/{token}";
            _distributedCache.SetString(token, user.Id.ToString(), new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24)
            });
            string body = _emailService.GetEmailTemplate(_settings.S3.EmailPath, "complete-signup.html");
            body = body.Replace("@ReceiverUserName", user.FullName)
                .Replace("@CallbackUrl", callbackUrl)
                .Replace("@WebSite", _settings.WebSite)
                .Replace("@email", user.Email);
            _emailService.SendCompleteSignupEmail(new List<string> {user.Email}, message.Subject, body);
        }

        public void Handle(UserImageUploadedEvent message){
            var entity = _userRepository.GetById(message.UserId);
            entity.ProfileImagePath = message.ProfileImagePath;
            _userRepository.Update(entity);
            _userRepository.SaveChanges();
        }
    }
}