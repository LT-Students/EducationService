﻿using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.EducationService.Business.Commands.Certificate.Interfaces;
using LT.DigitalOffice.EducationService.Models.Db;
using LT.DigitalOffice.EducationService.Models.Dto.Requests.Certificates;
using MassTransit;
using Moq;
using Moq.AutoMock;
using System;
using System.Collections.Generic;
using LT.DigitalOffice.EducationService.Models.Dto.Requests.Images;

namespace LT.DigitalOffice.EducationService.Business.UnitTests.CertificateCommandTests
{
  class CreateCertificateCommandTests
    {
        private ICreateCertificateCommand _command;
        private AutoMocker _mocker;

        private CreateCertificateRequest _request;
        private DbUserCertificate _dbCertificate;
        private Guid _imageId;
        private AddImageRequest _image;

        private void RequestClientMock()
        {
            var _operationResultAddImageMock = new Mock<IOperationResult<Guid>>();
            _operationResultAddImageMock.Setup(x => x.Body).Returns(_imageId);
            _operationResultAddImageMock.Setup(x => x.IsSuccess).Returns(true);
            _operationResultAddImageMock.Setup(x => x.Errors).Returns(new List<string>());

            var responseBrokerAddImageMock = new Mock<Response<IOperationResult<Guid>>>();
            responseBrokerAddImageMock
               .SetupGet(x => x.Message)
               .Returns(_operationResultAddImageMock.Object);

            //_mocker
            //    .Setup<IRequestClient<IAddImageRequest>, Task>(
            //        x => x.GetResponse<IOperationResult<Guid>>(
            //            It.IsAny<object>(), default, It.IsAny<RequestTimeout>()))
            //    .Returns(Task.FromResult(responseBrokerAddImageMock.Object));
        }

        /*[SetUp]
        public void SetUp()
        {
            _mocker = new AutoMocker();
            _command = _mocker.CreateInstance<CreateCertificateCommand>();

            _image = new AddImageRequest
            {
                Content = "content",
                Extension = ".jpg",
                Name = "imagename"
            };

            _request = new()
            {
                UserId = Guid.NewGuid(),
                Name = "name",
                SchoolName = "schoolname",
                ReceivedAt = DateTime.UtcNow,
                EducationType = EducationType.Online,
                Image = _image
            };

            _imageId = Guid.NewGuid();

            _dbCertificate = new()
            {
                Id = Guid.NewGuid(),
                UserId = _request.UserId,
                Name = _request.Name,
                SchoolName = _request.SchoolName,
                ReceivedAt = _request.ReceivedAt,
                EducationType = (int)_request.EducationType,
                ImageId = _imageId
            };

            _dbUser = new DbUser
            {
                Id = Guid.NewGuid(),
                IsAdmin = true
            };

            IDictionary<object, object> _items = new Dictionary<object, object>();
            _items.Add("UserId", _dbUser.Id);

            _mocker
                .Setup<IHttpContextAccessor, IDictionary<object, object>>(x => x.HttpContext.Items)
                .Returns(_items);

            _mocker
                .Setup<IDbUserCertificateMapper, DbUserCertificate>(x => x.Map(_request, _imageId))
                .Returns(_dbCertificate);

            _mocker
                .Setup<IUserRepository, DbUser>(x => x.GetAsync(_dbUser.Id))
                .Returns(_dbUser);

            RequestClientMock();
        }

        [Test]
        public void ShouldThrowForbiddenExceptionWhenUserHasNotRight()
        {
            _mocker
                .Setup<IAccessValidator, bool>(x => x.HasRights(Rights.AddEditRemoveUsers))
                .Returns(false);

            _mocker
                .Setup<IUserRepository, DbUser>(x => x.Get(_dbUser.Id))
                .Returns(new DbUser { IsAdmin = false });

            Assert.Throws<ForbiddenException>(() => _command.Execute(_request));
            _mocker.Verify<ICertificateRepository>(x => x.Add(It.IsAny<DbUserCertificate>()), Times.Never);
            _mocker.Verify<IUserRepository>(x => x.Get(_dbUser.Id), Times.Once);
        }

        [Test]
        public void ShouldThrowExceptionWhenRepositoryThrow()
        {
            _mocker
                .Setup<IUserRepository>(x => x.Get(It.IsAny<Guid>()))
                .Throws(new Exception());

            Assert.Throws<Exception>(() => _command.Execute(_request));
            _mocker.Verify<ICertificateRepository>(x => x.Add(_dbCertificate), Times.Never);
            _mocker.Verify<IUserRepository>(x => x.Get(_dbUser.Id), Times.Once);
            _mocker.Verify<IRequestClient<IAddImageRequest>>(
                x => x.GetResponse<IOperationResult<Guid>>(
                       It.IsAny<object>(), default, It.IsAny<RequestTimeout>()), Times.Never);
        }

        [Test]
        public void ShouldAddCertificateSuccesfull()
        {
            var expectedResponse = new OperationResultResponse<Guid>
            {
                Status = OperationResultStatusType.FullSuccess,
                Body = _dbCertificate.Id
            };

            SerializerAssert.AreEqual(expectedResponse, _command.Execute(_request));
            _mocker.Verify<ICertificateRepository>(x => x.Add(_dbCertificate), Times.Once);
            _mocker.Verify<IUserRepository>(x => x.Get(_dbUser.Id), Times.Once);
            _mocker.Verify<IRequestClient<IAddImageRequest>>(
                x => x.GetResponse<IOperationResult<Guid>>(
                       It.IsAny<object>(), default, It.IsAny<RequestTimeout>()), Times.Once);
        }

        [Test]
        public void ShouldReturnFailedResponseWhenAddImageRequestThrow()
        {
            _mocker
               .Setup<IRequestClient<IAddImageRequest>, Task>(
                   x => x.GetResponse<IOperationResult<Guid>>(
                       It.IsAny<object>(), default, It.IsAny<RequestTimeout>()))
               .Throws(new Exception());

            var expectedResponse = new OperationResultResponse<Guid>
            {
                Status = OperationResultStatusType.Failed,
                Errors = new List<string> { "Can not add certificate image to certificate. Please try again later." }
            };

            SerializerAssert.AreEqual(expectedResponse, _command.Execute(_request));
            _mocker.Verify<ICertificateRepository>(x => x.Add(_dbCertificate), Times.Never);
            _mocker.Verify<IUserRepository>(x => x.Get(_dbUser.Id), Times.Once);
            _mocker.Verify<IRequestClient<IAddImageRequest>>(
                x => x.GetResponse<IOperationResult<Guid>>(
                       It.IsAny<object>(), default, It.IsAny<RequestTimeout>()), Times.Once);
        }

        [Test]
        public void ShouldReturnFailedResponseWhenAddImageRequestIsNotSuccessful()
        {
            var _operationResultAddImageMock = new Mock<IOperationResult<Guid>>();
            _operationResultAddImageMock.Setup(x => x.IsSuccess).Returns(false);
            _operationResultAddImageMock.Setup(x => x.Errors).Returns(new List<string>());

            var responseBrokerAddImageMock = new Mock<Response<IOperationResult<Guid>>>();
            responseBrokerAddImageMock
               .SetupGet(x => x.Message)
               .Returns(_operationResultAddImageMock.Object);

            _mocker
                .Setup<IRequestClient<IAddImageRequest>, Task>(
                    x => x.GetResponse<IOperationResult<Guid>>(
                        It.IsAny<object>(), default, It.IsAny<RequestTimeout>()))
                .Returns(Task.FromResult(responseBrokerAddImageMock.Object));

            var expectedResponse = new OperationResultResponse<Guid>
            {
                Status = OperationResultStatusType.Failed,
                Errors = new List<string> { "Can not add certificate image to certificate. Please try again later." }
            };

            SerializerAssert.AreEqual(expectedResponse, _command.Execute(_request));
            _mocker.Verify<ICertificateRepository>(x => x.Add(_dbCertificate), Times.Never);
            _mocker.Verify<IUserRepository>(x => x.Get(_dbUser.Id), Times.Once);
            _mocker.Verify<IRequestClient<IAddImageRequest>>(
                x => x.GetResponse<IOperationResult<Guid>>(
                       It.IsAny<object>(), default, It.IsAny<RequestTimeout>()), Times.Once);
        }*/
    }
}
