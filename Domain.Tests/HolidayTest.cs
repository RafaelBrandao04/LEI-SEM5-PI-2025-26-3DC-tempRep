namespace Domain.Tests;

using Domain.Model;
using Domain.Factory;

public class HolidayTest
{

    [Fact]
    public void WhenPassingAColaborator_ThenHolidayIsInstantiated()
    {
        //Mock<Colaborator> colabDouble = new Mock<Colaborator>("a", "b@b.pt");
        Mock<IColaborator> colabDouble = new Mock<IColaborator>();

        new Holiday(colabDouble.Object);

        // isto não é um teste unitário a Holiday, porque não isola do Colaborator
        // Colaborator colab = new Colaborator("a", "a@b.c");
        // IColaborator colab = new Colaborator("a", "a@b.c");
        // new Holiday(colab);
    }

    [Fact]
    public void WhenPassingNullAsColaborator_ThenThrowsException()
    {
        Assert.Throws<ArgumentException>(() => new Holiday(null));
    }

    [Fact]
    public void WhenRequestingName_ThenReturnColaboratorName()
    {
        // arrange
        string NOME = "nome";
        Mock<IColaborator> colabDouble = new Mock<IColaborator>();
        colabDouble.Setup(p => p.GetName()).Returns(NOME);

        Holiday holiday = new Holiday(colabDouble.Object); // SUT/OUT

        // act
        string nameResult = holiday.GetName();

        // assert
        Assert.Equal(NOME, nameResult);
    }

    // [Fact]
    // public void WhenRequestingColaborator_ShouldReturnColaborator()
    // {
    //     // arrange
    //     Mock<IColaborator> colabDouble = new Mock<IColaborator>();
    //     Holiday holiday = new Holiday(colabDouble.Object);

    //     // act
    //     IColaborator colaborator = holiday.Colaborador;

    //     // assert
    //     Assert.Equal(colabDouble.Object, colaborator);
    // }

    [Fact]
    public void WhenRequestingCorrectHasColaborator_ShouldReturnTrue()
    {
        // arrange
        Mock<IColaborator> colabDouble = new Mock<IColaborator>();
        Holiday holiday = new Holiday(colabDouble.Object);

        // act
        bool hasColab = holiday.HasColaborador(colabDouble.Object);

        // assert
        Assert.True(hasColab);
    }


    [Fact]
    public void WhenRequestingHolidayPeriods_ShouldReturnHolidayPeriods()
    {
        // arrange
        Mock<IColaborator> colabDouble = new Mock<IColaborator>();
        Mock<IHolidayPeriodFactory> hpFactoryDouble = new Mock<IHolidayPeriodFactory>();
        Mock<IHolidayPeriodFactory> hpFactoryDouble2 = new Mock<IHolidayPeriodFactory>();
        
        var holiday = new Holiday(colabDouble.Object);        

        DateOnly startDate = new DateOnly(DateTime.Now.Year, 01, 01);
        DateOnly endDate = new DateOnly(DateTime.Now.Year, 12, 31);

        DateOnly startDateFirstHoliday = new DateOnly(DateTime.Now.Year, 02, 01);
        DateOnly endDateFirstHoliday=  new DateOnly(DateTime.Now.Year, 03, 20);
        DateOnly startDateSecondHoliday = new DateOnly(DateTime.Now.Year, 07, 31);
        DateOnly endDateSecondHoliday=  new DateOnly(DateTime.Now.Year, 08, 15);        


        Mock<HolidayPeriod> hpDouble1 = new Mock<HolidayPeriod>(startDateFirstHoliday, endDateFirstHoliday);
        Mock<HolidayPeriod> hpDouble2 = new Mock<HolidayPeriod>(startDateSecondHoliday, endDateSecondHoliday);

        hpFactoryDouble.Setup(hpF => hpF.NewHolidayPeriod(startDateFirstHoliday, endDateFirstHoliday)).Returns(hpDouble1.Object);
        hpFactoryDouble2.Setup(hpF => hpF.NewHolidayPeriod(startDateSecondHoliday, endDateSecondHoliday)).Returns(hpDouble2.Object);

        HolidayPeriod holidayPeriod1 = holiday.AddHolidayPeriod(hpFactoryDouble.Object, startDateFirstHoliday, endDateFirstHoliday);
        HolidayPeriod holidayPeriod2 = holiday.AddHolidayPeriod(hpFactoryDouble2.Object, startDateSecondHoliday, endDateSecondHoliday);

        List<HolidayPeriod> holidayPeriods = new List<HolidayPeriod> { holidayPeriod1, holidayPeriod2};

        // act
        List<HolidayPeriod> result = holiday.GetHolidayPeriodsDuring(startDate, endDate);

        // assert
        Assert.Equivalent(holidayPeriods, result);
    }

    public static readonly object[][] SucessCases =
    [
        [new DateOnly(DateTime.Now.Year,01,01), new DateOnly(DateTime.Now.Year,12,01)]
    ];
    [Theory, MemberData(nameof(SucessCases))]
    public void WhenRequestingaddHolidayPeriod_ThenReturnHolidayPeriod(DateOnly startDate, DateOnly endDate)
    {

        // arrange
        Mock<IColaborator> colabDouble = new Mock<IColaborator>();
        Holiday holidayDouble = new Holiday(colabDouble.Object);
        Mock<IHolidayPeriodFactory> hpFactoryDouble = new Mock<IHolidayPeriodFactory>();

        Mock<HolidayPeriod> holidayPeriodExpected = new Mock<HolidayPeriod>(startDate, endDate);

        hpFactoryDouble.Setup(hpF => hpF.NewHolidayPeriod(startDate, endDate)).Returns(holidayPeriodExpected.Object); // to isolate the result

        // act
        HolidayPeriod holidayPeriod = holidayDouble.AddHolidayPeriod(hpFactoryDouble.Object, startDate, endDate); // to get the actual result

        // assert
        Assert.Equivalent(holidayPeriodExpected.Object, holidayPeriod); // compare objects
    }

    [Fact]
    public void TestGetHolidayPeriodsDuring_OutOfRange()
    {
        // arrange
        Mock<IColaborator> colabDouble = new Mock<IColaborator>();
        Mock<IHolidayPeriodFactory> hpFactoryDouble = new Mock<IHolidayPeriodFactory>();
        Mock<IHolidayPeriodFactory> hpFactoryDouble2 = new Mock<IHolidayPeriodFactory>();
        
        var holiday = new Holiday(colabDouble.Object);

        DateOnly startDate = new DateOnly(DateTime.Now.Year, 01, 01);
        DateOnly endDate = new DateOnly(DateTime.Now.Year, 01, 31);

        DateOnly startDateFirstHoliday = new DateOnly(DateTime.Now.Year, 02, 01);
        DateOnly endDateFirstHoliday=  new DateOnly(DateTime.Now.Year, 03, 20);
        DateOnly startDateSecondHoliday = new DateOnly(DateTime.Now.Year, 07, 31);
        DateOnly endDateSecondHoliday=  new DateOnly(DateTime.Now.Year, 08, 15);        


        Mock<HolidayPeriod> hpDouble1 = new Mock<HolidayPeriod>(startDateFirstHoliday, endDateFirstHoliday);
        Mock<HolidayPeriod> hpDouble2 = new Mock<HolidayPeriod>(startDateSecondHoliday, endDateSecondHoliday);

        hpFactoryDouble.Setup(hpF => hpF.NewHolidayPeriod(startDateFirstHoliday, endDateFirstHoliday)).Returns(hpDouble1.Object);
        hpFactoryDouble2.Setup(hpF => hpF.NewHolidayPeriod(startDateSecondHoliday, endDateSecondHoliday)).Returns(hpDouble2.Object);

        HolidayPeriod holidayPeriod1 = holiday.AddHolidayPeriod(hpFactoryDouble.Object, startDateFirstHoliday, endDateFirstHoliday);
        HolidayPeriod holidayPeriod2 = holiday.AddHolidayPeriod(hpFactoryDouble2.Object, startDateSecondHoliday, endDateSecondHoliday);

        List<HolidayPeriod> holidayPeriods = new List<HolidayPeriod> { };

        // act
        List<HolidayPeriod> result = holiday.GetHolidayPeriodsDuring(startDate, endDate);

        // assert
        Assert.Equivalent(holidayPeriods, result);
    }

    [Fact]
    public void WhenPassingLowerNumberOfDays_GetCorrectNumberOfDays()
    {
        // arrange
        Mock<IColaborator> _colabDouble = new Mock<IColaborator>();
        Holiday _holiday = new Holiday(_colabDouble.Object);
        var hpFactoryDouble = new Mock<IHolidayPeriodFactory>();


        int expectedValue = 9;
        int numberOfDays = 5;

        DateOnly startDate = new DateOnly(DateTime.Now.Year, 02, 01);
        DateOnly endDate = new DateOnly(DateTime.Now.Year, 02, 10);

        HolidayPeriod holidayPeriodExpected = new HolidayPeriod(startDate, endDate);

        hpFactoryDouble.Setup(hpF => hpF.NewHolidayPeriod(startDate, endDate)).Returns(holidayPeriodExpected);// to isolate the result

        HolidayPeriod hp1 = _holiday.AddHolidayPeriod(hpFactoryDouble.Object, startDate, endDate);

        // act
        int numberOfDaysResult = _holiday.GetHolidaysDaysWithMoreThanXDaysOff(numberOfDays);

        // assert
        Assert.Equivalent(expectedValue, numberOfDaysResult);
    }


    [Fact]
    public void WhenPassingHigherNumberOfDays_GetZeroNumberOfDays()
    {
        // arrange
        Mock<IColaborator> _colabDouble = new Mock<IColaborator>();
        Holiday _holiday = new Holiday(_colabDouble.Object);
        var hpFactoryDouble = new Mock<IHolidayPeriodFactory>();


        int expectedValue = 0;
        int numberOfDays = 5;

        DateOnly startDate = new DateOnly(DateTime.Now.Year, 02, 01);
        DateOnly endDate = new DateOnly(DateTime.Now.Year, 02, 04);

        HolidayPeriod holidayPeriodExpected = new HolidayPeriod(startDate, endDate);

        hpFactoryDouble.Setup(hpF => hpF.NewHolidayPeriod(startDate, endDate)).Returns(holidayPeriodExpected);// to isolate the result

        HolidayPeriod hp1 = _holiday.AddHolidayPeriod(hpFactoryDouble.Object, startDate, endDate);

        // act
        int numberOfDaysResult = _holiday.GetHolidaysDaysWithMoreThanXDaysOff(numberOfDays);

        // assert
        Assert.Equivalent(expectedValue, numberOfDaysResult);
    }

    [Fact]
    public void WhenPassingHolidayPeriod_GetNumberOfDays()
    {
        // arrange
        Mock<IColaborator> _colabDouble = new Mock<IColaborator>();
        Holiday _holiday = new Holiday(_colabDouble.Object);
        var hpFactoryDouble = new Mock<IHolidayPeriodFactory>();

        int expectedValue = 9;

        DateOnly startDate = new DateOnly(DateTime.Now.Year, 02, 01);
        DateOnly endDate = new DateOnly(DateTime.Now.Year, 02, 10);

        HolidayPeriod holidayPeriodExpected = new HolidayPeriod(startDate, endDate);

        hpFactoryDouble.Setup(hpF => hpF.NewHolidayPeriod(startDate, endDate)).Returns(holidayPeriodExpected);// to isolate the result

        HolidayPeriod hp1 = _holiday.AddHolidayPeriod(hpFactoryDouble.Object, startDate, endDate);

        // act
        int numberOfDaysResult = _holiday.GetNumberOfHolidayPeriodsDays();

        // assert
        Assert.Equivalent(expectedValue, numberOfDaysResult);
    }

    // public bool hasHolidayPeriodsDuring(IColaborator colaborator, DateOnly startDate, DateOnly endDate)

    [Fact]
    public void WhenPassingHolidayPeriod_ShouldReturnTrue()
    {
        // arrange
        Mock<IColaborator> _colabDouble = new Mock<IColaborator>();
        Holiday _holiday = new Holiday(_colabDouble.Object);
        var hpFactoryDouble = new Mock<IHolidayPeriodFactory>();

        bool expectedResult = true;

        DateOnly startDate = new DateOnly(DateTime.Now.Year, 02, 01);
        DateOnly endDate = new DateOnly(DateTime.Now.Year, 02, 10);

        HolidayPeriod holidayPeriodExpected = new HolidayPeriod(startDate, endDate);

        hpFactoryDouble.Setup(hpF => hpF.NewHolidayPeriod(startDate, endDate)).Returns(holidayPeriodExpected);// to isolate the result

        HolidayPeriod hp1 = _holiday.AddHolidayPeriod(hpFactoryDouble.Object, startDate, endDate);

        // act
        bool result = _holiday.HasColaboratorAndHolidayPeriodsDuring(_colabDouble.Object, startDate, endDate);

        // assert
        Assert.Equivalent(expectedResult, result);
    }

    [Fact]
    public void WhenPassingOutRangeHolidayPeriod_ShouldReturnFalse()
    {
        // arrange
        Mock<IColaborator> _colabDouble = new Mock<IColaborator>();
        Holiday _holiday = new Holiday(_colabDouble.Object);
        var hpFactoryDouble = new Mock<IHolidayPeriodFactory>();

        bool expectedResult = false;

        DateOnly startDate = new DateOnly(DateTime.Now.Year, 02, 01);
        DateOnly endDate = new DateOnly(DateTime.Now.Year, 02, 10);
        DateOnly startDateOutRange = new DateOnly(DateTime.Now.Year, 03, 01);
        DateOnly endDateOutRange = new DateOnly(DateTime.Now.Year, 03, 10);

        HolidayPeriod holidayPeriodExpected = new HolidayPeriod(startDate, endDate);

        hpFactoryDouble.Setup(hpF => hpF.NewHolidayPeriod(startDate, endDate)).Returns(holidayPeriodExpected);// to isolate the result

        HolidayPeriod hp1 = _holiday.AddHolidayPeriod(hpFactoryDouble.Object, startDate, endDate);

        // act
        bool result = _holiday.HasColaboratorAndHolidayPeriodsDuring(_colabDouble.Object, startDateOutRange, endDateOutRange);

        // assert
        Assert.Equivalent(expectedResult, result);
    }
}
