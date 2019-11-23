﻿using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VerifyXunit;
using Xunit;
using Xunit.Abstractions;

// Non-nullable field is uninitialized
#pragma warning disable CS8618

public class VerifyObjectSamples:
    VerifyBase
{
    async Task ChangeDefaultsPerVerification(object target)
    {
        #region ChangeDefaultsPerVerification

        DontIgnoreEmptyCollections();
        DontScrubGuids();
        DontScrubDateTimes();
        DontIgnoreFalse();
        await Verify(target);

        #endregion
    }

    [Fact]
    public async Task ScopedSerializer()
    {
        #region ScopedSerializer

        var person = new Person
        {
            GivenNames = "John",
            FamilyName = "Smith",
            Dob = new DateTime(2000, 10, 1),
        };
        DontScrubDateTimes();
        var serializerSettings = BuildJsonSerializerSettings();
        serializerSettings.DateFormatHandling = DateFormatHandling.MicrosoftDateFormat;
        await Verify(person, jsonSerializerSettings: serializerSettings);

        #endregion
    }

    async Task Before()
    {
        #region Before

        var person = new Person
        {
            GivenNames = "John",
            FamilyName = "Smith",
            Spouse = "Jill",
            Address = new Address
            {
                Street = "1 Puddle Lane",
                Country = "USA"
            }
        };

        await Verify(person);

        #endregion
    }

    [Fact]
    public async Task Anon()
    {
        #region Anon

        var person1 = new Person
        {
            GivenNames = "John",
            FamilyName = "Smith"
        };
        var person2 = new Person
        {
            GivenNames = "Marianne",
            FamilyName = "Aguirre"
        };

        await Verify(
            new
            {
                person1,
                person2
            });

        #endregion
    }

    void ApplyExtraSettings()
    {
        #region ExtraSettings

        base.ApplyExtraSettings(jsonSerializerSettings =>
        {
            jsonSerializerSettings.DateFormatHandling = DateFormatHandling.MicrosoftDateFormat;
            jsonSerializerSettings.TypeNameHandling = TypeNameHandling.All;
        });

        #endregion
    }

    async Task After()
    {
        #region After

        var person = new Person
        {
            GivenNames = "John",
            FamilyName = "Smith",
            Spouse = "Jill",
            Address = new Address
            {
                Street = "1 Puddle Lane",
                Suburb = "Gotham",
                Country = "USA"
            }
        };

        await Verify(person);

        #endregion
    }

    class Person
    {
        public string GivenNames;
        public string FamilyName;
        public string Spouse;
        public Address Address;
        public DateTime Dob;
    }

    class Address
    {
        public string Street;
        public string Country;
        public string Suburb;
    }

    public VerifyObjectSamples(ITestOutputHelper output) :
        base(output)
    {
    }
}