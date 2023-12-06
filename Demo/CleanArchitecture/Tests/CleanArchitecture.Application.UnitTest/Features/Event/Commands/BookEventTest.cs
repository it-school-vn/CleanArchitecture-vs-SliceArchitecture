using System.Linq.Expressions;
using MediatR;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework.Internal;
using CleanArchitecture.Application.BusinessServices;
using CleanArchitecture.Application.Core.Abstraction.Services;
using CleanArchitecture.Application.Core.CustomExceptions;
using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Domain.Models.Event;
using CleanArchitecture.Domain.Models.Event.DTO.DomainEvents;
using CleanArchitecture.Domain.Models.Event.ValueObjects;
using static CleanArchitecture.Application.Features.Event.Commands.BookEvent;

namespace CleanArchitecture.Application.UnitTest.Features.Event.Commands;

[TestFixture]
public class BookEventTest
{
    private IGenericService<EventEntity> _eventService;
    private IGenericService<EventAttendeeEntity> _attendeeService;
    private ICurrentUser _currentUser;
    private IMediator _sender;
    private Handler _handler;
    private Randomizer _randomizer;

    [SetUp]
    public void Setup()
    {
        _eventService = Substitute.For<IGenericService<EventEntity>>();
        _attendeeService = Substitute.For<IGenericService<EventAttendeeEntity>>();
        _currentUser = Substitute.For<ICurrentUser>();
        _sender = Substitute.For<IMediator>();

        _randomizer = new Randomizer();
        _handler = new Handler(
            _eventService,
            _currentUser,
            _attendeeService,
            _sender
        );
    }


    private EventEntity CreateAnEvent()
    {
        return new EventEntity
        {
            Id = Ulid.NewUlid(),
            OwnerId = Guid.NewGuid(),
            TimeZone = 0,
            OpenDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-5)),
            OpenTime = TimeOnly.FromDateTime(DateTime.Now),
            ClosedDate = DateOnly.FromDateTime(DateTime.Now),
            ClosedTime = TimeOnly.FromDateTime(DateTime.Now),
            Fee = _randomizer.NextDecimal(),
            Title = _randomizer.GetString(25),
            Description = _randomizer.GetString(200),
            FeeRate = _randomizer.NextDecimal(),
            EventType = Domain.Models.Event.Enum.EventType.Event,
            Conference = new ConferenceOption(MeetingType.Online, ConferenceTool.Zoom, _randomizer.GetString(200), null, null),
            DateAt = DateOnly.FromDateTime(DateTime.Now.AddDays(20)),
            TimeAt = TimeOnly.FromDateTime(DateTime.Now),
            Duration = 60
        };
    }

    [Test]
    public void Validator_Validate_WhenIdIsNull_ReturnFalse()
    {
        // Arrange
        var validator = new Validator();
        var command = new Command();

        //Act

        var result = validator.Validate(command);

        // Assert

        Assert.IsFalse(result.IsValid);

    }

    [Test]
    public void Validator_Validate_WhenIdIsEmpty_ReturnFalse()
    {
        // Arrange
        var validator = new Validator();
        var command = new Command { Id = Ulid.Empty };

        //Act

        var result = validator.Validate(command);

        // Assert

        Assert.IsFalse(result.IsValid);

    }


    [Test]
    public async Task Handle_WhenEventExistsAndUserIsNotOwner_ReturnsOperationResultWithAttendeeId()
    {
        // Arrange

        var eventId = new Ulid();
        var userId = Guid.NewGuid();
        EventEntity eventEntity = CreateAnEvent();

        eventEntity.Id = eventId;

        var request = new Command { Id = eventId };
        _eventService.GetByAsync(Arg.Any<Expression<Func<EventEntity, bool>>>(), false, CancellationToken.None)
            .Returns(eventEntity);
        _currentUser.GetUserId().Returns(userId);
        _attendeeService.AnyAsync(Arg.Any<Expression<Func<EventAttendeeEntity, bool>>>(), CancellationToken.None)
            .Returns(false);
        _attendeeService.InsertAsync(Arg.Any<EventAttendeeEntity>(), CancellationToken.None)
            .Returns(new EventAttendeeEntity { Id = new Ulid() });

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.IsTrue(result.Succeeded);

        Assert.IsNotNull(result.Value);
        await _attendeeService.Received(1).InsertAsync(Arg.Any<EventAttendeeEntity>(), CancellationToken.None);
        await _sender.Received(1).Publish(Arg.Any<BookEventDomainEvent>(), CancellationToken.None);
    }

    [Test]
    public void Handle_WhenEventDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var eventId = new Ulid();
        var request = new Command { Id = eventId };

        _eventService.GetByAsync(Arg.Any<Expression<Func<EventEntity, bool>>>(), false, CancellationToken.None)
            .ReturnsNullForAnyArgs();

        // Act & Assert
        Assert.ThrowsAsync<NotFoundException>(async () => await _handler.Handle(request, CancellationToken.None));
    }

    [Test]
    public void Handle_WhenUserIsOwner_ThrowsBadRequestException()
    {
        // Arrange
        var eventId = new Ulid();
        var userId = Guid.NewGuid();
        var eventEntity = CreateAnEvent();

        eventEntity.Id = eventId;
        eventEntity.OwnerId = userId;
        var request = new Command { Id = eventId };

        _eventService.GetByAsync(Arg.Any<Expression<Func<EventEntity, bool>>>(), false, CancellationToken.None)
            .Returns(eventEntity);

        _currentUser.GetUserId().Returns(userId);

        // Act & Assert
        Assert.ThrowsAsync<BadRequestException>(async () => await _handler.Handle(request, CancellationToken.None));
    }

    [Test]
    public void Handle_WhenEventNotOpened_ThrowsBadRequestException()
    {
        // Arrange
        var eventId = new Ulid();
        var userId = Guid.NewGuid();
        var eventEntity = CreateAnEvent();

        eventEntity.Id = eventId;
        eventEntity.TimeZone = 0;
        eventEntity.OpenDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
        eventEntity.OpenTime = new TimeOnly(12, 0);
        eventEntity.ClosedDate = DateOnly.FromDateTime(DateTime.Now.AddDays(5));
        eventEntity.ClosedTime = new TimeOnly(10, 0);


        var request = new Command { Id = eventId };
        _eventService.GetByAsync(Arg.Any<Expression<Func<EventEntity, bool>>>(), false, CancellationToken.None)
            .Returns(eventEntity);
        _currentUser.GetUserId().Returns(userId);

        // Act & Assert
        Assert.ThrowsAsync<BadRequestException>(async () => await _handler.Handle(request, CancellationToken.None));
    }

    [Test]
    public void Handle_WhenEventClosed_ThrowsBadRequestException()
    {
        // Arrange
        var eventId = new Ulid();
        var userId = Guid.NewGuid();
        var eventEntity = CreateAnEvent();

        eventEntity.Id = eventId;
        eventEntity.TimeZone = 0;
        eventEntity.OpenDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-5));
        eventEntity.OpenTime = new TimeOnly(12, 0);
        eventEntity.ClosedDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-5));
        eventEntity.ClosedTime = new TimeOnly(10, 0);

        var request = new Command { Id = eventId };
        _eventService.GetByAsync(Arg.Any<Expression<Func<EventEntity, bool>>>(), false, CancellationToken.None)
            .Returns(eventEntity);
        _currentUser.GetUserId().Returns(userId);

        // Act & Assert
        Assert.ThrowsAsync<BadRequestException>(async () => await _handler.Handle(request, CancellationToken.None));
    }

    [Test]
    public async Task Handle_WhenUserAlreadyBooked_ReturnsAcceptedOperationResult()
    {
        // Arrange
        var eventId = new Ulid();
        var userId = Guid.NewGuid();
        var eventEntity = CreateAnEvent();
        eventEntity.Id = eventId;
        var request = new Command { Id = eventId };
        _eventService.GetByAsync(Arg.Any<Expression<Func<EventEntity, bool>>>(), false, CancellationToken.None)
            .Returns(eventEntity);
        _currentUser.GetUserId().Returns(userId);

        _attendeeService.AnyAsync(Arg.Any<Expression<Func<EventAttendeeEntity, bool>>>(), CancellationToken.None)
            .ReturnsForAnyArgs(true);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.IsTrue(result.Succeeded);
        Assert.True(result.Value == Ulid.Empty);
    }

}