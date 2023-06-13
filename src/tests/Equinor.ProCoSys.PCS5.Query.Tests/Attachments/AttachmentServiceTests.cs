using System;
using System.Linq;
using System.Threading.Tasks;
using Equinor.ProCoSys.Common.Misc;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.AttachmentAggregate;
using Equinor.ProCoSys.PCS5.Test.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Equinor.ProCoSys.PCS5.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Equinor.ProCoSys.PCS5.Query.Attachments;

namespace Equinor.ProCoSys.PCS5.Query.Tests.Attachments;

[TestClass]
public class AttachmentServiceTests : ReadOnlyTestsBase
{
    private Attachment _createdAttachment;
    private Guid _createdAttachmentGuid;
    private Attachment _modifiedAttachment;
    private Guid _modifiedAttachmentGuid;
    private Guid _sourceGuid;

    protected override void SetupNewDatabase(DbContextOptions<PCS5Context> dbContextOptions)
    {
        using var context = new PCS5Context(dbContextOptions, _plantProvider, _eventDispatcher, _currentUserProvider);

        _sourceGuid = Guid.NewGuid();
        _createdAttachment = new Attachment("X", _sourceGuid, TestPlantA, "t1.txt");
        _modifiedAttachment = new Attachment("X", _sourceGuid, TestPlantA, "t2.txt");

        context.Attachments.Add(_createdAttachment);
        context.Attachments.Add(_modifiedAttachment);
        context.SaveChangesAsync().Wait();
        _createdAttachmentGuid = _createdAttachment.Guid;

        _modifiedAttachment.IncreaseRevisionNumber();
        context.SaveChangesAsync().Wait();
        _modifiedAttachmentGuid = _modifiedAttachment.Guid;
    }

    [TestMethod]
    public async Task GetAllForSourceAsync_ShouldReturnCorrect_CreatedDtos()
    {
        // Arrange
        await using var context = new PCS5Context(_dbContextOptions, _plantProvider, _eventDispatcher, _currentUserProvider);
        var dut = new AttachmentService(context);

        // Act
        var result = await dut.GetAllForSourceAsync(_sourceGuid, default);

        // Assert
        var attachmentDtos = result.ToList();
        Assert.AreEqual(2, attachmentDtos.Count);
        var createdAttachmentDto = attachmentDtos.SingleOrDefault(a => a.Guid == _createdAttachmentGuid);
        Assert.IsNotNull(createdAttachmentDto);
        AssertAttachmentDto(_createdAttachment, createdAttachmentDto);
        Assert.IsNull(createdAttachmentDto.ModifiedBy);
        Assert.IsNull(createdAttachmentDto.ModifiedAtUtc);

        var modifiedAttachmentDto = attachmentDtos.SingleOrDefault(a => a.Guid == _modifiedAttachmentGuid);
        Assert.IsNotNull(modifiedAttachmentDto);
        AssertAttachmentDto(_modifiedAttachment, modifiedAttachmentDto);

        var modifiedBy = modifiedAttachmentDto.ModifiedBy;
        Assert.IsNotNull(modifiedBy);
        Assert.AreEqual(CurrentUserOid, modifiedBy.Guid);
        Assert.IsTrue(modifiedAttachmentDto.ModifiedAtUtc.HasValue);
        Assert.AreEqual(_modifiedAttachment.ModifiedAtUtc, modifiedAttachmentDto.ModifiedAtUtc);
    }

    private void AssertAttachmentDto(Attachment attachment, AttachmentDto attachmentDto)
    {
        Assert.AreEqual(attachment.SourceGuid, attachmentDto.SourceGuid);
        Assert.AreEqual(attachment.Guid, attachmentDto.Guid);
        Assert.AreEqual(attachment.FileName, attachmentDto.FileName);
        var createdBy = attachmentDto.CreatedBy;
        Assert.IsNotNull(createdBy);
        Assert.AreEqual(CurrentUserOid, createdBy.Guid);
        Assert.AreEqual(attachment.CreatedAtUtc, attachmentDto.CreatedAtUtc);
    }
}
