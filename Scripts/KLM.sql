--1) Create Database
CREATE DATABASE AircraftVisits

--2) Start Using the DB
USE AircraftVisits

--3)Dont allow whole table to be dropped
Create TRIGGER trCannotDelete
on database
for
drop_table
as
print 'you cannot drop tables in this database'
rollback

--4) Create Leg Table
CREATE TABLE [dbo].[Leg](
	[LegId] [int] NOT NULL,
	[AirlineCode] [char](2) NOT NULL,
	[AircraftRegistration] [char](5) NOT NULL,
	[FlightNumber] [smallint] NOT NULL,
	[Suffix] [char](1) NULL,
	[DepartureStation] [char](3) NOT NULL,
	[ArrivalStation] [char](3) NOT NULL,
	[STDDate] [datetime] NULL,
	[STDTime] [nchar](15) NULL,
	[ATDDate] [datetime] NULL,
	[ATDTime] [nchar](15) NULL,
	[STDDateLocal] [datetime] NULL,
	[STDTimeLocal] [nchar](15) NULL,
	[ATDDateLocal] [datetime] NULL,
	[ATDTimeLocal] [nchar](15) NULL,
	[STADate] [datetime] NULL,
	[STATime] [nchar](15) NULL,
	[ATADate] [datetime] NULL,
	[ATATime] [nchar](15) NULL,
	[STADateLocal] [datetime] NULL,
	[STATimeLocal] [nchar](15) NULL,
	[ATADateLocal] [datetime] NULL,
	[ATATimeLocal] [nchar](15) NULL,
	[ATDDateInAMSLocalTime] [datetime] NULL
) ON [PRIMARY]

--5) Create Airport Details table
CREATE TABLE SysAirportCityDim (
	[AirportCode] [char](3) NULL,
	[AirportName] [char](200) NULL,
	[FromDate] [datetime] NULL,
	[ToDate] [datetime] NULL,
	[IsoCountryCode] [char](3) NULL,
	[CountryName] [char](50) NULL,
	[CountryTimeZone] [nchar](2) NULL
) ON [PRIMARY]

--6) Create Table for GMT difference
CREATE TABLE SysGmtDiff (
	[CountryCode] [char](3) NULL,
	[CountryZone] [nchar](2) NULL,
	[GmtStartTms] [datetime] NULL,
	[GmtEndTms] [datetime] NULL,
	[GmtDiff] [smallint] NULL,
	[OffsetMinutes] [smallint] NULL
) ON [PRIMARY]

--7)Use Sql Server Import functionality to upload data to Table - SysAirportCityDim & SysGmtDiff
--Data to be uploaded is kept in folder path - "DV/FileToUpload"

-- 8) Create function to Convert DateTime -- Local-->TO-->UTC  & UTC-->TO-->Local
CREATE FUNCTION [dbo].[fun_ConvertDateTime]
(
	@QueryType char(10),
	@AirportCode char(3),
	@LocalDate datetime,
	@LocalTime nchar(15)
) 
RETURNS @ResultTable TABLE
( 
	dateOutput datetime,
	timeOutput nchar(15)
) 
AS 
BEGIN
	DECLARE @ATAdt datetime, @dateOutput datetime
	SET @ATAdt = CAST(@LocalDate + ' ' + @LocalTime as datetime)

	DECLARE @CountryTimeZone varchar(3),@countryCode varchar(3)
	DECLARE @offSetInMin smallint

	select @CountryTimeZone=LTRIM(RTRIM(CountryTimeZone)),@countryCode=LTRIM(RTRIM(IsoCountryCode)) from SysAirportCityDim where AirportCode=@AirportCode

	Select @offSetInMin=OffsetMinutes from SysGmtDiff where LTRIM(RTRIM(CountryCode))=@countryCode AND LTRIM(RTRIM(CountryZone))=@CountryTimeZone and @ATAdt BETWEEN GmtStartTms AND GmtEndTms
	
	IF (@QueryType = 'LocalToUTC')
	BEGIN
		Select @dateOutput=DATEADD(mi, -@offSetInMin, @ATAdt)
	END
	ELSE IF (@QueryType = 'UTCToLocal')
	BEGIN
		Select @dateOutput=DATEADD(mi, @offSetInMin, @ATAdt)
	END

	INSERT INTO @ResultTable
		SELECT  convert(varchar, @dateOutput, 23), convert(varchar,@dateOutput, 8)
                
        
RETURN
END

--9) Create SP to Insert Leg into the Table with incremental Updates
CREATE PROC [dbo].[InsertIntoLeg]
(
	@LegId INT, 
	@AirlineCode CHAR(2), 
	@AircraftRegistration CHAR(5), 
	@FlightNumber smallint, 
	@Suffix CHAR(1), 
	@DepartureStation CHAR(3), 
	@ArrivalStation CHAR(3),
	@STDDate datetime = NULL,
	@STDTime nchar(15) = NULL,
	@ATDDate datetime = NULL,
	@ATDTime nchar(15) = NULL,
	@STDDateLocal datetime = NULL,
	@STDTimeLocal nchar(15) = NULL,
	@ATDDateLocal datetime = NULL,
	@ATDTimeLocal nchar(15) = NULL,
	@STADate datetime = NULL,
	@STATime nchar(15) = NULL,
	@ATADate datetime = NULL,
	@ATATime nchar(15) = NULL,
	@STADateLocal  datetime = NULL,
	@STATimeLocal nchar(15) = NULL,
	@ATADateLocal  datetime = NULL,
	@ATATimeLocal nchar(15) = NULL	/**/
)
AS
BEGIN
	--Fill Dep and Arrival Time in UTC from local time - To get correct VisitDuration
	IF (@ATDDate IS NULL AND @ATDDateLocal IS NOT NULL)
	BEGIN
		Select @ATDDate=dateOutput,@ATDTime=timeOutput from fun_ConvertDateTime('LocalToUTC', @DepartureStation,@ATDDateLocal,@ATDTimeLocal)
	END
	IF (@ATADate IS NULL AND @ATDDateLocal IS NOT NULL)
	BEGIN
		Select @ATADate=dateOutput,@ATATime=timeOutput from fun_ConvertDateTime('LocalToUTC',@DepartureStation,@ATADateLocal,@ATATimeLocal)
	END

	--Fill ATDDateInAMSLocalTime Column
	DECLARE @ATDDateInAMSLocalTime datetime
	IF (@ATDDate IS NOT NULL)
	BEGIN
		Select @ATDDateInAMSLocalTime =CAST(dateOutput + ' ' + timeOutput as datetime) from fun_ConvertDateTime('UTCToLocal', 'AMS',@ATDDate,@ATDTime)
	END

	IF NOT EXISTS(Select 1 from Leg where LegId=@LegId)
	BEGIN
		INSERT INTO Leg(LegId, AirlineCode, AircraftRegistration, FlightNumber, Suffix, DepartureStation, ArrivalStation,
		STDDate, STDTime,ATDDate,ATDTime,STDDateLocal,STDTimeLocal,ATDDateLocal,ATDTimeLocal,STADate,STATime,ATADate,ATATime,STADateLocal,
		STATimeLocal,ATADateLocal,ATATimeLocal,ATDDateInAMSLocalTime)
		VALUES(@LegId, @AirlineCode, @AircraftRegistration, @FlightNumber, @Suffix, @DepartureStation, @ArrivalStation,
		@STDDate, @STDTime,@ATDDate,@ATDTime,@STDDateLocal,@STDTimeLocal,@ATDDateLocal,@ATDTimeLocal,@STADate,@STATime,@ATADate,@ATATime,@STADateLocal,
		@STATimeLocal,@ATADateLocal,@ATATimeLocal,@ATDDateInAMSLocalTime);
	END
	ELSE
	BEGIN
		UPDATE Leg
		SET
			AirlineCode = @AirlineCode, 
			AircraftRegistration = @AircraftRegistration, 
			FlightNumber = @FlightNumber, 
			Suffix = @Suffix, 
			DepartureStation = @DepartureStation, 
			ArrivalStation = @ArrivalStation, 
			STDDate = @STDDate, 
			STDTime = @STDTime, 
			ATDDate = @ATDDate, 
			ATDTime = @ATDTime, 
			STDDateLocal = @STDDateLocal, 
			STDTimeLocal = @STDTimeLocal, 
			ATDDateLocal = @ATDDateLocal, 
			ATDTimeLocal = @ATDTimeLocal, 
			STADate = @STADate, 
			STATime = @STATime, 
			ATADate = @ATADate, 
			ATATime = @ATATime, 
			STADateLocal = @STADateLocal, 
			STATimeLocal = @STATimeLocal, 
			ATADateLocal = @ATADateLocal, 
			ATATimeLocal = @ATATimeLocal,
			ATDDateInAMSLocalTime = @ATDDateInAMSLocalTime
		WHERE LegId=@LegId
	END
END

--10) Create SP to GetVisits by Date and Reg_No. & GetVisits By Flight_Number

CREATE PROC [dbo].[GetVisits]
(
	@FilterBy CHAR(12),
	@AircraftReg [char](5) = NULL,
	@DepDate datetime = NULL,
	@FlightNum VARCHAR(6) = NULL
)
AS
BEGIN

CREATE TABLE #LegTemp(
	[LegId] [int] NOT NULL,
	[AircraftRegistration] [char](5) NOT NULL,
	[FlightNumber] [nchar](6) NOT NULL,
	[DepartureStation] [char](3) NOT NULL,
	[ArrivalStation] [char](3) NOT NULL,
	[DepDate] [datetime] NOT NULL,
	[ArrDate] [datetime] NOT NULL,
	[ATDDateInAMSLocalTime] [datetime] NULL,
) ON [PRIMARY]

--Below Query Simplifies our DataSet
Insert Into #LegTemp (LegId,AircraftRegistration,FlightNumber,DepartureStation,ArrivalStation,DepDate,ArrDate,ATDDateInAMSLocalTime)
		Select LegId,AircraftRegistration,AirlineCode+RIGHT('0'+ CONVERT(VARCHAR,FlightNumber),4) AS FlightNum,DepartureStation,ArrivalStation,
		cast(CASE 
				WHEN ATDDate IS NOT NULL THEN ATDDate
				WHEN ATDDate IS NULL THEN ATDDateLocal
		END +' '+
		CASE 
				WHEN ATDTime IS NULL THEN ATDTimeLocal
				WHEN ATDTime IS NOT NULL THEN ATDTime
		 
		END as datetime) AS DepDate,
		cast(CASE 
				WHEN ATADate IS NOT NULL THEN ATADate
				WHEN ATADate IS NULL THEN ATADateLocal
		END +' '+
		CASE 
				WHEN ATATime IS NULL THEN ATATimeLocal
				WHEN ATATime IS NOT NULL THEN ATATime
		 
		END as datetime) AS ArrDate,
		ATDDateInAMSLocalTime
		from Leg as L 
		Where  ((@FilterBy = 'RegAndDate' AND L.AircraftRegistration=@AircraftReg)
				OR
			(@FilterBy = 'FlightNum')) AND NOT (L.ATDDate IS NULL AND L.ATDDateLocal IS NULL) AND NOT (L.ATADate IS NULL AND L.ATADateLocal IS NULL) AND L.DepartureStation!=L.ArrivalStation
		ORDER BY L.STDDate, L.STDTIME ASC
		
	---Below Query JOINS and Filter the data
	IF @FilterBy = 'RegAndDate'
	BEGIN
		Select  IB_LegId,IB_AircraftReg, IB_FlightNumber, IB_DepartureStation, IB_ArrivalStation, IB_DepDateInUTC,IB_ArrDateInUTC,
				OB_LegId, OB_AircraftReg, OB_FlightNumber, OB_DepartureStation, OB_ArrivalStation, OB_DepDateInUTC, OB_ArrDateInUTC,
				ATDDateInAMSLocalTime,VisitDurationInMinutes,VisitStation,SEQ from (
			SELECT L1.LegId as IB_LegId,L1.AircraftRegistration as IB_AircraftReg, L1.FlightNumber as IB_FlightNumber, L1.DepartureStation as IB_DepartureStation, L1.ArrivalStation as IB_ArrivalStation, L1.DepDate as IB_DepDateInUTC, L1.ArrDate as IB_ArrDateInUTC,
				L2.LegId as OB_LegId, L2.AircraftRegistration as OB_AircraftReg, L2.FlightNumber as OB_FlightNumber, L2.DepartureStation as OB_DepartureStation, L2.ArrivalStation as OB_ArrivalStation, L2.DepDate as OB_DepDateInUTC, L2.ArrDate as OB_ArrDateInUTC,
				L1.ATDDateInAMSLocalTime,
				DATEDIFF(minute, L1.ArrDate, L2.DepDate) as VisitDurationInMinutes,
				L2.DepartureStation As VisitStation,
				dense_rank() OVER (ORDER BY L1.ATDDateInAMSLocalTime ASC) SEQ,
				ROW_NUMBER() OVER(PARTITION BY L1.LegId ORDER BY L2.DepDate ASC) AS RN
			FROM #LegTemp L1 RIGHT JOIN #LegTemp L2 
			ON L1.ArrivalStation=L2.DepartureStation 
			WHERE CAST(L2.DepDate as datetime) >= CAST(L1.ArrDate as datetime)
				AND CONVERT(varchar, L1.DepDate, 23) = CAST(@DepDate as datetime)
				AND L1.DepartureStation IS NOT NULL AND L2.DepartureStation IS NOT NULL
		) As Visits 
		Where Visits.RN=1 
		ORDER by Visits.IB_DepDateInUTC asc
	END
	ELSE IF @FilterBy = 'FlightNum'
	BEGIN
		Select  IB_LegId,IB_AircraftReg, IB_FlightNumber, IB_DepartureStation, IB_ArrivalStation, IB_DepDateInUTC,IB_ArrDateInUTC,
				OB_LegId, OB_AircraftReg, OB_FlightNumber, OB_DepartureStation, OB_ArrivalStation, OB_DepDateInUTC, OB_ArrDateInUTC,
				ATDDateInAMSLocalTime,VisitDurationInMinutes,VisitStation,SEQ from (
			SELECT L1.LegId as IB_LegId,L1.AircraftRegistration as IB_AircraftReg, L1.FlightNumber as IB_FlightNumber, L1.DepartureStation as IB_DepartureStation, L1.ArrivalStation as IB_ArrivalStation, L1.DepDate as IB_DepDateInUTC, L1.ArrDate as IB_ArrDateInUTC,
				L2.LegId as OB_LegId, L2.AircraftRegistration as OB_AircraftReg, L2.FlightNumber as OB_FlightNumber, L2.DepartureStation as OB_DepartureStation, L2.ArrivalStation as OB_ArrivalStation, L2.DepDate as OB_DepDateInUTC, L2.ArrDate as OB_ArrDateInUTC,
				L1.ATDDateInAMSLocalTime,
				DATEDIFF(minute, L1.ArrDate, L2.DepDate) as VisitDurationInMinutes,L2.DepartureStation As VisitStation,
				ROW_NUMBER() OVER(PARTITION BY L1.AircraftRegistration,convert(varchar, L1.ATDDateInAMSLocalTime, 23),L1.ArrivalStation ORDER BY L1.ATDDateInAMSLocalTime,L2.DepDate ASC) AS SEQ,
				ROW_NUMBER() OVER(PARTITION BY L1.LegId ORDER BY L2.DepDate ASC) AS RN
			FROM #LegTemp L1 RIGHT JOIN #LegTemp L2 
			ON L1.ArrivalStation=L2.DepartureStation AND L1.AircraftRegistration=L2.AircraftRegistration
			WHERE L1.FlightNumber=@FlightNum AND CAST(L2.DepDate as datetime) >= CAST(L1.ArrDate as datetime)
				AND L1.DepartureStation IS NOT NULL AND L2.DepartureStation IS NOT NULL
		) As Visits 
		Where Visits.RN=1
		ORDER by Visits.IB_DepDateInUTC asc
	END
END

