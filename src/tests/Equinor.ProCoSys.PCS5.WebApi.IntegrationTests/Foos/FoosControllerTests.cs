using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Equinor.ProCoSys.Common.Misc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Equinor.ProCoSys.PCS5.WebApi.IntegrationTests.Foos;

[TestClass]
public class FoosControllerTests : TestBase
{
    private Guid _fooGuidUnderTest;
    private List<FooDto> _initialFoosInProject;

    [TestInitialize]
    public async Task TestInitialize()
    {
        _fooGuidUnderTest = TestFactory.Instance.SeededData[KnownPlantData.PlantA].FooAGuid;
        _initialFoosInProject = await FoosControllerTestsHelper
            .GetAllFoosInProjectAsync(UserType.Reader, TestFactory.PlantWithAccess, TestFactory.ProjectWithAccess);
    }

    [TestMethod]
    public async Task CreateFoo_AsWriter_ShouldCreateFoo()
    {
        // Arrange
        var title = Guid.NewGuid().ToString();

        // Act
        var guidAndRowVersion = await FoosControllerTestsHelper.CreateFooAsync(
            UserType.Writer,
            TestFactory.PlantWithAccess,
            title,
            TestFactory.ProjectWithAccess);

        // Assert
        AssertValidGuidAndRowVersion(guidAndRowVersion);
        var newFoo = await FoosControllerTestsHelper
            .GetFooAsync(UserType.Writer, TestFactory.PlantWithAccess, guidAndRowVersion.Guid);
        Assert.IsNotNull(newFoo);
        Assert.AreEqual(title, newFoo.Title);
        Assert.AreEqual(TestFactory.ProjectWithAccess, newFoo.ProjectName);
        AssertCreatedBy(UserType.Writer, newFoo.CreatedBy);

        var allFoos = await FoosControllerTestsHelper
            .GetAllFoosInProjectAsync(UserType.Writer, TestFactory.PlantWithAccess, TestFactory.ProjectWithAccess);
        Assert.AreEqual(_initialFoosInProject.Count+1, allFoos.Count);
    }

    [TestMethod]
    public async Task GetFoo_AsReader_ShouldGetFoo()
    {
        // Act
        var foo = await FoosControllerTestsHelper
            .GetFooAsync(UserType.Reader, TestFactory.PlantWithAccess, _fooGuidUnderTest);

        // Assert
        Assert.AreEqual(_fooGuidUnderTest, foo.Guid);
        Assert.IsNotNull(foo.RowVersion);
    }

    [TestMethod]
    public async Task GetAllFoos_AsReader_ShouldGetAllFoos()
    {
        // Act
        var foos = await FoosControllerTestsHelper
            .GetAllFoosInProjectAsync(UserType.Reader, TestFactory.PlantWithAccess, TestFactory.ProjectWithAccess);

        // Assert
        Assert.IsTrue(foos.Count > 0);
        Assert.IsTrue(foos.All(f => f.ProjectName == TestFactory.ProjectWithAccess));
        Assert.IsTrue(foos.All(f => !f.Title.IsEmpty()));
        Assert.IsTrue(foos.All(f => !f.RowVersion.IsEmpty()));
    }

    [TestMethod]
    public async Task UpdateFoo_AsWriter_ShouldUpdateFooAndRowVersion()
    {
        // Arrange
        var newTitle = Guid.NewGuid().ToString();
        var newText = Guid.NewGuid().ToString();
        var foo = await FoosControllerTestsHelper.GetFooAsync(UserType.Writer, TestFactory.PlantWithAccess, _fooGuidUnderTest);
        var initialRowVersion = foo.RowVersion;

        // Act
        var newRowVersion = await FoosControllerTestsHelper.UpdateFooAsync(
            UserType.Writer,
            TestFactory.PlantWithAccess,
            foo.Guid,
            newTitle,
            newText,
            initialRowVersion);

        // Assert
        AssertRowVersionChange(initialRowVersion, newRowVersion);
        foo = await FoosControllerTestsHelper.GetFooAsync(UserType.Writer, TestFactory.PlantWithAccess, _fooGuidUnderTest);
        Assert.AreEqual(newTitle, foo.Title);
        Assert.AreEqual(newText, foo.Text);
        Assert.AreEqual(newRowVersion, foo.RowVersion);
    }

    [TestMethod]
    public async Task VoidFoo_AsWriter_ShouldVoidFoo()
    {
        // Arrange
        var guidAndRowVersion = await FoosControllerTestsHelper.CreateFooAsync(
            UserType.Writer,
            TestFactory.PlantWithAccess,
            Guid.NewGuid().ToString(),
            TestFactory.ProjectWithAccess);
        var initialRowVersion = guidAndRowVersion.RowVersion;

        // Act
        var newRowVersion = await FoosControllerTestsHelper.VoidFooAsync(
            UserType.Writer,
            TestFactory.PlantWithAccess,
            guidAndRowVersion.Guid,
            guidAndRowVersion.RowVersion);

        // Assert
        AssertRowVersionChange(initialRowVersion, newRowVersion);
        var foo = await FoosControllerTestsHelper.GetFooAsync(UserType.Writer, TestFactory.PlantWithAccess, guidAndRowVersion.Guid);
        Assert.IsTrue(foo.IsVoided);
        Assert.AreEqual(newRowVersion, foo.RowVersion);
    }

    [TestMethod]
    public async Task DeleteFoo_AsWriter_ShouldDeleteFoo()
    {
        // Arrange
        var guidAndRowVersion = await FoosControllerTestsHelper.CreateFooAsync(
            UserType.Writer,
            TestFactory.PlantWithAccess,
            Guid.NewGuid().ToString(),
            TestFactory.ProjectWithAccess);
        var newRowVersion = await FoosControllerTestsHelper.VoidFooAsync(
            UserType.Writer,
            TestFactory.PlantWithAccess,
            guidAndRowVersion.Guid,
            guidAndRowVersion.RowVersion);
        var foo = await FoosControllerTestsHelper.GetFooAsync(UserType.Writer, TestFactory.PlantWithAccess, guidAndRowVersion.Guid);
        Assert.IsNotNull(foo);

        // Act
        await FoosControllerTestsHelper.DeleteFooAsync(
            UserType.Writer, TestFactory.PlantWithAccess,
            guidAndRowVersion.Guid,
            newRowVersion);

        // Assert
        await FoosControllerTestsHelper.GetFooAsync(UserType.Writer, TestFactory.PlantWithAccess, guidAndRowVersion.Guid, HttpStatusCode.NotFound);
    }

    [TestMethod]
    public async Task CreateFooLink_AsWriter_ShouldCreateFooLink()
    {
        // Arrange and Act
        var (_, linkGuidAndRowVersion)
            = await CreateFooLinkAsync(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

        // Assert
        AssertValidGuidAndRowVersion(linkGuidAndRowVersion);
    }

    [TestMethod]
    public async Task GetFooLinksAsync_AsReader_ShouldGetFooLinks()
    {
        // Arrange and Act
        var title = Guid.NewGuid().ToString();
        var url = Guid.NewGuid().ToString();
        var (fooGuidAndRowVersion, linkGuidAndRowVersion) = await CreateFooLinkAsync(title, url);

        // Act
        var links = await FoosControllerTestsHelper.GetFooLinksAsync(
            UserType.Reader,
            TestFactory.PlantWithAccess,
            fooGuidAndRowVersion.Guid);

        // Assert
        AssertFirstAndOnlyLink(
            fooGuidAndRowVersion.Guid,
            linkGuidAndRowVersion.Guid,
            linkGuidAndRowVersion.RowVersion,
            title,
            url,
            links);
    }

    [TestMethod]
    public async Task UpdateFooLink_AsWriter_ShouldUpdateFooLinkAndRowVersion()
    {
        // Arrange
        var newTitle = Guid.NewGuid().ToString();
        var newUrl = Guid.NewGuid().ToString();
        var (fooGuidAndRowVersion, linkGuidAndRowVersion) =
            await CreateFooLinkAsync(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

        // Act
        var newRowVersion = await FoosControllerTestsHelper.UpdateFooLinkAsync(
            UserType.Writer,
            TestFactory.PlantWithAccess,
            fooGuidAndRowVersion.Guid,
            linkGuidAndRowVersion.Guid,
            newTitle,
            newUrl,
            linkGuidAndRowVersion.RowVersion);

        // Assert
        var links = await FoosControllerTestsHelper.GetFooLinksAsync(
            UserType.Writer,
            TestFactory.PlantWithAccess,
            fooGuidAndRowVersion.Guid);

        AssertRowVersionChange(linkGuidAndRowVersion.RowVersion, newRowVersion);
        AssertFirstAndOnlyLink(
            fooGuidAndRowVersion.Guid,
            linkGuidAndRowVersion.Guid,
            newRowVersion,
            newTitle, 
            newUrl,
            links);
    }

    [TestMethod]
    public async Task DeleteFooLink_AsWriter_ShouldDeleteFooLink()
    {
        // Arrange
        var (fooGuidAndRowVersion, linkGuidAndRowVersion)
            = await CreateFooLinkAsync(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
        var links = await FoosControllerTestsHelper.GetFooLinksAsync(
            UserType.Writer,
            TestFactory.PlantWithAccess,
            fooGuidAndRowVersion.Guid);
        Assert.AreEqual(1, links.Count);

        // Act
        await FoosControllerTestsHelper.DeleteFooLinkAsync(
            UserType.Writer, TestFactory.PlantWithAccess,
            fooGuidAndRowVersion.Guid,
            linkGuidAndRowVersion.Guid,
            linkGuidAndRowVersion.RowVersion);

        // Assert
        links = await FoosControllerTestsHelper.GetFooLinksAsync(
            UserType.Writer,
            TestFactory.PlantWithAccess,
            fooGuidAndRowVersion.Guid);
        Assert.AreEqual(0, links.Count);
    }

    [TestMethod]
    public async Task CreateFooComment_AsWriter_ShouldCreateFooComment()
    {
        // Arrange and Act
        var (_, commentGuidAndRowVersion)
            = await CreateFooCommentAsync(Guid.NewGuid().ToString());

        // Assert
        AssertValidGuidAndRowVersion(commentGuidAndRowVersion);
    }

    [TestMethod]
    public async Task GetFooCommentsAsync_AsReader_ShouldGetFooComments()
    {
        // Arrange and Act
        var text = Guid.NewGuid().ToString();
        var (fooGuidAndRowVersion, commentGuidAndRowVersion) = await CreateFooCommentAsync(text);

        // Act
        var comments = await FoosControllerTestsHelper.GetFooCommentsAsync(
            UserType.Reader,
            TestFactory.PlantWithAccess,
            fooGuidAndRowVersion.Guid);

        // Assert
        AssertFirstAndOnlyComment(
            fooGuidAndRowVersion.Guid,
            commentGuidAndRowVersion.Guid,
            text,
            comments);
    }

    [TestMethod]
    public async Task UploadFooAttachment_AsWriter_ShouldUploadFooAttachment()
    {
        // Arrange and Act
        var (_, attachmentGuidAndRowVersion)
            = await UploadNewFooAttachmentAsync(Guid.NewGuid().ToString());

        // Assert
        AssertValidGuidAndRowVersion(attachmentGuidAndRowVersion);
    }

    [TestMethod]
    public async Task GetFooAttachmentsAsync_AsReader_ShouldGetFooAttachments()
    {
        // Arrange and Act
        var fileName = Guid.NewGuid().ToString();
        var (fooGuidAndRowVersion, attachmentGuidAndRowVersion) = await UploadNewFooAttachmentAsync(fileName);

        // Act
        var attachments = await FoosControllerTestsHelper.GetFooAttachmentsAsync(
            UserType.Reader,
            TestFactory.PlantWithAccess,
            fooGuidAndRowVersion.Guid);

        // Assert
        AssertFirstAndOnlyAttachment(
            fooGuidAndRowVersion.Guid,
            attachmentGuidAndRowVersion.Guid,
            attachmentGuidAndRowVersion.RowVersion,
            fileName,
            attachments);
    }

    [TestMethod]
    public async Task GetFooAttachmentDownloadUrl_AsReader_ShouldGetUrl()
    {
        // Arrange
        var fileName = Guid.NewGuid().ToString();
        var (fooGuidAndRowVersion, attachmentGuidAndRowVersion) = await UploadNewFooAttachmentAsync(fileName);

        var attachments = await FoosControllerTestsHelper.GetFooAttachmentsAsync(
            UserType.Reader,
            TestFactory.PlantWithAccess,
            fooGuidAndRowVersion.Guid);
        var uri = new Uri("http://blah.blah.com");
        var fullBlobPath = attachments.ElementAt(0).FullBlobPath;
        TestFactory.Instance.BlobStorageMock
            .Setup(a => a.GetDownloadSasUri(
                It.IsAny<string>(),
                fullBlobPath,
                It.IsAny<DateTimeOffset>(),
                It.IsAny<DateTimeOffset>()))
            .Returns(uri);


        // Act
        var attachmentUrl = await FoosControllerTestsHelper.GetFooAttachmentDownloadUrlAsync(
            UserType.Reader,
            TestFactory.PlantWithAccess,
            fooGuidAndRowVersion.Guid,
            attachmentGuidAndRowVersion.Guid);

        // Assert
        Assert.AreEqual(uri.AbsoluteUri, attachmentUrl);
    }

    [TestMethod]
    public async Task OverwriteExistingFooAttachment_AsWriter_ShouldUpdateFooAttachmentAndRowVersion()
    {
        // Arrange
        var fileName = Guid.NewGuid().ToString();
        var (fooGuidAndRowVersion, attachmentGuidAndRowVersion) =
            await UploadNewFooAttachmentAsync(fileName);

        // Act
        var newAttachmentRowVersion = await FoosControllerTestsHelper.OverwriteExistingFooAttachmentAsync(
            UserType.Writer,
            TestFactory.PlantWithAccess,
            fooGuidAndRowVersion.Guid,
            new TestFile("blah updated", fileName),
            attachmentGuidAndRowVersion.RowVersion);

        // Assert
        AssertRowVersionChange(attachmentGuidAndRowVersion.RowVersion, newAttachmentRowVersion);

        var attachments = await FoosControllerTestsHelper.GetFooAttachmentsAsync(
            UserType.Writer,
            TestFactory.PlantWithAccess,
            fooGuidAndRowVersion.Guid);

        AssertFirstAndOnlyAttachment(
            fooGuidAndRowVersion.Guid,
            attachmentGuidAndRowVersion.Guid,
            newAttachmentRowVersion,
            fileName,
            attachments);
    }

    [TestMethod]
    public async Task DeleteFooAttachment_AsWriter_ShouldDeleteFooAttachment()
    {
        // Arrange
        var (fooGuidAndRowVersion, attachmentGuidAndRowVersion)
            = await UploadNewFooAttachmentAsync(Guid.NewGuid().ToString());
        var attachments = await FoosControllerTestsHelper.GetFooAttachmentsAsync(
            UserType.Writer,
            TestFactory.PlantWithAccess,
            fooGuidAndRowVersion.Guid);
        Assert.AreEqual(1, attachments.Count);

        // Act
        await FoosControllerTestsHelper.DeleteFooAttachmentAsync(
            UserType.Writer, TestFactory.PlantWithAccess,
            fooGuidAndRowVersion.Guid,
            attachmentGuidAndRowVersion.Guid,
            attachmentGuidAndRowVersion.RowVersion);

        // Assert
        attachments = await FoosControllerTestsHelper.GetFooAttachmentsAsync(
            UserType.Writer,
            TestFactory.PlantWithAccess,
            fooGuidAndRowVersion.Guid);
        Assert.AreEqual(0, attachments.Count);
    }

    private async Task<(GuidAndRowVersion fooGuidAndRowVersion, GuidAndRowVersion linkGuidAndRowVersion)>
        CreateFooLinkAsync(string title, string url)
    {
        var fooGuidAndRowVersion = await FoosControllerTestsHelper.CreateFooAsync(
            UserType.Writer,
            TestFactory.PlantWithAccess,
            Guid.NewGuid().ToString(),
            TestFactory.ProjectWithAccess);

        var linkGuidAndRowVersion = await FoosControllerTestsHelper.CreateFooLinkAsync(
            UserType.Writer,
            TestFactory.PlantWithAccess,
            fooGuidAndRowVersion.Guid,
            title,
            url);

        return (fooGuidAndRowVersion, linkGuidAndRowVersion);
    }

    private async Task<(GuidAndRowVersion fooGuidAndRowVersion, GuidAndRowVersion commentGuidAndRowVersion)>
        CreateFooCommentAsync(string text)
    {
        var fooGuidAndRowVersion = await FoosControllerTestsHelper.CreateFooAsync(
            UserType.Writer,
            TestFactory.PlantWithAccess,
            Guid.NewGuid().ToString(),
            TestFactory.ProjectWithAccess);

        var commentGuidAndRowVersion = await FoosControllerTestsHelper.CreateFooCommentAsync(
            UserType.Writer,
            TestFactory.PlantWithAccess,
            fooGuidAndRowVersion.Guid,
            text);

        return (fooGuidAndRowVersion, commentGuidAndRowVersion);
    }

    private async Task<(GuidAndRowVersion fooGuidAndRowVersion, GuidAndRowVersion linkGuidAndRowVersion)>
        UploadNewFooAttachmentAsync(string fileName)
    {
        var fooGuidAndRowVersion = await FoosControllerTestsHelper.CreateFooAsync(
            UserType.Writer,
            TestFactory.PlantWithAccess,
            Guid.NewGuid().ToString(),
            TestFactory.ProjectWithAccess);

        var attachmentGuidAndRowVersion = await FoosControllerTestsHelper.UploadNewFooAttachmentAsync(
            UserType.Writer,
            TestFactory.PlantWithAccess,
            fooGuidAndRowVersion.Guid,
            new TestFile("blah", fileName));

        return (fooGuidAndRowVersion, attachmentGuidAndRowVersion);
    }

    private static void AssertFirstAndOnlyLink(
        Guid fooGuid,
        Guid linkGuid,
        string linkRowVersion,
        string title,
        string url,
        List<LinkDto> links)
    {
        Assert.IsNotNull(links);
        Assert.AreEqual(1, links.Count);
        var link = links[0];
        Assert.AreEqual(fooGuid, link.SourceGuid);
        Assert.AreEqual(linkGuid, link.Guid);
        Assert.AreEqual(linkRowVersion, link.RowVersion);
        Assert.AreEqual(title, link.Title);
        Assert.AreEqual(url, link.Url);
    }

    private static void AssertFirstAndOnlyComment(
        Guid fooGuid,
        Guid commentGuid,
        string text,
        List<CommentDto> comments)
    {
        Assert.IsNotNull(comments);
        Assert.AreEqual(1, comments.Count);
        var comment = comments[0];
        Assert.AreEqual(fooGuid, comment.SourceGuid);
        Assert.AreEqual(commentGuid, comment.Guid);
        Assert.AreEqual(text, comment.Text);
        Assert.IsNotNull(comment.CreatedBy);
        Assert.IsNotNull(comment.CreatedAtUtc);
    }

    private static void AssertFirstAndOnlyAttachment(
        Guid fooGuid,
        Guid attachmentGuid,
        string attachmentRowVersion,
        string fileName,
        List<AttachmentDto> attachments)
    {
        Assert.IsNotNull(attachments);
        Assert.AreEqual(1, attachments.Count);
        var attachment = attachments[0];
        Assert.AreEqual(fooGuid, attachment.SourceGuid);
        Assert.AreEqual(attachmentGuid, attachment.Guid);
        Assert.AreEqual(attachmentRowVersion, attachment.RowVersion);
        Assert.AreEqual(fileName, attachment.FileName);
    }
}
