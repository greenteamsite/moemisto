SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Tkachuk Olexander
-- Create date: 27.07.2015
-- Description:	Get schedules with times string
-- =============================================
CREATE PROCEDURE GetSchedules 
	@EventId int
AS
BEGIN
	SET NOCOUNT ON;

SELECT 
      pl.Title as PlaceTitle
	  ,pl.TranslitUrl as PlaceUrl
      ,[PriceFrom]
      ,[PriceTo]
      ,Cast(dts.StartEvent as date) as StartEvent
      ,STUFF((
    SELECT distinct ', ' + CONVERT(VARCHAR(5),dtsIn.StartEvent,108)
    FROM [dbo].EventScheduleDateTimes dtsIn
    WHERE (sch.EventScheduleId = dtsIn.EventScheduleId) 
    FOR XML PATH(''),TYPE).value('(./text())[1]','VARCHAR(MAX)')
  ,1,2,'') AS Times
  FROM [dbo].[EventSchedules] sch
  Inner Join [dbo].Places pl On pl.PlaceId = sch.PlaceId
  Inner Join [dbo].EventScheduleDateTimes dts On sch.EventScheduleId = dts.EventScheduleId 
  Where [EventId] = @EventId And dts.StartEvent > Getdate()
  Group By
	   sch.EventScheduleId 
	  ,pl.Title
	  ,pl.TranslitUrl
      ,[PriceFrom]
      ,[PriceTo]
      ,Cast(dts.StartEvent as date)
  Order By Cast(dts.StartEvent as date), pl.Title
END
