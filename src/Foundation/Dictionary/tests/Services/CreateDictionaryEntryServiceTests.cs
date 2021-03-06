﻿namespace xHelix.Foundation.Dictionary.Tests.Services
{
  using System.Collections.Generic;
  using System.Linq;
  using FluentAssertions;
  using Ploeh.AutoFixture;
  using Sitecore.Data.Items;
  using Sitecore.FakeDb;
  using Sitecore.FakeDb.AutoFixture;
  using xHelix.Foundation.Dictionary.Models;
  using xHelix.Foundation.Dictionary.Services;
  using xHelix.Foundation.Baseline.Attributes;
  using Xunit;

  public class CreateDictionaryEntryServiceTests
  {
    [Theory]
    [AutoDbData]
    public void CreateDictionaryEntry_Call_CreateItems(Db db,[Content]DictionaryEntryTemplate entryTemplate, [Content]DbItem rootItem, IEnumerable<string> pathParts, string defaultValue)
    {
      //Arrange
      var dictionary = new Dictionary()
      {
        Root = db.GetItem(rootItem.ID)
      };

      var path = string.Join("/", pathParts.Select(ItemUtil.ProposeValidItemName));

      //Act
      var phraseItem = CreateDictionaryEntryService.CreateDictionaryEntry(dictionary, path, defaultValue);

      //Assert
      phraseItem.Should().NotBeNull();
      phraseItem.TemplateID.Should().Be(Templates.DictionaryEntry.ID);
      phraseItem.Paths.FullPath.Should().Be($"{rootItem.FullPath}/{path}");
      phraseItem[Templates.DictionaryEntry.Fields.Phrase].Should().Be(defaultValue);
    }

    public class DictionaryEntryTemplate : DbTemplate
    {
      public DictionaryEntryTemplate():base(Templates.DictionaryEntry.ID)
      {
        this.Add(Templates.DictionaryEntry.Fields.Phrase);
      }
    }
  }
}
