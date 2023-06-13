using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Equinor.ProCoSys.BlobStorage;
using Equinor.ProCoSys.Common.Misc;
using Equinor.ProCoSys.PCS5.Command.Attachments;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.AttachmentAggregate;
using Equinor.ProCoSys.PCS5.Domain.Events.DomainEvents.AttachmentEvents;
using Equinor.ProCoSys.PCS5.Test.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Equinor.ProCoSys.PCS5.Command.Tests.Attachments;

[TestClass]
public class AttachmentServiceTests : TestsBase
{
    private readonly string _blobContainer = "bc";
    private readonly string _sourceType = "Whatever";
    private readonly Guid _sourceGuid = Guid.NewGuid();
    private Mock<IAttachmentRepository> _attachmentRepositoryMock;
    private AttachmentService _dut;
    private Attachment _attachmentAddedToRepository;
    private Attachment _existingAttachment;
    private Mock<IAzureBlobService> _blobStorageMock;
    private readonly string _existingFileName = "E.txt";
    private readonly string _newFileName = "N.txt";
    private readonly string _rowVersion = "AAAAAAAAABA=";

    [TestInitialize]
    public void Setup()
    {
        _attachmentRepositoryMock = new Mock<IAttachmentRepository>();
        _attachmentRepositoryMock
            .Setup(x => x.Add(It.IsAny<Attachment>()))
            .Callback<Attachment>(attachment =>
            {
                _attachmentAddedToRepository = attachment;
            });
        _existingAttachment = new Attachment(_sourceType, _sourceGuid, TestPlantA, _existingFileName);
        _attachmentRepositoryMock.Setup(l => l.TryGetAttachmentWithFilenameForSourceAsync(
                _existingAttachment.SourceGuid,
                _existingAttachment.FileName))
            .ReturnsAsync(_existingAttachment);

        _blobStorageMock = new Mock<IAzureBlobService>();
        var blobStorageOptionsMock = new Mock<IOptionsSnapshot<BlobStorageOptions>>();
        var options = new BlobStorageOptions
        {
            BlobContainer = _blobContainer
        };
        blobStorageOptionsMock
            .Setup(x => x.Value)
            .Returns(options);
        _dut = new AttachmentService(
            _attachmentRepositoryMock.Object,
            _plantProviderMock.Object,
            _unitOfWorkMock.Object,
            _blobStorageMock.Object,
            blobStorageOptionsMock.Object,
            new Mock<ILogger<AttachmentService>>().Object);
    }

    #region UploadNewAsync
    [TestMethod]
    public async Task UploadNewAsync_ShouldThrowException_AndNotUploadToBlobStorage_WhenFileNameExist()
    {
        // Act and Assert
        await Assert.ThrowsExceptionAsync<Exception>(()
            => _dut.UploadNewAsync(_sourceType, _sourceGuid, _existingFileName, new MemoryStream(), default));

        // Assert
        _blobStorageMock.Verify(b => b.UploadAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<Stream>(),
            It.IsAny<bool>(),
            default), Times.Never);
    }

    [TestMethod]
    public async Task UploadNewAsync_ShouldAddAttachmentToRepository_WhenFileNameNotExist()
    {
        // Act
        await _dut.UploadNewAsync(_sourceType, _sourceGuid, _newFileName, new MemoryStream(), default);

        // Assert
        Assert.IsNotNull(_attachmentAddedToRepository);
        Assert.AreEqual(_sourceGuid, _attachmentAddedToRepository.SourceGuid);
        Assert.AreEqual(_sourceType, _attachmentAddedToRepository.SourceType);
        Assert.AreEqual(_newFileName, _attachmentAddedToRepository.FileName);
    }

    [TestMethod]
    public async Task UploadNewAsync_ShouldSaveOnce_WhenFileNameNotExist()
    {
        // Act
        await _dut.UploadNewAsync(_sourceType, _sourceGuid, _newFileName, new MemoryStream(), default);

        // Assert
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(default), Times.Once);
    }

    [TestMethod]
    public async Task UploadNewAsync_ShouldAddAttachmentUploadedEvent_WhenFileNameNotExist()
    {
        // Act
        await _dut.UploadNewAsync(_sourceType, _sourceGuid, _newFileName, new MemoryStream(), default);

        // Assert
        Assert.IsInstanceOfType(_attachmentAddedToRepository.DomainEvents.First(), typeof(NewAttachmentUploadedEvent));
    }

    [TestMethod]
    public async Task UploadNewAsync_ShouldUploadToBlobStorage_WhenFileNameNotExist()
    {
        // Act
        await _dut.UploadNewAsync(_sourceType, _sourceGuid, _newFileName, new MemoryStream(), default);

        // Assert
        var p = _attachmentAddedToRepository.GetFullBlobPath();
        _blobStorageMock.Verify(b
            => b.UploadAsync(
                _blobContainer,
                p,
                It.IsAny<Stream>(),
                false,
                default), Times.Once);
    }

    [TestMethod]
    public async Task UploadOverwriteAsync_ShouldNotAddNewAttachmentToRepository_WhenFileNameExist_AndOverwrite()
    {
        // Act
        await _dut.UploadOverwriteAsync(_sourceType, _sourceGuid, _existingFileName, new MemoryStream(), _rowVersion, default);

        // Assert
        Assert.IsNull(_attachmentAddedToRepository);
    }

    [TestMethod]
    public async Task UploadOverwriteAsync_ShouldSaveOnce_WhenFileNameExist_AndOverwrite()
    {
        // Act
        await _dut.UploadOverwriteAsync(_sourceType, _sourceGuid, _existingFileName, new MemoryStream(), _rowVersion, default);

        // Assert
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(default), Times.Once);
    }

    [TestMethod]
    public async Task UploadOverwriteAsync_ShouldAddExistingAttachmentUploadedAndOverwrittenEvent_WhenFileNameExist_AndOverwrite()
    {
        // Act
        await _dut.UploadOverwriteAsync(_sourceType, _sourceGuid, _existingFileName, new MemoryStream(), _rowVersion, default);

        // Assert
        Assert.IsInstanceOfType(_existingAttachment.DomainEvents.First(), typeof(ExistingAttachmentUploadedAndOverwrittenEvent));
    }

    [TestMethod]
    public async Task UploadOverwriteAsync_ShouldUploadToBlobStorage_WhenFileNameExist_AndOverwrite()
    {
        // Act
        await _dut.UploadOverwriteAsync(_sourceType, _sourceGuid, _existingFileName, new MemoryStream(), _rowVersion, default);

        // Assert
        var p = _existingAttachment.GetFullBlobPath();
        _blobStorageMock.Verify(b
            => b.UploadAsync(
                _blobContainer,
                p,
                It.IsAny<Stream>(),
                true,
                default), Times.Once);
    }

    [TestMethod]
    public async Task UploadOverwriteAsync_ShouldSetAndReturnRowVersion()
    {
        // Act
        var result = await _dut.UploadOverwriteAsync(_sourceType, _sourceGuid, _existingFileName, new MemoryStream(), _rowVersion, default);

        // Assert
        // In real life EF Core will create a new RowVersion when save.
        // Since UnitOfWorkMock is a Mock this will not happen here, so we assert that RowVersion is set from command
        Assert.AreEqual(_rowVersion, result.RowVersion);
        Assert.AreEqual(_rowVersion, _existingAttachment.RowVersion.ConvertToString());
    }
    #endregion
}
