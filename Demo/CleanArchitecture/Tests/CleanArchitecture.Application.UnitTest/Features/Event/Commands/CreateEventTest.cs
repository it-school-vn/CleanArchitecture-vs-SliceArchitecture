using NSubstitute;
using AutoMapper;
using CleanArchitecture.Application.BusinessServices;
using CleanArchitecture.Application.Core.Abstraction.Services;
using CleanArchitecture.Application.Features.Event.Commands;
using CleanArchitecture.Domain.Models.Event;
using CleanArchitecture.Domain.Models.Event.DTO;
using NUnit.Framework.Internal;
using CleanArchitecture.Domain.Models.Event.ValueObjects;
using CleanArchitecture.Domain.Enums;
using static CleanArchitecture.Application.Features.Event.Commands.CreateEvent;

namespace CleanArchitecture.Application.UnitTest.Features.Event.Commands;
[TestFixture]
public class CreateEventTests
{
    private IGenericService<EventEntity> _eventService;
    private ICurrentUser _currentUser;
    private IMapper _mapper;
    private CreateEvent.Handler _handler;
    private Randomizer _randomizer;
    [SetUp]
    public void Setup()
    {
        _eventService = Substitute.For<IGenericService<EventEntity>>();
        _currentUser = Substitute.For<ICurrentUser>();
        var mapperConfiguration = new MapperConfiguration(cfg =>
                       {
                           cfg.AddProfile<MappingProfile>();
                       });

        _mapper = mapperConfiguration.CreateMapper();

        _randomizer = new Randomizer();
        _handler = new CreateEvent.Handler(_eventService, _currentUser, _mapper);
    }

    [Test]
    public async Task Handle_ValidRequest_ReturnsOperationResultWithId()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var request = new CreateEvent.Command
        {
            Request = new CreateOrUpdateEventRequest
            {
                OpenDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-5)),
                OpenTime = TimeOnly.FromDateTime(DateTime.Now),
                ClosedDate = DateOnly.FromDateTime(DateTime.Now),
                ClosedTime = TimeOnly.FromDateTime(DateTime.Now),
                Fee = _randomizer.NextDecimal(),
                Title = _randomizer.GetString(25),
                Description = _randomizer.GetString(200),
                EventType = Domain.Models.Event.Enum.EventType.Event,
                Conference = new ConferenceOption(MeetingType.Online, ConferenceTool.Zoom, _randomizer.GetString(200), null, null),
                DateAt = DateOnly.FromDateTime(DateTime.Now.AddDays(20)),
                TimeAt = TimeOnly.FromDateTime(DateTime.Now),
                Duration = 60
            }
        };


        var eventEntity = _mapper.Map<EventEntity>(request.Request);

        eventEntity.Id = Ulid.NewUlid();

        _currentUser.GetUserId().Returns(userId);

        _eventService.InsertAsync(eventEntity, CancellationToken.None).ReturnsForAnyArgs(eventEntity);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.That(result.Succeeded, Is.True);
        Assert.That(result.Value, Is.EqualTo(eventEntity.Id));
    }
    [Test]
    public void Validator_Validate_WhenRequestIsNull_ReturnFalse()
    {
        // Arrange
        var validator = new Validator();
        var command = new Command();

        //Act

        var result = validator.Validate(command);

        // Assert

        Assert.IsFalse(result.IsValid);

    }

}
