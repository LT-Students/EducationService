﻿using FluentValidation.TestHelper;
using LT.DigitalOffice.EducationService.Data.Interfaces;
using LT.DigitalOffice.EducationService.Models.Db;
using LT.DigitalOffice.EducationService.Models.Dto.Requests.Certificates;
using LT.DigitalOffice.EducationService.Validation.Certificates;
using Moq;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.EducationService.Validation.UnitTests.Certificate
{
    public class CreateCertificateRequestValidatorTests
    {
        private CreateCertificateRequestValidator _validator;

        private Guid _existUser = Guid.NewGuid();

        /*[SetUp]
        public void SetUp()
        {
            _repository = new Mock<IUserRepository>();
            _repository
                .Setup(x => x.GetAsync(_existUser))
                .Returns(new DbUser());

            _validator = new(_repository.Object);
        }

        [Test]
        public void ShouldNotThrowAnyException()
        {
            var correctRequest = new CreateCertificateRequest
            {
                Name = "Name",
                SchoolName = "SchoolNmae",
                EducationType = Models.Dto.Enums.EducationType.Online,
                ReceivedAt = DateTime.UtcNow,
                Image = new Models.Dto.Requests.User.AddImageRequest(),
                UserId = _existUser
            };

            _validator.TestValidate(correctRequest).ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void ShouldThrowExceptionWhenNameIsNotCorrect()
        {
            var incorrectRequest = new CreateCertificateRequest
            {
                Name = "",
                SchoolName = "",
                EducationType = (Models.Dto.Enums.EducationType) 5,
                ReceivedAt = DateTime.UtcNow,
                Image = null,
                UserId = Guid.NewGuid()
            };

            _validator.TestValidate(incorrectRequest).ShouldHaveValidationErrorFor(x => x.Name);
            _validator.TestValidate(incorrectRequest).ShouldHaveValidationErrorFor(x => x.SchoolName);
            _validator.TestValidate(incorrectRequest).ShouldHaveValidationErrorFor(x => x.EducationType);
            _validator.TestValidate(incorrectRequest).ShouldHaveValidationErrorFor(x => x.UserId);
            _validator.TestValidate(incorrectRequest).ShouldHaveValidationErrorFor(x => x.Image);
        }*/

    }
}